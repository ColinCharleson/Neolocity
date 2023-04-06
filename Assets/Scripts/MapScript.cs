using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
	public Transform player;
	public GameObject npcIcon;
	public Vector3 iconPosition;
	public Transform objIconLocation = null;

	void LateUpdate()
	{
		if (MissionManager.instance.lastMission != 6)
		{
			objIconLocation = MissionManager.instance.currentObjective;

			iconPosition = objIconLocation.position + new Vector3(0, 40, 0);
			npcIcon.transform.rotation = Quaternion.Euler(0, 180, 0);
			npcIcon.transform.position = iconPosition;
		}
		else
		{
			objIconLocation = GameObject.Find("End Door").transform;
			iconPosition = objIconLocation.position + new Vector3(0, 40, 0);
		}
	}
}
