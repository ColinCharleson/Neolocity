using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour
{
	public GameObject player;
	void Update()
	{
		if (MissionManager.instance.lastMission == 6)
		{
			if(Vector3.Distance(player.transform.position, this.transform.position) <1)
			{
				SceneManager.LoadScene("BossBattle");
			}
		}
	}
}
