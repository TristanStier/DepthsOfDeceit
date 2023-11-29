using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class LogicScript : MonoBehaviour
{

    private PlayerScript player;
    private SpawnPlayers spawnPlayersScript;
    private ShapeScript shape;
    public AudioSource hit;
    public GameObject playerObj;
    public GameObject shapeObj;
    public GameObject wallsObj;
    public GameObject cameraObj;
    public bool loaded = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MinigamePlayer").GetComponent<PlayerScript>();
        shape = GameObject.FindGameObjectWithTag("MinigameShape").GetComponent<ShapeScript>();
        spawnPlayersScript = GameObject.Find("SpawnPlayers").GetComponent<SpawnPlayers>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !loaded) {
            beginMinigame("Test");
        }
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

    public void beginMinigame(string playerName) {
        loaded = true;
        int currentPlayerIndex = playerName[^1];
        spawnPlayersScript.playerArray[currentPlayerIndex].GetComponentInChildren<Camera>().transform.position = new Vector3(0, 20, cameraObj.transform.position.z);
        spawnPlayersScript.GetComponent<CameraFollow>().minigame = true;
        BoxCollider2D[] s = wallsObj.GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D x in s) {
            x.enabled = true;
        }
        GameObject p = Instantiate(playerObj);
        p.tag = "MinigamePlayer";
        p.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Minigame_Player");

        p.transform.position = new Vector2(x: 0, y: 20);
    }
}
