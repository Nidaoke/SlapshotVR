using UnityEngine;
using System.Collections;

public class PuckStayOnTable : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(transform.position.x, 29.855f, transform.position.z);

//		if(transform.position.y < 29.8f)
//		{
//			transform.position = new Vector3(transform.position.x, 29.855f, transform.position.z);
//		}
	}
}
