using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Linq;

public abstract class Player : NetworkBehaviour
{
    private List<GameObject> respawnPoints = new List<GameObject>();

    protected Camera Camera;
    public Rigidbody RigidBody;

    public PlayerState State;
    public PlayerState PreviousState;

    public Dictionary<StateEnum, PlayerState> States;

    public Animator Animator;

    void Start()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("DogRespawn");
        respawnPoints.AddRange(temp);
        respawnPoints = respawnPoints.OrderBy(x => x.transform.localPosition.y).ToList();
    }

    // Use this for initialization
    public virtual void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        State.InterpretInput();

        if (GetComponent<ObjectSync>().hasAuthority)
        {
            int index = -1;
            if (Input.GetKeyDown(KeyCode.F1))
            {
                index = 0;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                index = 1;
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                index = 2;
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                index = 3;
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                index = 4;
            }
            else if (Input.GetKeyDown(KeyCode.F6))
            {
                index = 5;
            }

            if (index != -1)
            {
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gameObject.transform.position = respawnPoints[index].transform.position;
            }
        }
    }

    public void ChangeState(StateEnum newStateEnum)
    {
        ChangeState(newStateEnum, null);
    }

    public void ChangeState(StateEnum newStateEnum, Object o)
    {
        if (States[newStateEnum] != State)
        {
            PreviousState = State;
            State.ChangeState(States[newStateEnum], o);
        }
    }

    public bool IsState(StateEnum stateToTest)
    {
        return State == States[stateToTest];
    }

    public abstract void SetCamera(Camera camera);

    public bool isActivePlayer()
    {
        return GetComponentInChildren<Camera>() == null;
    }
}
