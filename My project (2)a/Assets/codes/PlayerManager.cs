using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        // Instantiate the player prefab for the local player only
        if (PhotonNetwork.IsConnected)
        {
            if(photonView.IsMine){

           
            PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        }
         }
    }
}