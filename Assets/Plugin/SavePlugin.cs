using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class SavePlugin : MonoBehaviour
{
    [DllImport("SavePlugin")]
    private static extern Vector3 GetPosition();

    [DllImport("SavePlugin")]
    private static extern void SetPosition(float x, float y, float z);

    [DllImport("SavePlugin")]
    private static extern void SaveToFile(float x, float y, float z, float health, float lastMission);

    [DllImport("SavePlugin")]
    private static extern void StartWriting(string fileName);

    [DllImport("SavePlugin")]
    private static extern void EndWriting();

    string m_Path;
    string fn;

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

        SaveToFile(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z, PlayerHealth.hp.health, MissionManager.instance.lastMission);

        EndWriting();
    } 
}
