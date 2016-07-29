using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour
{

    // Movement variables
    public Transform[] m_Waypoints;
    bool m_MovingForward;
    float m_WaypointsTotal;
    public enum DirectionToMove { Forward, Back, Stop };
    public DirectionToMove m_directionToMove;
    public int m_NextWaypoint;
    public int m_LastWaypoint;

    // Adjustable speeds
    public float m_Speed;
    public float m_TurningSpeed;
    public float m_Thurst;

    // Game variables
    public bool m_ActivePlayer;
    public bool m_Goalie;
    GameObject m_Puck;
    public GameObject m_CollidingPuck;
    public GameObject m_PuckSpot;
    public GameObject m_AimingArc;
    Vector3 m_ForwardSpot;
    bool m_Passing;
    float m_TriggerValue;
    bool m_Cooldown;

    void Awake()
    {
        m_WaypointsTotal = m_Waypoints.Length;
    }

    void FixedUpdate()
    {
        if (m_ActivePlayer)
        {
            Movement();
            Rotation();
            Shooting();
            GrabPuck();
        }

        if (m_Goalie)
        {
            GoalieMovement();
            Rotation();
        }
    }

    void Movement()
    {
        GetInputForMovement();


        switch (m_directionToMove)
        {
            case DirectionToMove.Forward:
                MoveToPoint(m_Waypoints[m_NextWaypoint]);
                break;
            case DirectionToMove.Back:
                MoveToPoint(m_Waypoints[m_LastWaypoint]);
                break;
            case DirectionToMove.Stop:
                //Don't Do Anything
                break;
        }

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
    }

    void GetInputForMovement()
    {
        if (m_ActivePlayer)
        {
            if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y > .2f)
                m_directionToMove = DirectionToMove.Forward;
            else if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y < -.2f)
                m_directionToMove = DirectionToMove.Back;
            else
                m_directionToMove = DirectionToMove.Stop;
        }

        if (m_Goalie)
        {
            if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x > .2f)
                m_directionToMove = DirectionToMove.Forward;
            else if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x < -.2f)
                m_directionToMove = DirectionToMove.Back;
            else
                m_directionToMove = DirectionToMove.Stop;
        }
    }

    void MoveToPoint(Transform point) //Move toward Waypoint
    {
        float step = 0f;
        if (m_ActivePlayer)
        {
            step = m_Speed * Time.deltaTime * Mathf.Abs(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y);
        }
        if (m_Goalie)
        {
            step = m_Speed * Time.deltaTime * Mathf.Abs(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x);
        }
        transform.position = Vector3.MoveTowards(transform.position, point.position, step);
    }

    void Rotation()
    {
        if (m_ActivePlayer)
        {
            if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x > .01f) //Rotate Around
            {
                transform.Rotate(Vector3.up * Time.deltaTime * m_TurningSpeed * OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x);
            }
            else if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x < -.01f)
            {
                transform.Rotate(Vector3.down * Time.deltaTime * m_TurningSpeed * Mathf.Abs(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x));
            }
        }

        if (m_Goalie)
        {
            if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y > .01f) //Rotate Around
            {
                transform.Rotate(Vector3.up * Time.deltaTime * m_TurningSpeed * OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y);
            }
            else if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y < -.01f)
            {
                transform.Rotate(Vector3.down * Time.deltaTime * m_TurningSpeed * Mathf.Abs(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y));
            }
        }
    }

    void Shooting()
    {
        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > .01F && m_Puck != null && !m_Passing)
        {
            // We're holding Left Trigger, move the aim!
            m_Passing = true;
            m_TriggerValue = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
            m_AimingArc.GetComponent<AimingControls>().Passing();
        }

        if (m_Passing && OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) < m_TriggerValue)
        {
            // We've let go and tried passing!
            StartCoroutine(CoolDownShot());
            m_AimingArc.GetComponent<AimingControls>().Passing();
            m_Puck.transform.parent = null;
            CameraManagement.inst.FollowPuck(m_Puck);
            m_Puck.GetComponent<PuckControls>().Hitting();
            Vector3 dirForShooting = ((m_AimingArc.GetComponent<AimingControls>().m_Direction.position - m_AimingArc.GetComponent<AimingControls> ().m_AimStart.transform.position) * m_Thurst);
            m_Puck.GetComponent<Rigidbody>().AddForce(dirForShooting.x, 500, dirForShooting.z);
            m_AimingArc.SetActive(false);
            m_Puck = null;
            m_TriggerValue = 0;
            m_Passing = false;
            m_ActivePlayer = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!m_Cooldown && other.GetComponent<PuckControls>())
        {
            if (!m_ActivePlayer)
            {
                CameraManagement.inst.FollowPlayer(this.gameObject);
                m_ActivePlayer = true;
            }
            m_CollidingPuck = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PuckControls>())
            if (m_Puck != null)
            {
                m_Puck.transform.parent = null;
                m_Puck = null;
            }
        m_CollidingPuck = null;

    }

    IEnumerator CoolDownShot()
    {
        m_Cooldown = true;
        yield return new WaitForSeconds(1);
        m_Cooldown = false;
    }
    void GrabPuck()
    {
        if (OVRInput.Get(OVRInput.RawButton.A))
        {
            // We tried to grab the Puck
            if (m_Puck != m_CollidingPuck && m_CollidingPuck != null)
            {
                if (m_ActivePlayer && !m_Cooldown)
                {
                    // The puck was there, lets stick it to us.
                    m_Puck = m_CollidingPuck;
                    m_Puck.transform.parent = this.transform;
                    m_Puck.transform.position = m_PuckSpot.transform.position;
                    m_Puck.GetComponent<PuckControls>().m_Owned = true;
                    m_AimingArc.SetActive(true);
                }
            }
        }

        if (OVRInput.GetUp(OVRInput.RawButton.A))
        {
            if (m_ActivePlayer && !m_Cooldown)
            {
                // We let go of the Puck.
                if (m_Puck != null)
                {
                    m_Puck.transform.parent = null;
                    m_Puck = null;
                }
                if (m_Passing)
                    m_Passing = false;
                m_AimingArc.SetActive(false);
            }
        }
    }





    void GoalieMovement()
    {
        GetInputForMovement();


        switch (m_directionToMove)
        {
            case DirectionToMove.Forward:
                MoveToPoint(m_Waypoints[m_NextWaypoint]);
                break;
            case DirectionToMove.Back:
                MoveToPoint(m_Waypoints[m_LastWaypoint]);
                break;
            case DirectionToMove.Stop:
                //Don't Do Anything
                break;
        }

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
    }
}
