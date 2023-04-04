using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class UiManagerM : MonoBehaviour
{
    public Canvas gameUI, pauseUI, mutiplayer;

    // Start is called before the first frame update
    void Start()
    {
        mutiplayer.enabled = true;
        gameUI.enabled = false;
        pauseUI.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    { 
        if(mutiplayer == false)
        {
            gameUI.enabled = true; 
            pauseUI.enabled = false;

            if (Input.GetKeyDown(InputSystem.key.pause))
            {
                gameUI.enabled = !gameUI.enabled;
                pauseUI.enabled = !pauseUI.enabled;
            }

            if (pauseUI.enabled)
            {
                Time.timeScale = 1;
                pauseUI.enabled = true;
                gameUI.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                pauseUI.enabled = false;
                gameUI.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
