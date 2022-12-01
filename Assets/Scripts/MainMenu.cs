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
    public Text fovText;
    public Slider fovSlider;
    public float fieldOV;
    public float sens;

    public Text musicText;
    public Slider musicSlider;
    public Text ambientText;
    public Slider ambientlider;
    public Text sfxText;
    public Slider sfxSlider;
    public GameObject player;
    public GameObject soundManager;
    public float sfx, ambient , music;
 

    void Start()
    {
        player.GetComponentInChildren<PlayerController>().mouseSensitivity = sens;
        sens = PlayerPrefs.GetFloat("CurrentSens", 100);
        sensSlider.value = sens / 10;

        player.GetComponentInChildren<PlayerController>().fov = fieldOV;
        fieldOV = PlayerPrefs.GetFloat("CurrentFov", 60);
        fovSlider.value = fieldOV;

        soundManager.GetComponent<SoundManager>().musicVolume = music;
        music = PlayerPrefs.GetFloat("MusicVolume");
        musicSlider.value = music;

        soundManager.GetComponent<SoundManager>().ambientVolume = ambient;
        ambient = PlayerPrefs.GetFloat("AmbientVolume");
        ambientlider.value = ambient;

        soundManager.GetComponent<SoundManager>().sfxVolume = sfx;
        sfx = PlayerPrefs.GetFloat("SFXVolume");
        sfxSlider.value = sfx;
    }
    private void Update()
	{
        PlayerPrefs.SetFloat("CurrentFov", fieldOV);
        PlayerPrefs.SetFloat("CurrentSens", sens);
        PlayerPrefs.SetFloat("MusicVolume", music);
        PlayerPrefs.SetFloat("AmbientVolume", ambient);
        PlayerPrefs.SetFloat("SFXVolume", sfx);

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

    public void MusicVolume(float volume)
    {
        music = volume;
        musicText.text = (music * 100).ToString("0");

    }
    public void AmbientVolume(float volume)
    {
        ambient = volume;
        ambientText.text = (ambient * 100).ToString("0");
    }
    public void SFXVolume(float volume)
    {
        sfx = volume;
        sfxText.text = (sfx * 100).ToString("0");
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

    public void ChangeFov(float fOV)
    {
        fieldOV = fOV;
        fovText.text = fieldOV.ToString("F0");
    }
    public void Quit()
    {
        Debug.Log("You Cant Leave :D");
        Application.Quit();
    }
}
