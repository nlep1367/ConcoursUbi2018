using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    public Color lineColor;
    public bool display = true;
    public bool isLooping;

    private List<Transform> wayPoints = new List<Transform>();

    void OnDrawGizmos()
    {
        if (display)
        {
            Transform[] pathTransforms = GetComponentsInChildren<Transform>();
            wayPoints = new List<Transform>();

            for (int i = 0; i < pathTransforms.Length; ++i)
            {
                if (pathTransforms[i] != transform)
                {
                    wayPoints.Add(pathTransforms[i]);
                }
            }

            if (isLooping)
            {
                DrawLooping(wayPoints);
            }
            else
            {
                DrawOneWay(wayPoints);
            }
        }     
    }

    void DrawLooping(List<Transform> wayPoints)
    {
        Gizmos.color = lineColor;

        for (int i = 0; i < wayPoints.Count; ++i)
        {
            Vector3 current = wayPoints[i].position;
            Vector3 previous = Vector3.zero;

            if (i > 0)
            {
                previous = wayPoints[i - 1].position;
            }
            else if (i == 0 && wayPoints.Count > 1)
            {
                previous = wayPoints[wayPoints.Count - 1].position;
            }

            Gizmos.DrawLine(previous, current);
            Gizmos.DrawWireSphere(current, 0.3f);
        }
    }

    void DrawOneWay(List<Transform> wayPoints)
    {
        Gizmos.color = Color.green;

        Vector3 previous = wayPoints[0].position;
        Gizmos.DrawWireSphere(previous, 0.3f);

        Gizmos.color = lineColor;

        for (int i = 1; i < wayPoints.Count; ++i)
        {
            Vector3 current = wayPoints[i].position;

            Gizmos.DrawLine(previous, current);
            Gizmos.DrawWireSphere(current, 0.3f);

            previous = current;
        }
    }

}
