using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarExit : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadSlider;

    void OnMouseDown()
    {
        PlayGame(2);
    }

    public void PlayGame(int levelindex)
    {
        StartCoroutine(LoadSceneAsyncs(levelindex));
    }

    IEnumerator LoadSceneAsyncs(int levelindex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelindex);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            loadSlider.value = operation.progress;
            yield return null;
        }
    }
}
