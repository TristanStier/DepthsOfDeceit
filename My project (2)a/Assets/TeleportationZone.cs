using UnityEngine;
using Photon.Pun;

public class TeleportationZone : MonoBehaviourPun
{
    public Transform destination; // Assign the destination object in the Inspector
    public Sprite[] teleporterSprites; // Assign different sprites to this array in the Inspector
    public string impostorTag = "Impostor";

    private bool impostorAssigned = false;

    private static int nextSpriteIndex = 0; // Static variable to keep track of the next available sprite index

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !impostorAssigned)
        {
            int playerSpriteIndex = nextSpriteIndex;
            photonView.RPC("TeleportPlayer", RpcTarget.All, other.GetComponent<PhotonView>().ViewID, playerSpriteIndex);
            
            nextSpriteIndex = (nextSpriteIndex + 1) % teleporterSprites.Length; // Move to the next sprite index
        }
    }

    [PunRPC]
    private void TeleportPlayer(int playerViewID, int playerSpriteIndex)
    {
        GameObject player = PhotonView.Find(playerViewID).gameObject;
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        if (allPlayers.Length >= 1)
        {
            foreach (GameObject p in allPlayers)
            {
                if (p == player)
                {
                    if (p == allPlayers[playerSpriteIndex] && !impostorAssigned)
                    {
                        p.tag = impostorTag;
                        impostorAssigned = true;
                    }

                    // Teleport the player to the destination
                    p.transform.position = destination.position;

                    // Change the player's sprite directly
                    SpriteRenderer playerSpriteRenderer = p.GetComponent<SpriteRenderer>();
                    if (playerSpriteRenderer != null && teleporterSprites.Length > 0)
                    {
                        playerSpriteRenderer.sprite = teleporterSprites[playerSpriteIndex];
                    }
                }
            }
        }
    }
}