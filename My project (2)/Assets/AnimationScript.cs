using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public enum Trail {
        HasTrail,
        NoTrail
    }

    public enum Warning {
        DisplayWarning,
        NoWarning
    }

    public enum Direction {
        Left,
        Right,
        Up,
        Down
    }

    public GameObject circle, square, triangle, hexagon, rhombus, capsule, roundedSquare;

    public GameObject Spawn(GameObject shape, Vector2 pos, Color col, Trail t, int rotSpeed, int moveSpeed, bool isWiper, Direction dir) {
        GameObject s = Instantiate(shape);
        s.transform.position = pos;
        s.GetComponent<TrailRenderer>().emitting = (t == Trail.HasTrail);
        s.tag = "MinigameShape";
        s.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Minigame_Shapes");
        s.GetComponent<TrailRenderer>().sortingLayerID = SortingLayer.NameToID("Minigame_Shapes_Trail");
        s.GetComponent<SpriteRenderer>().color = col;
        ShapeScript sScript = s.GetComponent<ShapeScript>();
        sScript.rotationSpeed = rotSpeed;
        sScript.hittable = true;
        sScript.moveSpeed = moveSpeed;
        sScript.isWiper = isWiper;
        sScript.dir = (int)dir;
        return s;
    }

    public void level1() {
        Spawn(square, new Vector2(10, 55), new Color(0, 0, 255), Trail.HasTrail, 2, 3, true, Direction.Left);
    }
}
