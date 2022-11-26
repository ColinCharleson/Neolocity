using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public Sound[] music, sfx, ambient;
    public AudioSource musicSource, sfxSource, ambientSource;

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
}
