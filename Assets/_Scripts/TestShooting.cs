using UnityEngine;
using System.Collections;

public class TestShooting : MonoBehaviour {

    public GameObject objectToShoot;
    public float thrust;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            objectToShoot.GetComponent<Rigidbody>().AddForce(transform.forward * thrust);
            objectToShoot.GetComponent<Rigidbody>().AddForce(transform.up * (thrust / 2.5f));
        }
	}
}
