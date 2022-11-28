using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject resume;
	private void Update()
	{
        string filePath = Application.dataPath + "/save.txt";
        if (System.IO.File.Exists(filePath))
        {
            resume.SetActive(true);
		}
		else
        {
            resume.SetActive(false);
        }
	}
	public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void Quit()
    {
        Debug.Log("You Cant Leave :D");
        Application.Quit();
    }
}
