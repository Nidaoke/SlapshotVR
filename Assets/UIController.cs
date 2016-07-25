using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

    public Text textHolder;
    public bool isPlayerTouching;

	void Update () {
        textHolder.text = "Player is touching the puck: " + isPlayerTouching;
	}
}
