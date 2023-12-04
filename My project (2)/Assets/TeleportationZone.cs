using UnityEngine;
using Photon.Pun;

public class TeleportationZone : MonoBehaviourPun
{
    public Transform destination; // Assign the destination object in the Inspector
    public string impostorTag = "Impostor";

    private bool impostorAssigned = false;
    private int playersInsideCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !impostorAssigned)
        {
            playersInsideCount++;

            if (playersInsideCount == PhotonNetwork.PlayerList.Length)
            {
                photonView.RPC("TeleportPlayers", RpcTarget.All);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !impostorAssigned)
        {
            playersInsideCount--;
        }
    }

    [PunRPC]
    private void TeleportPlayers()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in allPlayers)
        {
            if (!impostorAssigned)
            {
                player.tag = impostorTag;
                impostorAssigned = true;
            }

            // Teleport all players to the destination
            player.transform.position = destination.position;
        }
    }
}
