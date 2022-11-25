using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionNPC : MonoBehaviour
{
	public Text missionProgressDisplay;
	public Image missionProgressBack;
	//Character Data
	[Header("Character Data")]
	public GameObject player;
	public DataNPC data;

	[Header ("Mission Types")]
	//Mission Types
	public GoToMissionData missionGoTo;
	private GameObject missionEnd;

	public PickUpMission missionPickUp;
	private GameObject missionItem;

	public FollowMission missionFollow;
	private GameObject missionTarget;

	public HackMission missionHack;
	private float hackTime;

	//public KillMissionData missionKill;

	[Header("Mission Status")]
	//Mission Status
	public bool missionStarted;
	public bool missionFinished;
	public bool missionHandedIn;
	bool tempFinish = false;

	[Header("Text Box Data")]
	//Text Box
	public GameObject missionBox;
	public Text nameText;
	public Text messageText;

	// Start is called before the first frame update
	void Start()
	{
		missionProgressDisplay.enabled = false;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (missionProgressDisplay.enabled)
			missionProgressBack.enabled = true;
		else
			missionProgressBack.enabled = false;

		DisableTextBox();
		EndMission();
	}

	public void TalkToNPC(int lastMission)
	{
		if (missionStarted == true) // if the player is on the given mission
		{
			if(missionFinished == true) // if they finished it
			{
				missionBox.SetActive(true);
				nameText.text = data.name;
				messageText.text = data.finishedMessage;
				missionProgressDisplay.enabled = false;

				MissionManager.instance.lastMission += 1;
				MissionManager.instance.missionActive = false;
				MissionManager.instance.currentMission = 0;
				MissionManager.instance.currentObjective = null;

				missionHandedIn = true;
				missionStarted = false;
			}
			else // if they still need to finish it
			{
				missionBox.SetActive(true);
				nameText.text = data.name;
				messageText.text = data.keepGoingMessage;
			}
		}
		else
		{
			if (data.missionNumber == lastMission + 1) // if the player is ready for next mission
			{
				missionBox.SetActive(true);
				nameText.text = data.name;
				messageText.text = data.missionMessage;

				StartMission();
			}
			else if (data.missionNumber > lastMission + 1) //Checks if mission to give is too far ahead
			{
				missionBox.SetActive(true);
				nameText.text = data.name;
				messageText.text = data.notReadyMessage;
			}
			else if (data.missionNumber < lastMission + 1) //Checks if mission to give is too far behind
			{
				missionBox.SetActive(true);
				nameText.text = data.name;
				messageText.text = data.allDoneMessage;
			}

		}
	}

	public void StartMission()
	{
		tempFinish = false;
		MissionManager.instance.missionActive = true;
		MissionManager.instance.currentMission = MissionManager.instance.lastMission + 1;

		if (missionGoTo != null && missionStarted == false)
		{
			missionStarted = true;
			missionProgressDisplay.enabled = true;
			missionEnd = Instantiate(missionGoTo.missionEnd, missionGoTo.missionEndLocation, Quaternion.Euler(Vector3.zero));
			MissionManager.instance.currentObjective = missionEnd.transform;
		}

		if (missionPickUp != null && missionStarted == false)
		{
			missionStarted = true;
			missionProgressDisplay.enabled = true;
			missionItem = Instantiate(missionPickUp.missionPickUp, missionPickUp.pickUpLocation, Quaternion.Euler(Vector3.zero));
			Instantiate(missionPickUp.pickUpItem, missionItem.transform);
			MissionManager.instance.currentObjective = missionItem.transform;
		}

		if (missionFollow != null && missionStarted == false)
		{
			missionStarted = true;
			missionProgressDisplay.enabled = true;
			missionTarget = Instantiate(missionFollow.followTarget, missionFollow.startLocation, Quaternion.Euler(Vector3.zero));
			missionTarget.GetComponent<FollowBot>().target = missionFollow.endLocation;
			MissionManager.instance.currentObjective = missionTarget.transform;
		}
		
		if (missionHack != null && missionStarted == false)
		{
			missionStarted = true;
			missionProgressDisplay.enabled = true;
			missionTarget = Instantiate(missionHack.hackTarget, missionHack.startLocation, Quaternion.Euler(Vector3.zero));
			missionTarget.GetComponent<FollowBot>().target = missionHack.endLocation;
			MissionManager.instance.currentObjective = missionTarget.transform;
		}
	}
	public void EndMission()
	{
		if (missionGoTo != null && missionStarted == true) //Go to mission
		{
			if (Vector3.Distance(player.transform.position, missionGoTo.missionEndLocation) < 5)
			{
				tempFinish = true;
			}

			if (tempFinish == true)
			{
				missionFinished = true;
				MissionManager.instance.currentObjective = this.transform;
				Destroy(GameObject.FindGameObjectWithTag("MissionEnd"));
				missionProgressDisplay.color = Color.green;
				missionProgressDisplay.text = "LOCATION DISCOVERED: Return to " + data.name + " with the information about the location";
			}
			else
			{
				missionProgressDisplay.color = Color.cyan;
				missionProgressDisplay.text = "GO TO THE LOCATION: " +
				Vector3.Distance(player.transform.position, GameObject.FindGameObjectWithTag("MissionEnd").transform.position).ToString("00") + " m";
			}
		}

		if (missionPickUp != null && missionStarted == true) //Pick Up mission
		{
			if (Vector3.Distance(player.transform.position, missionPickUp.pickUpLocation) < 2)
			{
				tempFinish = true;
			}

			if(tempFinish == true)
			{
				missionFinished = true;
				MissionManager.instance.currentObjective = this.transform;
				Destroy(GameObject.FindGameObjectWithTag("MissionEnd"));

				missionProgressDisplay.color = Color.green;
				missionProgressDisplay.text = "PACKAGE RECEIVED: Return to " + data.name + " with the package";
			}
			else
			{
				missionProgressDisplay.color = Color.cyan;
				missionProgressDisplay.text = "PICK UP THE PACKAGE: " + 
				Vector3.Distance(player.transform.position, GameObject.FindGameObjectWithTag("MissionEnd").transform.position).ToString("00") + " m";
			}
		}

		if (missionFollow != null && missionStarted == true) //Follow mission
		{

			if (Vector3.Distance(player.transform.position, missionFollow.endLocation) < 10)
			{
				missionFinished = true;
				MissionManager.instance.currentObjective = this.transform;

				missionProgressDisplay.color = Color.green;
				missionProgressDisplay.text = "LOCATION DISCOVERED: Return to " + data.name + " with the location";
			}

			if (missionTarget.gameObject == null)
			{
				if(missionFinished == false)
				{
					missionStarted = false;
				}
			}
			else
			{
				if (Vector3.Distance(player.transform.position, missionTarget.transform.position) > 20)
				{
					missionProgressDisplay.color = Color.red;
					missionProgressDisplay.text = "DISCOVER ENEMIES DESTINATION: Youre losing them";
				}
				else if (Vector3.Distance(player.transform.position, missionTarget.transform.position) < 5)
				{
					missionProgressDisplay.color = Color.red;
					missionProgressDisplay.text = "DISCOVER ENEMIES DESTINATION: Presence detected, back up";
				}
				else
				{
					missionProgressDisplay.color = Color.cyan;
					missionProgressDisplay.text = "DISCOVER ENEMIES DESTINATION: Follow them";
				}
			}
		}

		if (missionHack != null && missionStarted == true) //Hack mission
		{
			if (missionTarget.gameObject == null)
			{
				if (hackTime > missionHack.hackingTime)
				{
					missionFinished = true;
					missionProgressDisplay.color = Color.green;
					missionProgressDisplay.text = "HACK COMPLETED: Return to " + data.name + " with the data";
					MissionManager.instance.currentObjective = this.transform;
				}
				else
				{
					missionStarted = false;
				}
			}
			else
			{
				if (100f > ((hackTime / missionHack.hackingTime) * 100))
				{
					if (Vector3.Distance(player.transform.position, missionTarget.transform.position) > 20)
					{
						missionProgressDisplay.color = Color.red;
						missionProgressDisplay.text = "SIGNAL WEAK: Get closer";
					}
					else if (Vector3.Distance(player.transform.position, missionTarget.transform.position) < 5)
					{
						missionProgressDisplay.color = Color.red;
						missionProgressDisplay.text = "PRESENCE DETECTED: Too close";
					}
					else
					{
						hackTime += 1 * Time.deltaTime;

						missionProgressDisplay.color = Color.cyan;
						missionProgressDisplay.text = "HACK PROGRESS: " + ((hackTime / missionHack.hackingTime) * 100).ToString("00") + "%";
					}
				}
				else
				{
					missionProgressDisplay.color = Color.green;
					missionProgressDisplay.text = "HACK COMPLETED: Return To " + data.name + " With The Data";
				}
			}
		}
	}
	public void DisableTextBox()
	{
		if (Input.GetKey(KeyCode.Q))
		{
			missionBox.SetActive(false);
			nameText.text = null;
			messageText.text = null;
		}

	}
}
