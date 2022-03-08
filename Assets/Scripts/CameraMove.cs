using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that moves the camera.
public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform playerPawn;

    // Move the camera so that the player is vertically centered.
    void Update()
    {
        transform.position = new Vector3(0, playerPawn.transform.position.y, -10);
    }
}
