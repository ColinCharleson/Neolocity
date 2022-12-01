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
    public Text _musicText, _ambientText, _sfxText;
    public float musicVolume = 1f;
    public float ambientVolume = 1f;
    public float sfxVolume = 1f;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        PlayMusic("BackgroundMusic");
        PlayAmbient("Rain");
        musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        musicSource.volume = musicVolume;
        _musicSlider.value = musicVolume;
     
        ambientVolume = PlayerPrefs.GetFloat("AmbientVolume");
        ambientSource.volume = ambientVolume;
        _ambientSlider.value = ambientVolume;

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        walkSource.volume = sfxVolume;
        runSoruce.volume = sfxVolume;
        _sfxSlider.value = sfxVolume;
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        musicSource.volume = musicVolume;

        PlayerPrefs.SetFloat("AmbientVolume", ambientVolume);
        ambientSource.volume = ambientVolume;

        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        walkSource.volume = sfxVolume;
        runSoruce.volume = sfxVolume;
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

    public void MusicVolume(float volume)
    {
        musicVolume = volume;
        _musicText.text = (musicVolume*100).ToString("0");

    }
    public void AmbientVolume(float volume)
    {
        ambientVolume = volume;
        _ambientText.text = (ambientVolume *100).ToString("0");
    }
    public void SFXVolume(float volume)
    {
        sfxVolume = volume;
        _sfxText.text = (sfxVolume*100).ToString("0");
    }

    public void SetVolumeSliders()
    {
        _sfxText.text = (sfxVolume * 100).ToString("0");
        _ambientText.text = (ambientVolume * 100).ToString("0");
        _musicText.text = (musicVolume * 100).ToString("0");
    }
}
