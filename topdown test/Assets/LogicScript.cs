using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicScript : MonoBehaviour
{

    private PlayerScript player;
    public AudioSource hit;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
