using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PartCollector : MonoBehaviour
{
    public string collectMessage = "Press F to Collect";
    private bool isPlayerNear = false;
    public TextMeshProUGUI collectedPartsText; // TextMesh Pro Text elemanına referans
    private static int collectedParts = 0;
    private string objectID;

    void Start()
    {
        objectID = gameObject.name; // Unique identifier for each object, you can use a different method if necessary
        if (PlayerPrefs.GetInt(objectID, 0) == 1)
        {
            Destroy(gameObject);
        }
        UpdateCollectedPartsUI();
    }

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
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            collectedParts++;
            PlayerPrefs.SetInt(objectID, 1);
            Debug.Log("Collected parts: " + collectedParts);
            Destroy(gameObject);
            UpdateCollectedPartsUI();

            if (collectedParts == 6)
            {
                OnCollectedPartsReachedSix();
            }
        }
    }

    void OnGUI()
    {
        if (isPlayerNear)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            GUI.Label(new Rect(screenPosition.x - 100, Screen.height - screenPosition.y - 50, 200, 50), collectMessage);
        }
    }

    void UpdateCollectedPartsUI()
    {
        if (collectedPartsText != null)
        {
            collectedPartsText.text = "" + collectedParts;
        }
    }

    void OnCollectedPartsReachedSix()
    {
        SceneManager.LoadScene("Final");
        Debug.Log("Collected parts have reached 6!");
        // Add your custom logic here
    }

    public static void PARTCOLLECTORResetCollectedParts()
    {
        collectedParts = 0;
        //PlayerPrefs.DeleteAll();
    }
}
