using UnityEngine;
using System.Collections;

public class AimingControls : MonoBehaviour
{

    public float m_AimingSpeed;
    public GameObject m_AimStart;
    public GameObject m_AimingEnd;
    public GameObject m_AimLeft;
    public GameObject m_AimRight;
    public Transform m_Direction;
    public Transform m_AimTarget;
    bool m_Passing;

    public void Passing()
    {
        if (!m_Passing)
        {
            m_Passing = true;
            m_Direction = m_AimingEnd.transform;
            m_AimStart.SetActive(true);
            m_AimStart.GetComponent<LineRenderer>().SetPosition(0, m_AimStart.transform.position);
            m_AimStart.GetComponent<LineRenderer>().SetPosition(1, m_AimingEnd.transform.position);
            if (m_AimTarget == null)
                m_AimTarget = m_AimRight.transform;
            m_AimingEnd.SetActive(true);

        }
        else
        {
            Debug.Log("Activate Pass");
            m_Direction = m_AimingEnd.transform;
            m_Passing = false;
            m_AimStart.SetActive(false);
            m_AimingEnd.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (m_Passing)
        {

            if (m_AimingEnd.transform.localPosition.x > 4.4F)
            {
                m_AimTarget = m_AimRight.transform;
                Debug.Log("Go Right");
            }

            if (m_AimingEnd.transform.localPosition.x < -4.4F)
            {
                m_AimTarget = m_AimLeft.transform;
                Debug.Log("Go Left");
            }

            m_AimingEnd.transform.position = Vector3.Lerp(m_AimingEnd.transform.position, m_AimTarget.position, Time.deltaTime * m_AimingSpeed);
            m_AimStart.GetComponent<LineRenderer>().SetPosition(1, m_AimingEnd.transform.position);
            m_AimStart.GetComponent<LineRenderer>().SetPosition(0, m_AimStart.transform.position);
        }
    }
}
