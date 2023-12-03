using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShapeScript : MonoBehaviour
{
    enum shapeType {
        Circle = 0,
        Square,
        RoundedSquare,
        Capsule,
        Triangle,
        Hexagon,
        Diamond
    }

    private LogicScript logic;
    public AnimationScript animScript;
    public bool hittable = true;
    public GameObject shapeObj;
    public Sprite sp;
    public int rotationSpeed;
    public int moveSpeed;
    public float dzXleft = -15f, dzXright = 12f;
    public float dzYup = 67f, dzYdown = 44f;
    public bool isWiper;
    public int dir;
    private Vector3 vecDir;
    
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("MinigameLogic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * 100);
        
        if (isWiper) {
            switch (dir) {
                case 0:
                    vecDir = Vector3.left;
                    break;
                case 1:
                    vecDir = Vector3.right;
                    break;
                case 2:
                    vecDir = Vector3.up;
                    break;
                case 3:
                    vecDir = Vector3.down;
                    break;
                default:
                    Debug.Log("Invalid shape direction");
                    break;
            }
            transform.position += moveSpeed * Time.deltaTime * vecDir;
            if (transform.position.x <= dzXleft || transform.position.x >= dzXright || transform.position.y <= dzYdown || transform.position.y >= dzYup) {
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("MinigamePlayer") && hittable) { // check if it was the player
            logic.decreaseLife(1);
        }
    }
}
