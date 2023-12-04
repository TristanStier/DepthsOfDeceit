using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImpWin : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        // Check win conditions in every frame
        CheckWinConditions();
    }

    // Check if the impostors outnumber the players
    private void CheckWinConditions()
    {
        int impostors = 0;
        int players = 0;

        // Count players and impostors
        foreach (PhotonView photonView in PhotonNetwork.PhotonViews)
        {
            if (photonView.IsMine)
            {
                if (photonView.gameObject.CompareTag("Player"))
                    players++;
                else if (photonView.gameObject.CompareTag("Impostor"))
                    impostors++;
            }
        }

        // Check win conditions
        if (impostors >= players)
        {
            photonView.RPC("ShowWinScene", RpcTarget.All);
        }
    }

    // RPC to show the win scene for all players
    [PunRPC]
    private void ShowWinScene()
    {
        SceneManager.LoadScene("ImpostorWin");
    }
}
