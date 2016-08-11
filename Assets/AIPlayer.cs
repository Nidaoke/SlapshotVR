 using UnityEngine;
using System.Collections;

public class AIPlayer : MonoBehaviour
{

    // Movement variables
    public Transform[] m_Waypoints;
    float m_WaypointsTotal;
    public int m_NextWaypoint;
    public int m_LastWaypoint;

    // Adjustable speeds
    public float m_Speed;
    public float m_TurningSpeed;
    public float m_Thurst;

    public GameObject m_Puck;
    public GameObject m_CollidingPuck;
    public GameObject m_PuckSpot;
    bool m_HasPuck;

    void Awake()
    {
        m_WaypointsTotal = m_Waypoints.Length;
    }

    void FixedUpdate()
    {

        transform.LookAt(m_Puck.transform);

        if (m_HasPuck)
        {
            // Do passing or shooting
        }
        else
        {
            // Decide whether to move
            if (Vector3.Distance(m_Waypoints[m_NextWaypoint].transform.position, m_Puck.transform.position) < 10F)
            {
                // Puck is close to forward
                //Debug.Log("MoveTrue!");
                MoveTowardsPuck(true);
            }else
            {
                //Debug.Log("Don't Move!");
            }

            if (Vector3.Distance(m_Waypoints[m_LastWaypoint].transform.position, m_Puck.transform.position) < 10F)
            {
                // Puck is close to back
                //Debug.Log("MoveFalse!");
                MoveTowardsPuck(false);
            }
        }
    }

    void MoveTowardsPuck(bool GoForward)
    {

        if (transform.position == m_Waypoints[m_NextWaypoint].position) //Switch waypoints, allowing us to move on curves
        {
            if (m_NextWaypoint < m_WaypointsTotal - 1)
            {
                m_LastWaypoint++;
                m_NextWaypoint++;
            }
        }
        else if (transform.position == m_Waypoints[m_LastWaypoint].position)
        {
            if (m_LastWaypoint > 0)
            {
                m_LastWaypoint--;
                m_NextWaypoint--;
            }
        }


        if (GoForward)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_Waypoints[m_NextWaypoint].transform.position, Time.deltaTime * m_Speed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_Waypoints[m_LastWaypoint].transform.position, Time.deltaTime * m_Speed);
        }
    }

}
