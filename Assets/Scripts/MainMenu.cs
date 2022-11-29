using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject resume;
    public Text sensText;
    public Slider sensSlider;

    public float sens;
    public GameObject player;



    void Start()
    {
        player.GetComponentInChildren<PlayerController>().mouseSensitivity = sens;
        sens = PlayerPrefs.GetFloat("CurrentSens", 100);
        sensSlider.value = sens / 10;
    }
    private void Update()
	{
       

        PlayerPrefs.SetFloat("CurrentSens", sens );
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

    public void Senestivity(float sensi)
    {
        sens = sensi * 10;
        sensText.text = sens.ToString("F0");
    }



    public void Quit()
    {
        Debug.Log("You Cant Leave :D");
        Application.Quit();
    }
}
