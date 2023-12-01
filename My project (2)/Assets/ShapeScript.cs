using System.Collections;
using System.Collections.Generic;
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
    
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("MinigameLogic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // Tests
        transform.position += Vector3.left * 5 * Time.deltaTime;
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

    /*class Shape {
        private shapeType shape;
        private bool isEmitting;
        private Color shapeColor;
        private float[2] scale;
        private float[2] coords;


        public Shape(shapeType s=shapeType.Circle, bool isE=false, Color c=new Color(0, 44, 180), float[] sc={1, 1}, float[] cds={0, 0}) {
            shape = s;
            isEmitting = isE;
            shapeColor = c;
            for (int i = 0; i < 2; i++) {
                scale[i] = sc[i];
                coords[i] = cds[i];
            }
        }

        public void draw() {
            Instantiate(shapeObj, new Vector3(coords[0], coords[1]), transform.rotation);
        }

    }*/
}
