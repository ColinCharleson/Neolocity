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
 /*   public float sfx, ambient , music;
   private AudioSource musicSource, sfxSource, ambientSource;
    private AudioSource walkSource, runSoruce;*/


    void Start()
    {
        player.GetComponentInChildren<PlayerController>().mouseSensitivity = sens;
        sens = PlayerPrefs.GetFloat("CurrentSens", 100);
        sensSlider.value = sens / 10;

        player.GetComponentInChildren<PlayerController>().fov = fieldOV;
        fieldOV = PlayerPrefs.GetFloat("CurrentFov", 60);
        fovSlider.value = fieldOV;

      /*  music = PlayerPrefs.GetFloat("MusicVolume");
        musicSource.volume = music;
        musicSlider.value = music;

        ambient = PlayerPrefs.GetFloat("AmbientVolume");
        ambientSource.volume = ambient;
        ambientlider.value = ambient;

        sfx = PlayerPrefs.GetFloat("SFXVolume");
        walkSource.volume = sfx;
        runSoruce.volume = sfx;
        sfxSlider.value = sfx;*/
    }
    private void Update()
	{
        PlayerPrefs.SetFloat("CurrentFov", fieldOV);
        PlayerPrefs.SetFloat("CurrentSens", sens);
        /*      PlayerPrefs.SetFloat("MusicVolume", music);
              musicSource.volume = music;

              PlayerPrefs.SetFloat("AmbientVolume", ambient);
              ambientSource.volume = ambient;

              PlayerPrefs.SetFloat("SFXVolume", sfx);
              walkSource.volume = sfx;
              runSoruce.volume = sfx;*/

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
/*
    public void MusicVolume(float volume)
    {
        music = volume;
        musicText.text = music.ToString("F0");
    }
    public void AmbientVolume(float volume)
    {
        ambient = volume;
        ambientText.text = ambient.ToString("F0");
    }
    public void SFXVolume(float volume)
    {
        sfx = volume;
        sfxText.text = sfx.ToString("F0");
    }*/

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
