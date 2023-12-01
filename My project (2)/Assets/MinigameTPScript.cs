using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MinigameTPScript : MonoBehaviour
{

    public LogicScript logic;
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("MinigameLogic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (!logic.loaded && collision.gameObject.CompareTag("Player")) {
            logic.beginMinigame(collision.gameObject);
        }
    }
}
