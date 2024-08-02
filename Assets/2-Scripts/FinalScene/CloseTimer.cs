using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTimer : MonoBehaviour
{
    private float timer = 0f;
    private float shutdownTime = 240f; // 4 dakika (240 saniye)

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shutdownTime)
        {
            QuitGame();
        }
    }

    void QuitGame()
    {
        // Unity Editor'da çalışırken oyunu durdurmak için
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Build edilmiş oyunda uygulamayı kapatmak için
        Application.Quit();
#endif
    }
}
