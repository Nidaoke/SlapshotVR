using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManagement : Singleton.Manager<PlayerManagement>
{
    public List<GameObject> m_RedTeam = new List<GameObject>();
    public List<GameObject> m_BlueTeam = new List<GameObject>();
}
