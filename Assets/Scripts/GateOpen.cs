using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (MissionManager.instance.lastMission >= 3)
            Destroy(this.gameObject);
    }
}
