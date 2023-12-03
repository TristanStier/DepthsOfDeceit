using UnityEngine;
using Photon.Pun;

public class Lights : MonoBehaviourPunCallbacks
{
    public UnityEngine.Rendering.Universal.Light2D playerLight2D; // Use Light2D type

    void Start()
    {
        if (photonView.IsMine)
        {
            // Enable the light for the local player
            playerLight2D.enabled = true;
        }
        else
        {
            // Disable the light for other players
            playerLight2D.enabled = false;
        }
    }

}