using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class LoadPlugin : MonoBehaviour
{
	[DllImport("LoadPlugin")]
	private static extern float LoadFromFile(int j, string fileName);

	[DllImport("LoadPlugin")]
	private static extern int GetLines(string fileName);

	string fn;
	public PlayerController player;
	// Start is called before the first frame update
	void Start()
	{
		fn = Application.dataPath + "/save.txt";

		//if (!Application.isEditor)
		{
			if (IsLoadingGame.instance.load)
			{
				LoadPosition();
			}
			Destroy(IsLoadingGame.instance.gameObject);
		}
	}

	public void LoadPosition()
	{
		player.transform.position = new Vector3(LoadFromFile(0, fn), LoadFromFile(1, fn), LoadFromFile(2, fn));

		PlayerHealth.hp.health = LoadFromFile(3, fn);
		MissionManager.instance.lastMission = (int)LoadFromFile(4, fn);
		player.speedSP = (float)LoadFromFile(5, fn);
		player.damageSP = (float)LoadFromFile(6, fn);
		player.jumpSP = (float)LoadFromFile(7, fn);
		player.boostSP = (float)LoadFromFile(8, fn);
		player.healthSP = (float)LoadFromFile(9, fn);
		player.scrap = (int)LoadFromFile(10, fn);
	}
}