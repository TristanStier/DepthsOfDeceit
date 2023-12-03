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

    public enum Trail {
        HasTrail,
        NoTrail
    }

    public enum Warning {
        DisplayWarning,
        NoWarning
    }

    private enum Direction {
        Right,
        Left,
        Up,
        Down
    }
    
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("MinigameLogic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // Tests
        //transform.position += Vector3.left * 5 * Time.deltaTime;
        //transform.Rotate(0, 0, -3);
        

        /*if (Input.GetKeyDown(KeyCode.Space)) {
            spawn();
        }*/
    }

    public void OnTriggerEnter2DChild(Collider2D collision) {
        if (collision.gameObject.CompareTag("MinigamePlayer") && hittable) { // check if it was the player
            logic.decreaseLife(1);
        }
    }

    /*public void spawn() {
        GameObject s = Instantiate(shapeObj, new Vector3(2f, 2f, 0f), transform.rotation);
        s.GetComponent<SpriteRenderer>().sprite = sp;
    }*/

    /*public GameObject Spawn(GameObject shape, Vector2 pos, Color col, Trail t, float rotSpeed) {
        GameObject s = Instantiate(shape);
        s.transform.position = pos;
        s.GetComponent<TrailRenderer>().emitting = (t == Trail.HasTrail);
        s.tag = "MinigameShape";
        s.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Minigame_Shape");
        s.GetComponent<TrailRenderer>().sortingLayerID = SortingLayer.NameToID("Minigame_Shape");
        s.GetComponent<SpriteRenderer>().color = col;
        s.transform.Rotate(rotSpeed * Time.deltaTime * Vector3.forward);
        return s;
    }*/

    /*public GameObject SpawnStationary(GameObject shape, Direction d, Warning w, Vector2 pos, Color col, Trail t) {

    }

    public GameObject SpawnWiper() {
        
    }*/
}
