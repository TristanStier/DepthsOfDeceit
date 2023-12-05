using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImpWin : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView == null)
        {
            Debug.LogError("PhotonView component is missing on the GameObject with ImpWin script.");
        }
    }

    private void Update()
    {
        // Check win conditions in every frame
        // moved to impostor.cs for efficiency
        //CheckWinConditions();
    }

    // Check if the impostors outnumber the players
    public void CheckWinConditions()
    {
        if (photonView == null)
        {
            // Log an error and return if photonView is null
            Debug.LogError("PhotonView is null in CheckWinConditions method.");
            return;
        }

        int impostors = 0;
        int players = 0;

        // Count players and impostors
        foreach (PhotonView pv in PhotonNetwork.PhotonViews)
        {
            if (pv.IsMine)
            {
                if (pv.gameObject.CompareTag("Player"))
                    players++;
                else if (pv.gameObject.CompareTag("Impostor"))
                    impostors++;
            }
        }

        // Check win conditions
        if (impostors >= players) // >=
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