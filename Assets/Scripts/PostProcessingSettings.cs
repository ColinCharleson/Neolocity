using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class PostProcessingSettings : MonoBehaviour
{
    private Volume postProcessVolume;
    
    MotionBlur mB;
    public Toggle mBToggle;

    LiftGammaGain brightness;
    public Slider brightnessSlider;
    public Text brightnessCounter;
    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<MotionBlur>(out mB);
        postProcessVolume.profile.TryGet<LiftGammaGain>(out brightness);

        if (PlayerPrefs.GetInt("MotionBlur") == 1)
        {
            mBToggle.isOn = true;
            mB.active = true;
        }
        else
        {
            mBToggle.isOn = false;
            mB.active = false;
        }

        brightness.gain.value = new Vector4(0, 0, 0, PlayerPrefs.GetFloat("Brightness"));
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
        brightnessCounter.text = ((50 * brightnessSlider.value) + 50).ToString("0");
    }
	public void BrightnessChange()
    {
       brightness.gain.value = new Vector4(0, 0, 0, brightnessSlider.value);
       PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        brightnessCounter.text = ((50 * brightnessSlider.value) + 50).ToString("0");
    }
    public void MotionBlurToggle()
    {
        mB.active = !mB.active;

        if (mB.active)
        {
            PlayerPrefs.SetInt("MotionBlur", 1);
        }
		else
        {
            PlayerPrefs.SetInt("MotionBlur", 0);
        }
    }
}