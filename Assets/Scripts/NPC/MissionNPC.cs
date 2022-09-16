using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionNPC : MonoBehaviour
{
	//Character Data
	public GameObject player;
	public DataNPC data;

	//Mission Types
	public GoToMissionData missionGoTo;
	public GameObject missionEnd;

	public PickUpMission missionPickUp;
	public GameObject pickUpEnd;

	public KillMissionData missionKill;

	//Mission Status
	public bool missionStarted;
	public bool missionFinished;
	public bool missionHandedIn;
	
	//Text Box
	public GameObject missionBox;
	public Text nameText;
	public Text messageText;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update()
	{
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
				player.GetComponent<PlayerController>().lastMission += 1;
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
		if (missionGoTo != null && missionStarted == false)
		{
			missionStarted = true;
			missionEnd = Instantiate(missionGoTo.missionEnd, missionGoTo.missionEndLocation, Quaternion.Euler(Vector3.zero));
		}

		if (missionPickUp != null && missionStarted == false)
		{
			missionStarted = true;
			missionEnd = Instantiate(missionPickUp.missionPickUp, missionPickUp.pickUpLocation, Quaternion.Euler(Vector3.zero));
			Instantiate(missionPickUp.pickUpItem, missionEnd.transform);
		}
	}
	public void EndMission()
	{
		if (missionGoTo != null && missionStarted == true) //Go to mission
		{
			if (Vector3.Distance(player.transform.position, missionGoTo.missionEndLocation) < 5)
			{
				missionFinished = true;
				Destroy(GameObject.FindGameObjectWithTag("MissionEnd"));
			}
		}

		if (missionPickUp != null && missionStarted == true) //Pick Up mission
		{
			if (Vector3.Distance(player.transform.position, missionPickUp.pickUpLocation) < 2)
			{
				missionFinished = true;
				Destroy(GameObject.FindGameObjectWithTag("MissionEnd"));
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
