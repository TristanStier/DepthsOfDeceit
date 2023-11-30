using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab; // Reference to the camera prefab
    public Sprite[] playerSprites; // Array of different player sprites

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            // Instantiate the player
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);

            // Assign a sprite based on the first available color
            Sprite selectedSprite = GetAvailableColor();

            // Set the sprite on the player's sprite renderer (assuming the sprite renderer is on the player object)
            SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            if (playerSpriteRenderer != null && selectedSprite != null)
            {
                playerSpriteRenderer.sprite = selectedSprite;
            }
            else
            {
                Debug.LogError("SpriteRenderer not found on the player object or no available color found.");
            }

            // Instantiate the camera and set it as a child of the player
            GameObject camera = Instantiate(cameraPrefab, player.transform);
            camera.transform.localPosition = new Vector3(0f, 0f, camera.transform.localPosition.z);
        }
        else
        {
            Debug.LogError("Photon is not connected. Make sure you are connected before trying to instantiate players.");
        }
    }

    // Function to get the first available color
    private Sprite GetAvailableColor()
    {
        foreach (Sprite sprite in playerSprites)
        {
            // Check if the sprite is already assigned to another player
            bool spriteTaken = IsSpriteTaken(sprite);

            if (!spriteTaken)
            {
                // The sprite is available, return it
                return sprite;
            }
        }

        // No available sprite found
        Debug.LogError("No available sprite found.");
        return null;
    }

    // Function to check if a sprite is taken by another player
    private bool IsSpriteTaken(Sprite spriteToCheck)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            if (playerSpriteRenderer != null && playerSpriteRenderer.sprite == spriteToCheck)
            {
                // The sprite is already assigned to another player
                return true;
            }
        }

        // The sprite is not taken
        return false;
    }
}