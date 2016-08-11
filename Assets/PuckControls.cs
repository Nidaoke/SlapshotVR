using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuckControls : MonoBehaviour
{

    public Vector3 m_LastTrans;
    public bool m_Owned;
    public List<GameObject> m_Players = PlayerManagement.inst.m_RedTeam;
    public Transform[] m_EnemyPlayers;
    public float m_Distance;
    public int m_Player;

    public Transform nearestPlayer1, nearestPlayer2;

    void Start()
    {
        m_Players = PlayerManagement.inst.m_RedTeam;
    }

    void Update()
    {
        nearestPlayer1 = GetClosestEnemy(m_EnemyPlayers);
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

    Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
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

            if (!GameManagerment.inst.goalScored) {
                m_Players[m_Player].GetComponent<PlayerControls>().m_ActivePlayer = true;
                CameraManagement.inst.FollowPlayer(m_Players[m_Player]);
            }
        }
    }
}
