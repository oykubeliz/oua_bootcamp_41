using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;
    public GameObject LoadingScreen;
    public Slider ProgressBar;
    
    public void SwitchToScene()
    {
        LoadingScreen.SetActive(true);
        ProgressBar.value = 0;
        StartCoroutine(SwitchToSceneAsync());
    }

    IEnumerator SwitchToSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Map1");
        while (!asyncLoad.isDone)
        {
            ProgressBar.value = asyncLoad.progress;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        LoadingScreen.SetActive(false);
    }
}
