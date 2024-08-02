using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIconToggle : MonoBehaviour
{
    public Sprite image1;
    public Sprite image2;
    private Image buttonImage;
    private bool isImage1Active;
    
    void Start()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = image1;
            isImage1Active = true;
        }
    }

    public void ToggleButtonImage()
    {
        if (isImage1Active)
        {
            buttonImage.sprite = image2;
        }
        else
        {
            buttonImage.sprite = image1;
        }
        isImage1Active = !isImage1Active;
    }
}