using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX;
    public float maxX;

    public float minY;
    public float maxY;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Photon is not connected. Make sure you are connected before trying to instantiate players.");
        }
    }
}