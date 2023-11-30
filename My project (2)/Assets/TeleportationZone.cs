using UnityEngine;

public class TeleportationZone : MonoBehaviour
{
    // Set the teleport destination
    public Transform teleportDestination;

    // Minimum number of players needed to trigger teleportation
    public int minPlayersRequired = 5;

    // Delay before teleportation (in seconds)
    public float teleportDelay = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check the number of players in the scene
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            if (players.Length >= minPlayersRequired)
            {
                // Invoke the TeleportPlayers method after the delay
                Invoke("TeleportPlayers", teleportDelay);
            }
            else
            {
                Debug.Log("Not enough players to trigger teleportation.");
            }
        }
    }

    // Method to teleport players
    private void TeleportPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Teleport all players to the destination
        foreach (GameObject player in players)
        {
            player.transform.position = teleportDestination.position;
        }
    }
}