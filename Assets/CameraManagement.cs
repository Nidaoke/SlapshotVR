/* Created by Dean Day 26/07/2016
 * Greenlight Games Ltd
 * Working with Forever Humble PDX
*/
using UnityEngine;
using System.Collections;

public class CameraManagement : Singleton.Manager<CameraManagement>
{

    public GameObject m_Camera;
    public float m_Speed;

    bool m_Lerp;
    public GameObject m_Follow;

    public void FollowPuck(GameObject Puck)
    {
        if (!m_Lerp)
            m_Follow.GetComponent<PlayerControls>().m_ActivePlayer = false;

        m_Follow = Puck;
        m_Lerp = true;
    }

    public void FollowPlayer(GameObject Player)
    {
        if (!m_Lerp)
            m_Follow.GetComponent<PlayerControls>().m_ActivePlayer = false;

        m_Follow = Player;
        m_Lerp = false;
    }

    void FixedUpdate()
    {
        if (m_Lerp)
        {
            Vector3 GoTo = new Vector3(m_Follow.transform.position.x, m_Camera.transform.position.y, m_Follow.transform.position.z + 4);
            m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, GoTo, Time.deltaTime * m_Speed);
        }
        else
        {
            Vector3 GoTo = new Vector3(m_Follow.transform.position.x, m_Camera.transform.position.y, m_Follow.transform.position.z + 4);
            m_Camera.transform.position = GoTo;
        }
    }
}
