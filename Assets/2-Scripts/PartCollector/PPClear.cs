using UnityEngine;
using TMPro;

public class PlayerPrefsUtility : MonoBehaviour
{
    public TMP_Text textElement; // TextMesh Pro Text elemanı
    public void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        PartCollector.PARTCOLLECTORResetCollectedParts();
        Debug.Log("All PlayerPrefs have been cleared.");
    }
    
    public void ClearText(string newText)
    {
        if (textElement != null)
        {
            textElement.text = newText;
        }
        else
        {
            Debug.LogWarning("Text element is not assigned!");
        }
    }
    
    
}