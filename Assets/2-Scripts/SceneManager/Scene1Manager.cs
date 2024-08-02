using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Manager : MonoBehaviour
{
    public SceneTransitionData sceneTransitionData;
    
    public Transform BoatSpawnPoint;
    public GameObject player;

    void Start()
    {
        if (sceneTransitionData.fromScene2)
        {
            
            Debug.Log("<color=orange><b>Scene1'e Scene2'den geçildi.!</b></color>");
            
            
            sceneTransitionData.fromScene2 = false;

            if (player != null && BoatSpawnPoint != null)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = BoatSpawnPoint.position;
                player.transform.rotation = BoatSpawnPoint.rotation;
                player.GetComponent<CharacterController>().enabled = true;

                Debug.Log("Player position set to: " + BoatSpawnPoint.position);
                Debug.Log("Player rotation set to: " + BoatSpawnPoint.rotation);
            }
            else
            {
                Debug.LogWarning("Player veya DefaultSpawnPoint referansı atanmamış.");
            }
        }
    }
}
