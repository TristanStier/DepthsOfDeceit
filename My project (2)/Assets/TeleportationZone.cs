using UnityEngine;
using Photon.Pun;

public class TeleportationZone : MonoBehaviourPun
{
    public Transform destination; // Assign the destination object in the Inspector
    public string impostorTag = "Impostor";

    private bool impostorAssigned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            photonView.RPC("TeleportPlayer", RpcTarget.All, other.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    private void TeleportPlayer(int playerViewID)
    {
        GameObject player = PhotonView.Find(playerViewID).gameObject;
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        if (allPlayers.Length >= 2)
        {
            foreach (GameObject p in allPlayers)
            {
                if (p == player)
                {
                    if (!impostorAssigned)
                    {
                        p.tag = impostorTag;
                        impostorAssigned = true;
                    }

                    // Teleport the player to the destination
                    p.transform.position = destination.position;
                }
            }
        }
    }
}
