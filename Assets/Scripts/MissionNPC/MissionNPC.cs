using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionNPC : MonoBehaviour
{
	public TextMeshProUGUI missionProgressDisplay;
	public Image missionProgressBack;
	//Character Data
	[Header("Character Data")]
	public GameObject player;
	public DataNPC data;

	[Header("Mission Types")]
	//Mission Types
	public GoToMissionData missionGoTo;
	private GameObject missionEnd;

	public KillMissionData missionKill;

	public PickUpMission missionPickUp;
	public GameObject missionItem;

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
	float timer, failTime;

	[Header("Text Box Data")]
	//Text Box
	public GameObject missionBox;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI messageText;

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
			if (missionFinished == true) // if they finished it
			{
				missionBox.SetActive(true);
				nameText.text = data.name;
				messageText.text = data.finishedMessage;
				Destroy(GameObject.FindGameObjectWithTag("MissionEnd"));
				missionProgressDisplay.enabled = false;

				MissionManager.instance.lastMission += 1;
				MissionManager.instance.missionActive = false;
				MissionManager.instance.currentMission = 0;
				MissionManager.instance.currentObjective = null;

				MissionManager.instance.gameObject.GetComponent<SavePlugin>().SavePosition();

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
		timer = 0;
		hackTime = 0;
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

		if (missionKill != null && missionStarted == false)
		{
			missionStarted = true;
			missionProgressDisplay.enabled = true;
			missionTarget = Instantiate(missionKill.battlePrefab, missionKill.spawnArea, Quaternion.Euler(Vector3.zero));
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
			if (missionItem == null)
			{
				tempFinish = true;
			}

			if (tempFinish == true)
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
			failTime = 3;
			if (timer < failTime)
			{
				if (Vector3.Distance(player.transform.position, missionFollow.endLocation) < 20)
				{
					missionFinished = true;
					MissionManager.instance.currentObjective = this.transform;

					missionProgressDisplay.color = Color.green;
					missionProgressDisplay.text = "LOCATION DISCOVERED: Return to " + data.name + " with the location";
				}

				if (missionTarget.gameObject == null)
				{
					if (missionFinished == false)
					{
						missionStarted = false;
						missionProgressDisplay.color = Color.red;
						missionProgressDisplay.text = "YOU FAILED: Return to " + data.name + " to try again";
						MissionManager.instance.currentObjective = this.transform;
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
						timer += Time.deltaTime;
					}
					else
					{
						missionProgressDisplay.color = Color.cyan;
						missionProgressDisplay.text = "DISCOVER ENEMIES DESTINATION: Follow them";
						timer = 0;
					}
				}
			}
			else
			{
				missionStarted = false;
				missionProgressDisplay.color = Color.red;
				missionProgressDisplay.text = "THEY SPOTTED YOU: Return to " + data.name + " to try again";
				MissionManager.instance.currentObjective = this.transform;
			}
		}

		if (missionHack != null && missionStarted == true) //Hack mission
		{
			failTime = 3;
			if (timer < failTime)
			{
				if (hackTime > missionHack.hackingTime)
				{
					missionFinished = true;
					missionProgressDisplay.color = Color.green;
					missionProgressDisplay.text = "HACK COMPLETED: Return to " + data.name + " with the data";
					MissionManager.instance.currentObjective = this.transform;
				}
				if (missionTarget.gameObject == null)
				{
					if (hackTime <= missionHack.hackingTime)
					{
						missionStarted = false;
						missionProgressDisplay.text = "YOU FAILED: Return to " + data.name + " to try again";
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
							timer += Time.deltaTime;
						}
						else
						{
							hackTime += 1 * Time.deltaTime;

							missionProgressDisplay.color = Color.cyan;
							missionProgressDisplay.text = "HACK PROGRESS: " + ((hackTime / missionHack.hackingTime) * 100).ToString("00") + "%";
							timer = 0;
						}
					}
					else
					{
						missionProgressDisplay.color = Color.green;
						missionProgressDisplay.text = "HACK COMPLETED: Return To " + data.name + " With The Data";
					}
				}
			}
			else
			{
				missionStarted = false;
				missionProgressDisplay.color = Color.red;
				missionProgressDisplay.text = "THEY SPOTTED YOU: Return to " + data.name + " to try again";
				MissionManager.instance.currentObjective = this.transform;
			}
		}

		if (missionKill != null && missionStarted == true) //Kill mission
		{
			if(missionTarget.transform.childCount <= 0)
			{
				tempFinish = true;
			}

			if (tempFinish == true)
			{
				missionFinished = true;
				MissionManager.instance.currentObjective = this.transform;

				missionProgressDisplay.color = Color.green;
				missionProgressDisplay.text = "ENEMIES ELIMINATED: Return to " + data.name + "";
			}
			else
			{
				missionProgressDisplay.color = Color.cyan;
				missionProgressDisplay.text = "ELIMINATE RED ROBOTS: " + missionTarget.transform.childCount + " left";
			}
		}
	}
	public void DisableTextBox()
	{
		if (Input.GetKey(InputSystem.key.cancelTalk))
		{
			missionBox.SetActive(false);
			nameText.text = null;
			messageText.text = null;
		}

	}
}
