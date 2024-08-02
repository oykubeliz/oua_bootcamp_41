using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuPanel, settingsMenuPanel;
    public GameObject button1, button2, button3;
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Settings()
    {
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
        settingsMenuPanel.SetActive(true);
    }

    public void BacktoSettings()
    {
        button1.SetActive(true);
        button2.SetActive(true);
        button3.SetActive(true);
        settingsMenuPanel.SetActive(false);
    }
    

     public void ReturnMainMenu()
     {
         SceneManager.LoadScene("MainMenu");
     }
    
    
    public void Quit () 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
    
}
