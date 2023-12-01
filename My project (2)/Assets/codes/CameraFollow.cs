using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraFollow : MonoBehaviour
{
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;
    private Transform target;
    private PhotonView view;
    public bool minigame = false;

    void Start()
    {
        view = GetComponent<PhotonView>();

        // Initialize the target based on the local player
        if (PhotonNetwork.IsConnected && view.IsMine)
        {
            target = transform;  // Use the camera's own transform if it's the local player's camera
        }
        else
        {
            // Use the parent of the camera as the target
            if (transform.parent != null)
            {
                target = transform.parent;
            }
            else
            {
                Debug.LogWarning("Camera has no parent. Set a parent object for the camera to follow.");
            }
        }
    }

    void Update()
    {
        // Check if the view is not null and if it's the local player's view before updating camera position
        if (view.IsMine && !minigame)
        {

                // Smoothly move the camera towards the target position
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        }
    }
}