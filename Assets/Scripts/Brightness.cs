using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Brightness : MonoBehaviour
{


    public Slider brighnessSlider;
    public Text brighnessText;
    public PostProcessProfile brightness;
    public PostProcessLayer layer;
    public float brightnessPref = 1;

    AutoExposure exposure;
    // Start is called before the first frame update
    void Start()
    {
        brightness.TryGetSettings(out exposure);

        brightnessPref = PlayerPrefs.GetFloat("Currentbrightness", 1);
        brighnessSlider.value = brightnessPref;
    }

    void Update()
    {
        PlayerPrefs.SetFloat("CurrentBrightness", brightnessPref);
    }

    public void ChangeBrightness(float value)
    {
        exposure.keyValue.value = brightnessPref;
        brightnessPref = value;
        brighnessText.text = (brightnessPref*10).ToString("F0");
    }
}
