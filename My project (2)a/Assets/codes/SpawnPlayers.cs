using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public Sprite[] playerSprites;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Get the number of players in the room
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            // Ensure the player count is within the bounds of your sprite array
            int spriteIndex = Mathf.Clamp(playerCount - 1, 0, playerSprites.Length - 1);

            Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            // Instantiate the player with the assigned sprite
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
            player.GetComponent<SpriteRenderer>().sprite = playerSprites[spriteIndex];

            // Instantiate the camera and set it as a child of the player
            GameObject camera = Instantiate(cameraPrefab, player.transform);
            camera.transform.localPosition = new Vector3(0f, 0f, camera.transform.localPosition.z);
        }
        else
        {
            Debug.LogError("Photon is not connected. Make sure you are connected before trying to instantiate players.");
        }
    }
}