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

    public GameObject Spawn(GameObject shape, Vector2 pos, Color col, Trail t, int rotSpeed, int moveSpeed, bool isWiper, int dur, Direction dir, Vector3 scale, string layer, string trailLayer, bool isWarning) {
        GameObject s = Instantiate(shape);
        s.transform.position = pos;
        s.GetComponent<TrailRenderer>().emitting = (t == Trail.HasTrail);
        s.tag = "MinigameShape";
        s.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID(layer);
        s.GetComponent<TrailRenderer>().sortingLayerID = SortingLayer.NameToID(trailLayer);
        SpriteRenderer sSprite = s.GetComponent<SpriteRenderer>();
        //sSprite.color = col;
        s.transform.localScale = scale;
        ShapeScript sScript = s.GetComponent<ShapeScript>();
        sScript.rotationSpeed = rotSpeed;
        sScript.moveSpeed = moveSpeed;
        sScript.isWiper = isWiper;
        sScript.dir = (int)dir;
        if (isWarning) {
            sScript.hittable = false;
            col.a = .1f;
            sSprite.color = col;
            //sSprite.material.color.a = 1;
            StartCoroutine(SpawnWarning(sScript, sSprite, col));
        } else {
            sScript.hittable = true;
        }
        if (!isWiper) {
            StartCoroutine(DestroyAfterDuration(s, dur));
        }
        return s;
    }

    private IEnumerator SpawnWarning(ShapeScript sScript, SpriteRenderer sSprite, Color col) {
        yield return new WaitForSeconds(1);
        sScript.hittable = true;
        col.a = 1;
        sSprite.color = col;
    }
    private IEnumerator DestroyAfterDuration(GameObject s, int time) {
        yield return new WaitForSeconds(time+1);
        Destroy(s);
    }

    public GameObject SpawnStationary(GameObject shape, Vector2 pos, Color col, int rotSpeed, int dur, Vector3 scale, string layer = "Minigame_Shapes", string trailLayer = "Minigame_Shapes_Trail") {
        pos.x = Mathf.Lerp(-10.5f, 6.5f, pos.x);
        pos.y = Mathf.Lerp(51.5f, 60.5f, pos.y);
        GameObject s = Spawn(shape, pos, col, Trail.NoTrail, rotSpeed, 0, false, dur , 0, scale, layer, trailLayer, true);
        return s;
    }
    public GameObject SpawnWiper(GameObject shape, float pos, Color col, Trail t, int rotSpeed, int moveSpeed, Vector3 scale, Direction dir, string layer="Minigame_Shapes", string trailLayer="Minigame_Shapes_Trail") {
        Vector2 fullPos;
        if (dir == Direction.Left) {
            pos = Mathf.Lerp(51.5f, 60.5f, pos);
            fullPos = new Vector2(8.5f, pos);
        } else if (dir == Direction.Right) {
            pos = Mathf.Lerp(51.5f, 60.5f, pos);
            fullPos = new Vector2(-12.5f, pos);
        } else if (dir == Direction.Down) {
            pos = Mathf.Lerp(-10.5f, 6.5f, pos);
            fullPos = new Vector2(pos, 62.5f);
        } else {
            pos = Mathf.Lerp(-10.5f, 6.5f, pos);
            fullPos = new Vector2(pos, 49.5f);
        }
        GameObject s = Spawn(shape, fullPos, col, t, rotSpeed, moveSpeed, true, 0, dir, scale, layer, trailLayer, false);
        return s;
    }

    public void Level1() {
        //Spawn(hexagon, new Vector2(6, 55), new Color(0, 0, 255), Trail.HasTrail, 4, 4, true, Direction.Left, "Minigame_Shapes", "Minigame_Shapes_Trail", false);
        SpawnStationary(triangle, new Vector2(.8f, .7f), new Color(0, 0, 255), -4, 3, new Vector3(1, 1, 1));
        SpawnWiper(hexagon, .2f, new Color(0, 0, 255), Trail.HasTrail, 4, 4, new Vector3(1, 1, 1), Direction.Right);
    }
}
