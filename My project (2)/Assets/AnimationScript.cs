using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

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

    public GameObject Spawn(GameObject shape, Vector2 pos, Color col, Trail t, int rotSpeed, int moveSpeed, bool isWiper, Direction dir, Vector3 scale, string layer, string trailLayer, bool isWarning) {
        GameObject s = Instantiate(shape);
        s.transform.position = pos;
        s.GetComponent<TrailRenderer>().emitting = (t == Trail.HasTrail);
        s.tag = "MinigameShape";
        s.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID(layer);
        s.GetComponent<TrailRenderer>().sortingLayerID = SortingLayer.NameToID(trailLayer);
        s.GetComponent<SpriteRenderer>().color = col;
        s.transform.localScale = scale;
        ShapeScript sScript = s.GetComponent<ShapeScript>();
        sScript.rotationSpeed = rotSpeed;
        sScript.hittable = true;
        sScript.moveSpeed = moveSpeed;
        sScript.isWiper = isWiper;
        sScript.dir = (int)dir;
        return s;
    }

    public GameObject SpawnStationary(GameObject shape, Vector2 pos, Color col, Trail t, int rotSpeed, Vector3 scale, string layer = "Minigame_Shapes", string trailLayer = "Minigame_Shapes_Trail")
    {
        GameObject s = Spawn(shape, pos, col, t, rotSpeed, 0, false, 0, scale, layer, trailLayer, false);
        return s;
    }
    public GameObject SpawnWiper(GameObject shape, Vector2 pos, Color col, Trail t, int rotSpeed, int moveSpeed, Vector3 scale, Direction dir, string layer="Minigame_Shapes", string trailLayer="Minigame_Shapes_Trail") {
        GameObject s = Spawn(shape, pos, col, t, rotSpeed, moveSpeed, true, dir, scale, layer, trailLayer, false);
        return s;
    }

    public void Level1() {
        //Spawn(hexagon, new Vector2(6, 55), new Color(0, 0, 255), Trail.HasTrail, 4, 4, true, Direction.Left, "Minigame_Shapes", "Minigame_Shapes_Trail", false);
        //SpawnStationary(hexagon, new Vector2(0, 55), new Color(0, 0, 255), Trail.HasTrail, -1);
        SpawnWiper(hexagon, new Vector2(6, 55), new Color(0, 0, 255), Trail.HasTrail, 4, 4, new Vector3(1, 1, 1), Direction.Left);
    }
}
