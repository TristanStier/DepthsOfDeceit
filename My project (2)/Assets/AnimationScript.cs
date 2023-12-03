using System.Collections;
using System.Collections.Generic;
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

    public GameObject Spawn(GameObject shape, Vector2 pos, Color col, Trail t, float rotSpeed) {
        GameObject s = Instantiate(shape);
        s.transform.position = pos;
        s.GetComponent<TrailRenderer>().emitting = (t == Trail.HasTrail);
        s.tag = "MinigameShape";
        s.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Minigame_Shape");
        s.GetComponent<TrailRenderer>().sortingLayerID = SortingLayer.NameToID("Minigame_Shape");
        s.GetComponent<SpriteRenderer>().color = col;
        s.transform.Rotate(rotSpeed * Time.deltaTime * Vector3.forward);
        return s;
    }
}
