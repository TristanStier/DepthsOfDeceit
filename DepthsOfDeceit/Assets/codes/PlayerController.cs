using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5f;

    void Update()
    {
        if (photonView.IsMine)
        {
            // Handle movement only for the local player

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * speed * Time.deltaTime;

            // Apply movement to the parent object
            transform.parent.Translate(movement);
        }

    }
}