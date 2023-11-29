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
    public GameObject[] playerArray;
    public int i = 0;

    private void Start()
    {
        spawnPlayer();
    }

    public void spawnPlayer() {
        if (PhotonNetwork.IsConnected)
        {
            Vector2 randomPosition = new Vector2(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
            playerArray[i] = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
            playerArray[i].name = "Player"+i.ToString();
            i += 1;
        }
        else
        {
            Debug.LogError("Photon is not connected. Make sure you are connected before trying to instantiate players.");
        }
    }
}