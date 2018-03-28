using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DogBark : NetworkBehaviour {
    private DogBarkEcho Echo;
    public Action<Vector3> HasBarked;

    private Color green;
    private Color yellow;
    private Color red;

    public float Cooldown = 10f;
    private float _time = 10f;

    private void Start()
    {
        green.r = 66.0f / 255;
        green.g = 142f / 255;
        green.b = 64f / 255;
        green.a = 1;

        yellow.r = 196.0f / 255;
        yellow.g = 178.0f / 255;
        yellow.b = 59.0f / 255;
        yellow.a = 1;

        red.r = 164.0f / 255;
        red.g = 25.0f / 255;
        red.b = 15.0f / 255;
        red.a = 1;
    }

    // Update is called once per frame
    void Update () {
        if (!hasAuthority)
            return;

        if (Time.time > Cooldown + _time)
        {
            if (Input.GetButtonDown("Y") && GameEssentials.PlayerDog.IsState(StateEnum.GROUNDED))
            {
                Cmd_StartBark(green);
                GetComponent<EchoSoundsControl>().BarkJoyfully();
                GameEssentials.PlayerDog.ChangeState(StateEnum.BARKING);
                _time = Time.time;
            }

            /*
            if (Input.GetButtonDown("Y"))
            {
                Cmd_StartBark(yellow);
                GameEssentials.PlayerDog.ChangeState(StateEnum.BARKING);
            }
            */

            if (Input.GetButtonDown("B") && GameEssentials.PlayerDog.IsState(StateEnum.GROUNDED))
            {
                Cmd_StartBark(red);
                GetComponent<EchoSoundsControl>().BarkAggressively();
                GameEssentials.PlayerDog.ChangeState(StateEnum.BARKING);
                _time = Time.time;

                if (HasBarked != null)
                    HasBarked.Invoke(transform.position);
            }
        }
    }

    [Command]
    public void Cmd_StartBark(Color color)
    {
        if (!Echo)
            Echo = GameObject.FindGameObjectWithTag("Fille").GetComponent<DogBarkEcho>();

        Echo.StartBark(color);
        RpcBarkVisible(color);

        if (HasBarked != null)
            HasBarked.Invoke(transform.position);
    }

    [ClientRpc]
    void RpcBarkVisible(Color color)
    {
        GameObject PlaneBark = GameObject.FindGameObjectWithTag("DogEcho");
        if (PlaneBark)
        {
            Vector3 pos = GameObject.FindGameObjectWithTag("Doggo").transform.position;
            pos.y += 0.1f;
            PlaneBark.transform.position = pos;
            Renderer Rend = PlaneBark.GetComponent<Renderer>();
            if (Rend)
            {
                Rend.material.SetColor("_Color", color);
                Rend.material.SetVector("_EchoCenter", pos);
                Rend.material.SetFloat("_EchoRadius", Adaptation.MaximumBarkRadius);
                Rend.enabled = true;
            }
        }
    }
}
