using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ToggleMotionBlur : MonoBehaviour
{

    private PostProcessVolume postProcessVolume;
    private MotionBlur motionBlur;

    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out motionBlur);
    }

    public void MotionBlurOnOff(bool on)
    {
        if(on)
        {
            motionBlur.active = true;
        }
        else
        {
            motionBlur.active = false;
        }
    }
}
