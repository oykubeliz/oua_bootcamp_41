using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public float threshold;
    public Vector3 playerPosition;
    [SerializeField] private List<GameObject> checkpoint;
    [SerializeField] private Vector3 vectorPoint;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        // Removed the y position threshold check
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CP"))
        {
            vectorPoint = other.transform.position;
            playerPosition = vectorPoint;
        }
        else if (other.gameObject.CompareTag("DMG"))
        {
            player.GetComponent<CharacterController>().enabled = false;
            transform.position = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}