using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
	public static MissionManager instance;

	public int lastMission = 0;

    public bool missionActive;
    public int currentMission;

    public Transform currentObjective;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
	private void FixedUpdate()
	{
        var objects = GameObject.FindGameObjectsWithTag("NPC");
        var objectCount = objects.Length;

        if (lastMission != 6)
        {
            if (missionActive == false)
            {
                foreach (var obj in objects)
                {
                    if (obj.GetComponent<MissionNPC>().data.missionNumber == lastMission + 1)
                    {
                        currentObjective = obj.transform;
                    }
                }

            }
        }
		else
		{
            currentObjective = GameObject.Find("End Door").transform;
		}
    }
}
