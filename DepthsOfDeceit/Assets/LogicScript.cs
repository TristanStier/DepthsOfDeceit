using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.SlotRacer;
using Unity.Collections;
using UnityEngine;

public class LogicScript : MonoBehaviour
{

    private PlayerScript player;
    private ShapeScript shape;
    public AudioSource hit;
    public GameObject playerObj;
    public GameObject shapeObj;
    public GameObject wallsObj;
    public GameObject backgroundObj;
    public bool loaded = false;
    public Vector3 previousCamPos;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MinigamePlayer").GetComponent<PlayerScript>();
        shape = GameObject.FindGameObjectWithTag("MinigameShape").GetComponent<ShapeScript>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Tab) && !loaded) {
            beginMinigame();
        }*/
    }

    [ContextMenu("Decrease Life")] // add it to Unity
    public void decreaseLife(int num) {
        if (!player.invulnerable && player.lifePoints > 0) {
            hit.Play();
            player.lifePoints -= num;
            player.StartCoroutine(player.setInvulnerable());
            Debug.Log("Decreasing life " + player.lifePoints);
        } else if (!player.invulnerable && player.lifePoints <= 0) {
            hit.Play();
            gameOver();
        }
    }

    [ContextMenu("Game Over")] // add it to Unity
    public void gameOver() {
        Debug.Log("Game Over!");
    }

    public void beginMinigame(GameObject player) {
        loaded = true;
        previousCamPos = new Vector3(player.GetComponentInChildren<Camera>().transform.position.x,  player.GetComponentInChildren<Camera>().transform.position.y,  player.GetComponentInChildren<Camera>().transform.position.z);
        player.GetComponentInChildren<Camera>().transform.position = new Vector3(0, 20,  player.GetComponentInChildren<Camera>().transform.position.z);
        player.GetComponentInChildren<CameraFollow>().minigame = true;
        player.GetComponentInChildren<PlayerMovement>().minigame = true;
        BoxCollider2D[] s = wallsObj.GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D x in s) {
            x.enabled = true;
        }
        backgroundObj.GetComponent<SpriteRenderer>().enabled = true;
        GameObject p = Instantiate(playerObj);
        p.tag = "MinigamePlayer";
        p.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Minigame_Player");

        p.transform.position = new Vector2(x: 0, y: 20);
    }
}
