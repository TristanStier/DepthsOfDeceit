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

    private enum Direction {
        Right,
        Left,
        Up,
        Down
    }

    public GameObject circle, square, triangle, hexagon, rhombus, capsule, roundedSquare;
    public float dzXleft = -15f, dzXright = 12f;
    public float dzYup = 67f, dzYdown = 44f;

    public GameObject Spawn(GameObject shape, Vector2 pos, Color col, Trail t, int rotSpeed, int moveSpeed) {
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
        return s;
    }

    public void level1() {
        Spawn(square, new Vector2(10, 55), new Color(0, 0, 255), Trail.HasTrail, 2, 3);
    }
}
