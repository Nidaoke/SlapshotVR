using UnityEngine;
using System.Collections;

public class OrbCollider : MonoBehaviour {

    public GameObject playerToFollow;
    public Vector3 offset;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Puck")
        {
            if (!other.gameObject.GetComponent<PuckControls>().m_Owned)
            {
                playerToFollow.GetComponent<PlayerControls>().m_ActivePlayer = true;
                CameraManagement.inst.FollowPlayer(playerToFollow);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = playerToFollow.transform.position;
	}
}
