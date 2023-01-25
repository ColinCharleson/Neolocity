using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class ToggleMotionBlur : MonoBehaviour
{

    private PostProcessVolume postProcessVolume;
    private MotionBlur motionBlur;
    public float motionOnOff;
     public Toggle MotionToggle;

    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out motionBlur);

        motionOnOff = MotionToggle ? 1 : 0;

        if (PlayerPrefs.GetInt("Motion Blur Toggle") == 1)
        {
            MotionToggle.isOn = true;
        }
        else
        {
            MotionToggle.isOn = false;
        }
    }

    void Update()
    {
        PlayerPrefs.SetFloat("ToggleMotion", motionOnOff);


        if (MotionToggle.isOn == true)
        {
            PlayerPrefs.SetInt("Motion Blur Toggle", 1);
        }
        if (MotionToggle.isOn == false)
        {
            PlayerPrefs.SetInt("Motion Blur Toggle", 0);
        }
    }

    public void MotionBlurOnOff(bool on)
    {
        motionOnOff = on ? 1 : 0;

        if (motionOnOff == 1)
        {
            motionBlur.active = true;
        }
        else
        {
            motionBlur.active = false;
        }
    }
}
