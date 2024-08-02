using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource myAudioSource;
    private bool isMuted = false;

    // Ses açma/kapama fonksiyonu
    public void ToggleSound()
    {
        isMuted = !isMuted;
        myAudioSource.mute = isMuted;
    }
}
