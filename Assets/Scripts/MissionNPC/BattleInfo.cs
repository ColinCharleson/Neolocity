using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfo : MonoBehaviour
{
    public int enemiesLeft;
    // Update is called once per frame
    void Update()
    {
        enemiesLeft = transform.childCount;
    }
}
