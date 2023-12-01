using UnityEngine;
using Photon.Pun;
using System;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX;
    public float maxX;

    public float minY;
    public float maxY;

    private void Start()
    {
        spawnPlayer();
    }

    public void spawnPlayer() {
        if (PhotonNetwork.IsConnected)
        {
            Vector2 randomPosition = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
            player.tag = "Player";
        }
        else
        {
            Debug.LogError("Photon is not connected. Make sure you are connected before trying to instantiate players.");
        }
    }
}