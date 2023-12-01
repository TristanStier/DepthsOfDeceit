/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scrollSpeed = 100000.0f;
    public float distanceToMove = 100.0f;

    private Vector3 startPosition;
    private bool moveRight = true;
    public bool minigame = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!minigame) {
            // Calculate movement direction based on moveRight flag
            Vector3 movementDirection = moveRight ? Vector3.right : Vector3.left;

            // Move the camera
            transform.Translate(movementDirection * 0.08f * scrollSpeed * Time.deltaTime);

            // Check if the camera moved the desired distance
            if (Mathf.Abs(transform.position.x - startPosition.x) >= distanceToMove)
            {
                // Change direction
                moveRight = !moveRight;
            }
        }
    }
}*/