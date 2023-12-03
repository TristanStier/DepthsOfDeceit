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
    public bool hittable = true;
    public GameObject shapeObj;
    public Sprite sp;
    public int rotationSpeed;
    public int moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("MinigameLogic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime * 100);
        transform.position += moveSpeed * Time.deltaTime * Vector3.left;
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("MinigamePlayer") && hittable) { // check if it was the player
            logic.decreaseLife(1);
        }
    }
}
