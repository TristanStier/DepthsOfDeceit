using UnityEngine;

public class Ladder : MonoBehaviour
{
    // Adjust this speed to control how fast the player climbs
    public float climbSpeed = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable gravity for the player
            other.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Simulate climbing by adjusting the player's velocity
            float verticalInput = Input.GetAxis("Vertical");
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, verticalInput * climbSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Enable gravity for the player
            other.GetComponent<Rigidbody2D>().gravityScale = 1f;
        }
    }
}