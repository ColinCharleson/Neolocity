using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
	public static MissionManager instance;

	public int lastMission = 0;

    public bool missionActive;
    public int currentMission;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
}
