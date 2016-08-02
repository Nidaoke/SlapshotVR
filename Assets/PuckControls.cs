using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuckControls : MonoBehaviour
{

    public Vector3 m_LastTrans;
    public bool m_Owned, lookingForNewPlayer;
    public List<GameObject> m_Players = PlayerManagement.inst.m_RedTeam;
    public List<GameObject> m_OrbPlayers;
    public float m_Distance;
    public int m_Player;

    void Start()
    {
        m_Players = PlayerManagement.inst.m_RedTeam;
    }

    void Update()
    {
        if (lookingForNewPlayer)
        {
            if (m_Owned)
                lookingForNewPlayer = false;
            if (m_OrbPlayers.Count == 1)
            {
                CameraManagement.inst.FollowPlayer(m_OrbPlayers[0]);
            }
        }
    }

    public void Hitting()
    {
        m_Owned = false;
        m_Distance = Mathf.Infinity;
        StopCoroutine(Reposition()); 
        StartCoroutine(Reposition());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GoalScanner" && !m_Owned)
        {
            if (!GameManagerment.inst.goalScored)
                GameManagerment.inst.GoalScored();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PlayerOrb")
        {
            Debug.Log(other.gameObject.name);
            if(!m_OrbPlayers.Contains(other.gameObject.transform.parent.gameObject))
                m_OrbPlayers.Add(other.gameObject.transform.parent.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            m_OrbPlayers.Remove(other.gameObject);
    }

    IEnumerator Reposition()
    {
        yield return new WaitForSeconds(.5f);
        lookingForNewPlayer = true;
        yield return new WaitForSeconds(2.5f);

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

            if (!GameManagerment.inst.goalScored) {
                m_Players[m_Player].GetComponent<PlayerControls>().m_ActivePlayer = true;
                CameraManagement.inst.FollowPlayer(m_Players[m_Player]);
            }
        }
    }
}
