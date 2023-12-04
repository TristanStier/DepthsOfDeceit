using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviourPunCallbacks //MonoBehaviour
{

    private PlayerScript playerScript;
    private AnimationScript animationScript;
    public AudioSource hit;
    public GameObject playerObj;
    public GameObject shapeObj;
    public GameObject wallsObj;
    public GameObject backgroundObj;
    public GameObject livesObj;
    public GameObject animationObj;
    public Slider taskBarObj;
    public GameObject taskBarGameObj;
    public GameObject AnimationObj;
    public GameObject playerInstance;
    public GameObject collidedPlayer;
    public IEnumerator currentLevel;
    public AudioSource currentMusic;
    public bool loaded = false;
    public Color invincibleColor = new Color(0, 200, 255);
    public Color regColor = new Color(255, 255, 255);
    public Vector3 previousCamPos;

    public AudioSource gameMusic;

    public List<GameObject> playerArray;

    // Start is called before the first frame update
    void Start()
    {
        animationScript = animationObj.GetComponent<AnimationScript>();
        taskBarObj.maxValue = (PhotonNetwork.PlayerList.Length-1) * 2; // 2 minigames per player
    }

    // Update is called once per frame
    void Update()
    {
        if (loaded) {
            if (playerScript.invulnerable) {
                for(int i = 0; i < livesObj.transform.childCount; i++) {
                    livesObj.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(0, 200, 255);
                }
            } else {
                for(int i = 0; i < livesObj.transform.childCount; i++) {
                    livesObj.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                }
            }
        }
        
    }
    [PunRPC]
    public void RpcStopSound(AudioSource audio)
    {
        if (photonView.IsMine) {
            audio.Stop();
        }
    }
    [PunRPC]
    public void RpcPlaySound(AudioSource audio)
    {
        if (photonView.IsMine) {
            audio.Play();
        }
    }
    [PunRPC]
    public void RpcTaskBar(GameObject taskbarObj, bool enabled)
    {
        if (photonView.IsMine) {
            taskBarObj.enabled = enabled;
        }
    }
    /*public void isMine(Func<AudioSource> musicFunc, AudioSource music) {
         if (photonView.IsMine)
        {
            photonView.RPC(musicFunc, music);
        }
    }*/

    [ContextMenu("Decrease Life")] // add it to Unity
    public void decreaseLife(int num) {
        if (!playerScript.invulnerable && playerScript.lifePoints > 0) {
            //hit.Play();
            photonView.RPC("RpcPlaySound", RpcTarget.All, hit);
            playerScript.lifePoints -= num;
            decreaseLifeGui(playerScript.lifePoints);
            playerScript.StartCoroutine(playerScript.setInvulnerable());
            Debug.Log("Decreasing life " + playerScript.lifePoints);
        } else if (!playerScript.invulnerable && playerScript.lifePoints <= 0) {
            photonView.RPC("RpcPlaySound", RpcTarget.All, hit);
            //RpcPlaySound(hit);
            decreaseLifeGui(playerScript.lifePoints);
            endMinigame(false);
        }
    }

    [ContextMenu("Game Over")] // add it to Unity
    public void endMinigame(bool won) {
        StopCoroutine(currentLevel);
        photonView.RPC("RpcStopSound", RpcTarget.All, currentMusic);
        //RpcStopSound(currentMusic);
        collidedPlayer.GetComponentInChildren<Camera>().transform.position = previousCamPos;
        collidedPlayer.GetComponentInChildren<CameraFollow>().minigame = false;
        collidedPlayer.GetComponentInChildren<PlayerMovement>().minigame = false;
        Destroy(playerInstance);
        //taskBarGameObj.SetActive(true);
        if (won) {
            playerArray.Add(collidedPlayer);
            taskBarObj.value += 1;
        }
        StartCoroutine(setUnLoaded()); // gives enough time for level to unload
        /*if (taskBarObj.value == taskBarObj.maxValue) {
            Win(); // Win for crewmates
        }*/
        photonView.RPC("RpcPlaySound", RpcTarget.All, gameMusic);
    }

    private IEnumerator setUnLoaded() {
        yield return new WaitForSeconds(2f);
        loaded = false;
    }

    public void decreaseLifeGui(int num) {
        livesObj.transform.GetChild(num).GetComponent<SpriteRenderer>().enabled = false;
    }

    public void beginMinigame(GameObject player, List<GameObject> pArray) {
        if (loaded) {
            return;
        }

        currentLevel = animationScript.Level1();

        int s = UnityEngine.Random.Range(0, 6);

        switch (s) {
            case 0:
                currentLevel = animationScript.Level1();
                break;
            case 1:
                currentLevel = animationScript.Level2();
                break;
            case 2:
                currentLevel = animationScript.Level3();
                break;
            case 3:
                currentLevel = animationScript.Level4();
                break;
            case 4:
                currentLevel = animationScript.Level5();
                break;
            case 5:
                currentLevel = animationScript.Level6();
                break;
        }
        playerArray = pArray;

        //gameMusic.Stop();
        photonView.RPC("RpcStopSound", RpcTarget.All, gameMusic);
        currentLevel = animationScript.Level1();

        StartCoroutine(currentLevel);
        //taskBarGameObj.SetActive(false);
        collidedPlayer = player;
        previousCamPos = new Vector3(player.GetComponentInChildren<Camera>().transform.position.x,  player.GetComponentInChildren<Camera>().transform.position.y,  player.GetComponentInChildren<Camera>().transform.position.z);
        player.GetComponentInChildren<Camera>().transform.position = new Vector3(-2, 56.1f,  player.GetComponentInChildren<Camera>().transform.position.z);
        player.GetComponentInChildren<CameraFollow>().minigame = true;
        player.GetComponentInChildren<PlayerMovement>().minigame = true;
        for(int i = 0; i < livesObj.transform.childCount; i++) {
           livesObj.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
           livesObj.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
        GameObject p = Instantiate(playerObj);
        playerInstance = p;
        p.tag = "MinigamePlayer";
        playerScript = p.GetComponent<PlayerScript>();
        playerScript.lifePoints = 2;
        p.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Minigame_Player");
        p.GetComponent<TrailRenderer>().sortingLayerID = SortingLayer.NameToID("Minigame_Player");

        p.transform.position = new Vector2(x: 0, y: 55);
        loaded = true;
    }
}
