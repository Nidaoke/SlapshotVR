using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject puck;
    public Transform puckSpawnSpot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.Return))
            Instantiate(puck, puckSpawnSpot.position, Quaternion.identity);
	}
}
