using Photon.Pun;
using UnityEngine;

public class Impostor : MonoBehaviourPunCallbacks
{
    public float killCooldown = 10f;
    public float killDistance = 2f;
    public GameObject bodyPrefab;
    private float lastKillTime;
    public ImpWin impWinS;

    void Start() {
        impWinS = GameObject.FindGameObjectWithTag("impwinobj").GetComponent<ImpWin>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastKillTime >= killCooldown && gameObject.CompareTag("Impostor"))
            {
                TryKill();
            }
        }
    }

    void TryKill()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            if (player != gameObject)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance <= killDistance)
                {
                    photonView.RPC("KillRPC", RpcTarget.All, player.GetComponent<PhotonView>().ViewID);
                    break;
                }
            }
        }
    }
    

    [PunRPC]
    void KillRPC(int targetViewID)
    {
        GameObject targetPlayer = PhotonView.Find(targetViewID).gameObject;

        // Only the killer's client executes this code
        Renderer targetRenderer = targetPlayer.GetComponent<Renderer>();

        // Use MaterialPropertyBlock to modify the material's alpha value
        MaterialPropertyBlock materialBlock = new MaterialPropertyBlock();
        targetRenderer.GetPropertyBlock(materialBlock);

        // Adjust the transparency (alpha value) of the material
        materialBlock.SetFloat("_Mode", 2); // Set the material rendering mode to transparent
        materialBlock.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        materialBlock.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

        // Set the color directly in the MaterialPropertyBlock
        materialBlock.SetColor("_Color", new Color(1f, 1f, 1f, 0.0f)); // Set alpha to 0.5 for half transparency

        // Apply the modified MaterialPropertyBlock to the renderer
        targetRenderer.SetPropertyBlock(materialBlock);

        // Disable shadows for the killed player
        UnityEngine.Rendering.Universal.ShadowCaster2D targetShadowCaster = targetPlayer.GetComponent<UnityEngine.Rendering.Universal.ShadowCaster2D>();
        if (targetShadowCaster != null)
        {
            targetShadowCaster.enabled = false;
        }

        // Disable the collider to allow the killed player to go through walls
        CapsuleCollider2D targetCollider = targetPlayer.GetComponent<CapsuleCollider2D>();
        if (targetCollider != null)
        {
            targetCollider.enabled = false;
        }
        targetPlayer.tag = "Ghost";

        // Spawn a body prefab at the target player's position
        Instantiate(bodyPrefab, targetPlayer.transform.position, Quaternion.identity);

        // Reset the cooldown timer
        lastKillTime = Time.time;

        // Synchronize the target player's state across the network
        photonView.RPC("DisablePlayerForOthersRPC", RpcTarget.OthersBuffered, targetViewID);
        impWinS.CheckWinConditions();
    }

    [PunRPC]
    void DisablePlayerForOthersRPC(int viewID)
    {
        if (PhotonView.Find(viewID).IsMine)
        {
            // Get the target player's GameObject
            GameObject targetPlayer = PhotonView.Find(viewID).gameObject;

            // Get the Renderer component of the target player
            Renderer targetRenderer = targetPlayer.GetComponent<Renderer>();

            // Use MaterialPropertyBlock to modify the material's alpha value
            MaterialPropertyBlock materialBlock = new MaterialPropertyBlock();
            targetRenderer.GetPropertyBlock(materialBlock);

            // Make the target player half transparent
            materialBlock.SetFloat("_Mode", 2); // Set the material rendering mode to transparent
            materialBlock.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            materialBlock.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            // Set the color directly in the MaterialPropertyBlock
            materialBlock.SetColor("_Color", new Color(1f, 1f, 1f, 0.5f)); // Set alpha to 0.5 for half transparency

            // Apply the modified MaterialPropertyBlock to the renderer
            targetRenderer.SetPropertyBlock(materialBlock);

            // Disable the renderer for everyone except the target player
        }
    }
}
