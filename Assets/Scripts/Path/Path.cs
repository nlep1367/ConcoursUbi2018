using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    public Color lineColor;
    public bool display = true;
    public bool isLooping = false;

    private List<WayPoint> wayPoints;

    public WayPoint GetWayPoint(int index)
    {
        return wayPoints[index];
    }

    public int GetNextWayPoint(int index)
    {
        ++index;

        if (isLooping)
        {
            return (index) % wayPoints.Count;
        }
        else if(index < wayPoints.Count)
        {
            return index;
        }

        return -1; // Not looping and reached the end of the path
    }

    private void Start()
    {
        wayPoints = new List<WayPoint>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i) != transform)
            {
                wayPoints.Add(transform.GetChild(i).GetComponent<WayPoint>());
            }
        }

    }

    void OnDrawGizmos()
    {
        if (display)
        {            
            wayPoints = new List<WayPoint>();

            for (int i = 0; i < transform.childCount; ++i)
            {
                if (transform.GetChild(i) != transform)
                {
                    wayPoints.Add(transform.GetChild(i).GetComponent<WayPoint>());
                    if (!Application.isPlaying) wayPoints[i].name = "WayPoint_" + i.ToString();
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

    void DrawLooping(List<WayPoint> wayPoints)
    {
        Gizmos.color = lineColor;

        for (int i = 0; i < wayPoints.Count; ++i)
        {
            Vector3 current = wayPoints[i].transform.position;
            Vector3 previous = Vector3.zero;

            if (i > 0)
            {
                previous = wayPoints[i - 1].transform.position;
            }
            else if (i == 0 && wayPoints.Count > 1)
            {
                previous = wayPoints[wayPoints.Count - 1].transform.position;
            }

            Gizmos.DrawLine(previous, current);
            Gizmos.DrawWireSphere(current, 0.3f);
        }
    }

    void DrawOneWay(List<WayPoint> wayPoints)
    {
        Gizmos.color = Color.green;

        Vector3 previous = wayPoints[0].transform.position;
        Gizmos.DrawWireSphere(previous, 0.3f);

        Gizmos.color = lineColor;

        for (int i = 1; i < wayPoints.Count; ++i)
        {
            Vector3 current = wayPoints[i].transform.position;

            Gizmos.DrawLine(previous, current);
            Gizmos.DrawWireSphere(current, 0.3f);

            previous = current;
        }
    }

}
