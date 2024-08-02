using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Boat1Interaction : MonoBehaviour
{
    public string sailMessage = "Press F to sail";
    private bool isPlayerNear = false;
    public VideoPlayer videoPlayer; // VideoPlayer referansı
    private bool videoPlayed = false;
    public GameObject[] uiElements; // İnaktif hale getirmek istediğiniz UI elemanlarının referansları

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F) && !videoPlayed)
        {
            videoPlayed = true;
            SetUIElementsActive(false); // UI elemanlarını inaktif hale getirme
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoEnd; // Video bittiğinde çağrılacak fonksiyon
            StartCoroutine(LoadSceneInBackground());
        }
    }

    void OnGUI()
    {
        if (isPlayerNear && !videoPlayed)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            GUI.Label(new Rect(screenPosition.x - 100, Screen.height - screenPosition.y - 50, 200, 50), sailMessage);
        }
    }

    private IEnumerator LoadSceneInBackground()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Map2");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (videoPlayed && asyncLoad.progress >= 0.9f)
            {
                // asyncLoad.allowSceneActivation = true;
                break;
            }
            yield return null;
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("Map2");
    }
    
    private void SetUIElementsActive(bool isActive)
    {
        foreach (GameObject element in uiElements)
        {
            element.SetActive(isActive);
        }
    }
}
