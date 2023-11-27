using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;
    PhotonView view;

    void Start()
    {

        view = GetComponent<PhotonView>();

    }
    void LateUpdate()
    {       

        if (target != null)
        {
            // Smoothly move the camera towards the target position
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        
    }
}