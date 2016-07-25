using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    //FollowPlayer

    public GameObject thingToFollow;
    public PlayerMovementScript[] players;
    public Vector3 positionOffset;
    public float moveSpeed, rotSpeed;

    public void PuckHasBeenShot(GameObject puck)
    {
        thingToFollow = puck;
        moveSpeed = 5;
        StartCoroutine(WaitAndReposition());
    }

    public IEnumerator WaitAndReposition()
    {
        yield return new WaitForSeconds(2);
        if(thingToFollow.tag == "Puck")
        {
            GameObject newPlayerToGoTo = null;
            float minDist = Mathf.Infinity;

            foreach(PlayerMovementScript player in players)
            {
                float dist = Vector3.Distance(player.gameObject.transform.position, transform.position);
                if(dist < minDist)
                {
                    newPlayerToGoTo = player.gameObject;
                    minDist = dist;
                    Debug.Log("Set newplayertogoto to " + newPlayerToGoTo);
                }
            }

            thingToFollow = newPlayerToGoTo;
            thingToFollow.GetComponent<PlayerMovementScript>().activePlayer = true;
        }

    }

    void Update()
    {
        if(transform.position != (thingToFollow.transform.position + positionOffset))
        {
            transform.position = Vector3.Lerp(transform.position, thingToFollow.transform.position + positionOffset, Time.deltaTime * moveSpeed);
        }

        if (players.Length == 0)
        {
            players = GameObject.FindObjectsOfType<PlayerMovementScript>();
        }
    }
    /*public PlayerMovementScript[] players;
    public GameObject assignedPlayer, mCamera, tracker;
    public bool inProcessOfMoving;
    public float speed, turnSpeed, controlTurnSpeed;
	
    void Start()
    {
        players = GameObject.FindObjectsOfType<PlayerMovementScript>();
    }

    void CameraLookAtTracker()
    {
        Vector3 targetDir = tracker.transform.position - mCamera.transform.position;
        float step = 1 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(mCamera.transform.forward, targetDir, step, 0.0f);
        //Debug.DrawRay(mCamera.transform.position, newDir, Color.red);
        mCamera.transform.rotation = Quaternion.LookRotation(newDir);
    }
	
	void Update () {
        if(assignedPlayer == null || !assignedPlayer.GetComponent<PlayerMovementScript>().activePlayer)
        {
            foreach (PlayerMovementScript player in players)
            {
                if (player.activePlayer)
                {
                    assignedPlayer = player.gameObject;
                    inProcessOfMoving = true;
                }
            }
        }

        tracker.transform.localPosition = new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"), tracker.transform.localPosition.z);
        CameraLookAtTracker();

        /*if (Mathf.Abs(Input.GetAxis("Horizontal2")) > .1f)
            mCamera.transform.Rotate(Vector2.up * Time.deltaTime * controlTurnSpeed * Input.GetAxis("Horizontal2"));
        if (Mathf.Abs(Input.GetAxis("Vertical2")) > .1f)
            mCamera.transform.Rotate(Vector2.left * Time.deltaTime * controlTurnSpeed * Input.GetAxis("Vertical2"));
            //
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach(PlayerMovementScript player in players)
            {
                if (player.gameObject.name == "RedPlayer1")
                    player.activePlayer = true;
                else
                    player.activePlayer = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (PlayerMovementScript player in players)
            {
                if (player.gameObject.name == "RedPlayer2")
                    player.activePlayer = true;
                else
                    player.activePlayer = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (PlayerMovementScript player in players)
            {
                if (player.gameObject.name == "RedPlayer3")
                    player.activePlayer = true;
                else
                    player.activePlayer = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            foreach (PlayerMovementScript player in players)
            {
                if (player.gameObject.name == "RedPlayer4")
                    player.activePlayer = true;
                else
                    player.activePlayer = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            foreach (PlayerMovementScript player in players)
            {
                if (player.gameObject.name == "RedPlayer5")
                    player.activePlayer = true;
                else
                    player.activePlayer = false;
            }
        }

        if (inProcessOfMoving)
        {
            MoveToPlayer(assignedPlayer.transform);
        }
	}

    void MoveToPlayer(Transform player)
    {
        transform.parent = player;
        float step = speed * Time.deltaTime;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, transform.localPosition.y, 0), step);

        float stepR = turnSpeed * Time.deltaTime;

        float angle = Mathf.LerpAngle(transform.localEulerAngles.y, 0, stepR);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);

        if ((transform.localPosition.x == 0 && transform.localPosition.z == 0) && (transform.localEulerAngles.y == 0 || transform.localEulerAngles.y == 360))
            inProcessOfMoving = false;
    }*/ //Old Not Working For What We Need
}