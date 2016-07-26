using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuckControls : MonoBehaviour
{

    public Vector3 m_LastTrans;
    public bool m_Owned;
    public List<GameObject> m_Players = PlayerManagement.inst.m_RedTeam;
    public float m_Distance;
    public int m_Player;

    public void Hitting()
    {
        m_Owned = false;
        m_Distance = Mathf.Infinity;
        StartCoroutine(Reposition());
    }

    IEnumerator Reposition()
    {
        yield return new WaitForSeconds(3);

        // We've stopped
        if (!m_Owned)
        {
            // Find the closest player
            for (int i = 0; i < m_Players.Count; i++)
            {
                if (Vector3.Distance(transform.position, m_Players[i].transform.position) < m_Distance)
                {
                    m_Distance = Vector3.Distance(transform.position, m_Players[i].transform.position);
                    m_Player = i;
                }
            }
        }
        //m_Players[m_Player].GetComponent<PlayerControls>().m_ActivePlayer = true;
        //CameraManagement.inst.FollowPlayer(m_Players[m_Player]);
    }
}
