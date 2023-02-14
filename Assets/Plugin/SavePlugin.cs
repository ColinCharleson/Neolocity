using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class SavePlugin : MonoBehaviour
{
    [DllImport("Plugin")]
    private static extern Vector3 GetPosition();

    [DllImport("Plugin")]
    private static extern void SetPosition(float x, float y, float z);

    [DllImport("Plugin")]
    private static extern void SaveToFile(float x, float y, float z, float health, float lastMission,
    float speedSP, float damageSP, float jumpSP, float boostSP, float healthSP, int scrap);

    [DllImport("Plugin")]
    private static extern void StartWriting(string fileName);

    [DllImport("Plugin")]
    private static extern void EndWriting();

    string m_Path;
    string fn;

    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        m_Path = Application.dataPath;
        fn = m_Path + "/save.txt";
    }

    public void SavePosition()
    {
        Debug.Log(fn);
        StartWriting(fn);
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        player = obj.GetComponent<PlayerController>();

        SaveToFile(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z, PlayerHealth.hp.health, MissionManager.instance.lastMission,
             player.speedSP, player.damageSP, player.jumpSP, player.boostSP, player.healthSP, player.scrap);

        EndWriting();
    } 
}
