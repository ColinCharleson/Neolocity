using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public Transform player;
    public GameObject npcIcon;

    Transform objIconLocation = null;

    void LateUpdate()
    {
        if (objIconLocation != MissionManager.instance.currentObjective)
        {
            objIconLocation = MissionManager.instance.currentObjective;

            Vector3 iconPosition = objIconLocation.position + new Vector3(0, 40, 0);
            npcIcon.transform.position = iconPosition;
        }
    }
}
