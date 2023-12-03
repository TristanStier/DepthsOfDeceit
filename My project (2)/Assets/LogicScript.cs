using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.SlotRacer;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
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
    public bool loaded = false;
    public Color invincibleColor = new Color(0, 200, 255);
    public Color regColor = new Color(255, 255, 255);
    public Vector3 previousCamPos;
    // Start is called before the first frame update
    void Start()
    {
        animationScript = animationObj.GetComponent<AnimationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Tab) && !loaded) {
            beginMinigame();
        }*/
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

    [ContextMenu("Decrease Life")] // add it to Unity
    public void decreaseLife(int num) {
        if (!playerScript.invulnerable && playerScript.lifePoints > 0) {
            hit.Play();
            playerScript.lifePoints -= num;
            decreaseLifeGui(playerScript.lifePoints);
            playerScript.StartCoroutine(playerScript.setInvulnerable());
            Debug.Log("Decreasing life " + playerScript.lifePoints);
        } else if (!playerScript.invulnerable && playerScript.lifePoints <= 0) {
            hit.Play();
            decreaseLifeGui(playerScript.lifePoints);
            endMinigame();
        }
    }

    [ContextMenu("Game Over")] // add it to Unity
    public void endMinigame() {
        collidedPlayer.GetComponentInChildren<Camera>().transform.position = previousCamPos;
        collidedPlayer.GetComponentInChildren<CameraFollow>().minigame = false;
        collidedPlayer.GetComponentInChildren<PlayerMovement>().minigame = false;
        Destroy(playerInstance);
        taskBarGameObj.SetActive(true);
        taskBarObj.value += 1;
        loaded = false;
    }

    public void decreaseLifeGui(int num) {
        livesObj.transform.GetChild(num).GetComponent<SpriteRenderer>().enabled = false;
    }

    public void beginMinigame(GameObject player) {
        if (loaded) {
            return;
        }
        animationScript.level1();
        taskBarGameObj.SetActive(false);
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
