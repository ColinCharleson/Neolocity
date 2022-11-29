using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public Sound[] music, sfx, ambient;
    public AudioSource musicSource, sfxSource, ambientSource;
    public AudioSource walkSource, runSoruce;

    public Slider _musicSlider, _ambientSlider, _sfxSlider;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("BackgroundMusic");
        PlayAmbient("Rain");
    }
    

    //Sound Sliders and Toggles
   
    public void MusicVolume()
    {
        SoundManager.Instance.MusicVolume(_musicSlider.value);
    }
    public void AmbientVolume()
    {
        SoundManager.Instance.AmbientVolume(_ambientSlider.value);
    }
    public void SFXVolume()
    {
        SoundManager.Instance.SFXVolume(_sfxSlider.value);
    }
    public void PlayMusic(string name)
    {
        Sound soundArray = Array.Find(music, x => x.name == name);

        if (soundArray == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = soundArray.clip;
            musicSource.Play();
        }
    }
    public void PlayAmbient(string name)
    {
        Sound soundArray = Array.Find(ambient, x => x.name == name);

        if (soundArray == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            ambientSource.clip = soundArray.clip;
            ambientSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        Sound soundArray = Array.Find(sfx, x => x.name == name);

        if (soundArray == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(soundArray.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleAmbient()
    {
        ambientSource.mute = !ambientSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void AmbientVolume(float volume)
    {
        ambientSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        walkSource.volume = volume;
        runSoruce.volume = volume;
    }
}
