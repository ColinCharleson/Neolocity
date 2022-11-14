using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas gameUI, pauseUI;
    // Start is called before the first frame update
    void Start()
    {
        gameUI.enabled = true;
        pauseUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
		{
            gameUI.enabled = !gameUI.enabled;
            pauseUI.enabled = !pauseUI.enabled;
        }

        if(pauseUI.enabled == true)
		{
            Time.timeScale = 0;
		}
		else
		{
            Time.timeScale = 1;
		}
    }
}
