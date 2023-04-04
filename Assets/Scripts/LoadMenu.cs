using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("Mainmenu");
        Debug.Log("Load");
    }
}
