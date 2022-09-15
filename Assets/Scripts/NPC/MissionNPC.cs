using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionNPC : MonoBehaviour
{
	public DataNPC data;

	public GameObject missionBox;
	public Text nameText;
	public Text messageText;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		DisableTextBox();
	}

	public void GiveMission(int playerProgress)
	{
		if (data.missionNumber == playerProgress + 1)
		{
			missionBox.SetActive(true);
			nameText.text = data.name;
			messageText.text = data.missionMessage;
		}
		else
		{
			missionBox.SetActive(true);
			nameText.text = data.name;
			messageText.text = data.notReadyMessage;
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
