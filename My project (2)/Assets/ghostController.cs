using UnityEngine;

public class GhostController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the "Ground" or "Ghost"
        if (gameObject.CompareTag("Ghost") && other.CompareTag("Ground"))
        {
            Debug.Log("Ghost collided with Ground. Disabling Ground collider.");

            // Disable the collider to allow the object to go through the ground
            other.enabled = false;

            // Invoke a method to re-enable the collider after 1 second
            Invoke("EnableCollider", 4f);
        }
        else if (gameObject.CompareTag("Ground") && other.CompareTag("Ghost"))
        {
            Debug.Log("Ground collided with Ghost. Disabling Ghost collider.");

            // Disable the collider of the ghost object to allow it to go through the ground
            GetComponent<Collider>().enabled = false;

            // Invoke a method to re-enable the collider after 1 second
            Invoke("EnableCollider", 4f);
        }
    }

    private void EnableCollider()
    {
        Debug.Log("Re-enabling collider.");

        // Re-enable the collider after the specified delay
        GetComponent<Collider>().enabled = true;
    }
}
