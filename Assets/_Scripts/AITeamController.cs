using UnityEngine;
using System.Collections;

public class AITeamController : MonoBehaviour 
{
	[SerializeField] private PlayerMovementScript[] mAIPlayers;

	[SerializeField] private PlayerMovementScript mClosestTeamMate;

	[SerializeField] private GameObject mPuck;
	[SerializeField] private Transform mGoal;
	bool mJustShotPuck = false;
	float mPuckShotCooldown = 1f;

	// Use this for initialization
	void Start () 
	{
		mClosestTeamMate = mAIPlayers[0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckForPuck();

		//Only move the AI if the puck is in play ~Adam
		if (mPuck != null)
		{
			DetermineClosestTeamMate();

			//Chase the puck if none of the AI team mates have it~Adam
			if(!mClosestTeamMate.hasPuck && !mJustShotPuck)
			{
				//Chase the puck ~Adam
				if(mPuck.transform.position.z <= mClosestTeamMate.transform.position.z)
				{
					if(mClosestTeamMate.turnedAround)
					{
						mClosestTeamMate.directionToMove = PlayerMovementScript.DirectionToMove.back;
					}
					else
					{
						mClosestTeamMate.directionToMove = PlayerMovementScript.DirectionToMove.forward;
					}
				}
				else
				{
					if(!mClosestTeamMate.turnedAround)
					{
						mClosestTeamMate.directionToMove = PlayerMovementScript.DirectionToMove.back;
					}
					else
					{
						mClosestTeamMate.directionToMove = PlayerMovementScript.DirectionToMove.forward;
					}
				}
				//Spin to grab the puck ~Adam
				if(Vector3.Distance(mClosestTeamMate.transform.position,mPuck.transform.position)<5f && !mJustShotPuck)
				{
					mClosestTeamMate.transform.Rotate(Vector3.up * Time.deltaTime * mClosestTeamMate.turningSpeed);
				}
			}
			//Aim at the opponent's goal and fire ~Adam
			else if (!mJustShotPuck)
			{

				Debug.DrawRay(mClosestTeamMate.puckSpot.position, mClosestTeamMate.puckSpot.transform.forward*100f, Color.blue);


				// Bit shift the index of the layer (11) to get a bit mask
				int layerMask = 1 << 11;

				// This would cast rays only against colliders in layer 11.


				RaycastHit hit;
				// Does the ray intersect any objects excluding the player layer
				if (Physics.Raycast(mClosestTeamMate.puckSpot.position, mClosestTeamMate.puckSpot.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, layerMask))
				{
					Debug.DrawRay(mClosestTeamMate.puckSpot.position, mClosestTeamMate.puckSpot.TransformDirection (Vector3.forward) * hit.distance, Color.yellow);
					Debug.Log("Goal In Sight!");
				           
					mClosestTeamMate.PublichShoot();
					StartCoroutine(PuckShotCooldown());
				}
				else
				{
					mClosestTeamMate.transform.Rotate(Vector3.up * Time.deltaTime * mClosestTeamMate.turningSpeed*0.6f);
				}
			}
		}
	}


	void CheckForPuck()
	{
		mPuck = GameObject.FindGameObjectWithTag("Puck");
	}

	void DetermineClosestTeamMate()
	{
		foreach (PlayerMovementScript teamMate in mAIPlayers)
		{
//			if(Vector3.Distance(teamMate.transform.position, mPuck.transform.position) < Vector3.Distance(mClosestTeamMate.transform.position,mPuck.transform.position))
//			{
//			}
			if(Mathf.Abs(teamMate.transform.position.x - mPuck.transform.position.x) < (Mathf.Abs(mClosestTeamMate.transform.position.x - mPuck.transform.position.x)))
			{
				if(Vector3.Distance(teamMate.transform.position, mPuck.transform.position) < 15f)
				{
					mClosestTeamMate = teamMate;
				}
			}
		}
	}


	IEnumerator PuckShotCooldown()
	{
		mJustShotPuck = true;
		yield return new WaitForSeconds(mPuckShotCooldown);
		mJustShotPuck = false;
	}
}
