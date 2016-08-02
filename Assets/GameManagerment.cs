using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManagerment : Singleton.Manager<GameManagerment> {

    public bool goalScored;
    [SerializeField] private GameObject cameraText;

    public void GoalScored()
    {
        if (!goalScored)
        {
            goalScored = true;
            cameraText.GetComponent<Text>().text = "Goal Scored!";
        }
    }
}