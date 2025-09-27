/*
 * Name: E. Nathan Lee
 * Date: 9/27/2025
 * Description: Controls camera to follow player. Original script from the tutorial.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    private Vector3 offset; // Offset between camera and player

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position; // Tracking initial position to find center
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset; // Follow player based on movement from center (0,0)
    }
}
