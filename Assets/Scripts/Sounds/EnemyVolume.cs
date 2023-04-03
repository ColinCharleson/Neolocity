using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EnemyVolume : MonoBehaviour
{
    [SerializeField]
    private AudioMixer Mixer;
    [SerializeField]
    private Slider enemySlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("EnemyVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetEnemyVolume();
        }
    }

    public void SetEnemyVolume()
    {
        float volume = enemySlider.value;
        Mixer.SetFloat("EnemySound",Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("EnemyVolume", volume);
    }

    private void LoadVolume()
    {
        enemySlider.value = PlayerPrefs.GetFloat("EnemyVolume");

        SetEnemyVolume(); 
    }
}
