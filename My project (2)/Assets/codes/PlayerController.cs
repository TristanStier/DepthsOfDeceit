using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 15f;
    public bool minigame = false;

    void Update()
    {
        if (photonView.IsMine && !minigame)
        {
            // Check if the parent object has the tag "impostor"
            if (transform.parent != null && transform.parent.CompareTag("Impostor"))
            {
                speed = 8f; // Adjust speed for the impostor
            }

            // Handle movement only for the local player
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * speed * Time.deltaTime;

            // Apply movement to the parent object
            transform.parent.Translate(movement);
        }
    }
}
