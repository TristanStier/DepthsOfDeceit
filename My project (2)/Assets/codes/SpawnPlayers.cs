using UnityEngine;
using Photon.Pun;
using System.Collections;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public Sprite[] playerSprites;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            int spriteIndex = Mathf.Clamp(playerCount - 1, 0, playerSprites.Length - 1);

            Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            // Instantiate the player
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);

            // Wait until the PhotonView is ready
            StartCoroutine(WaitForPhotonView(player, spriteIndex));
        }
        else
        {
            Debug.LogError("Photon is not connected. Make sure you are connected before trying to instantiate players.");
        }
    }

    IEnumerator WaitForPhotonView(GameObject player, int spriteIndex)
    {
        yield return new WaitUntil(() => player.GetComponent<PhotonView>().IsMine);

        // Now it's safe to assign the sprite
        player.GetComponent<SpriteRenderer>().sprite = playerSprites[spriteIndex];

        // Instantiate the camera and set it as a child of the player
        GameObject camera = Instantiate(cameraPrefab, player.transform);
        camera.transform.localPosition = new Vector3(0f, 0f, camera.transform.localPosition.z);
    }
}
