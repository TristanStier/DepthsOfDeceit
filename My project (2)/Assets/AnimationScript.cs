using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;

public class AnimationScript : MonoBehaviour
{
    public float tempo; //seconds per beat
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
    public GameObject logic;
    public LogicScript lScript;
    public AudioSource lv1Music;
    public AudioSource lv2Music;
    public AudioSource lv3Music;

    void Start() {
        lScript = logic.GetComponent<LogicScript>();
    }

    public GameObject Spawn(GameObject shape, Vector2 pos, Color col, Trail t, int rotSpeed, int moveSpeed, bool isWiper, float dur, Direction dir, Vector3 scale, float alpha, bool hittable, float startRot, string layer, string trailLayer, bool isWarning) {
        GameObject s = Instantiate(shape);
        s.transform.position = pos;
        s.transform.eulerAngles = new Vector3(0, 0, startRot);
        s.GetComponent<TrailRenderer>().emitting = (t == Trail.HasTrail);
        s.tag = "MinigameShape";
        s.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID(layer);
        s.GetComponent<TrailRenderer>().sortingLayerID = SortingLayer.NameToID(trailLayer);
        SpriteRenderer sSprite = s.GetComponent<SpriteRenderer>();
        col.a = alpha;
        sSprite.color = col;
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
            sScript.hittable = hittable;
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
    private IEnumerator DestroyAfterDuration(GameObject s, float time) {
        yield return new WaitForSeconds(time+1);
        Destroy(s);
    }

    public GameObject SpawnStationary(GameObject shape, Vector2 pos, Color col, int rotSpeed, float dur, Vector3 scale, float startRot=0f, float alpha=1f, bool hittable=true, string layer = "Minigame_Shapes", string trailLayer = "Minigame_Shapes_Trail", bool warning=true) {
        pos.x = Mathf.Lerp(-10.5f, 6.5f, pos.x);
        pos.y = Mathf.Lerp(51.5f, 60.5f, pos.y);
        GameObject s = Spawn(shape, pos, col, Trail.NoTrail, rotSpeed, 0, false, dur , 0, scale, alpha, hittable, startRot, layer, trailLayer, warning);
        return s;
    }
    public GameObject SpawnWiper(GameObject shape, float pos, Color col, Trail t, int rotSpeed, int moveSpeed, Vector3 scale, Direction dir, float startRot=0f, float alpha=1f, bool hittable=true, string layer="Minigame_Shapes", string trailLayer="Minigame_Shapes_Trail") {
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
        GameObject s = Spawn(shape, fullPos, col, t, rotSpeed, moveSpeed, true, 0, dir, scale, alpha, hittable, startRot, layer, trailLayer, false);
        return s;
    }

    public IEnumerator Level1() {
        yield return new WaitForSeconds(1);
        lv1Music.Play();
        GameObject s = SpawnStationary(triangle, new Vector2(.8f, .7f), new Color(0, 0, 255), 0, 3, new Vector3(1, 1, 1), 270);
        yield return new WaitForSeconds(2);
        SpawnWiper(hexagon, .2f, new Color(0, 0, 255), Trail.HasTrail, 4, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(3);
        //SpawnStationary(square, new Vector2(.5f, .5f), new Color(0, 0, 255), 1, 6, new Vector3(.5f, 30, 1));
        SpawnWiper(hexagon, .2f, new Color(0, 0, 255), Trail.HasTrail, 4, 4, new Vector3(1, 1, 1), Direction.Right);
        SpawnWiper(hexagon, .4f, new Color(0, 0, 255), Trail.HasTrail, 4, 4, new Vector3(1, 1, 1), Direction.Right);
        SpawnWiper(hexagon, .8f, new Color(0, 0, 255), Trail.HasTrail, 4, 4, new Vector3(1, 1, 1), Direction.Right);

        // Background shapes
        SpawnStationary(square, new Vector2(.5f, .5f), new Color(0, 0, 255), 1, 11, new Vector3(.5f, 30, 1), startRot: 0, alpha: .01f, hittable: false, layer: "Minigame_Background_Shapes", warning: false);
        SpawnStationary(square, new Vector2(.5f, .5f), new Color(0, 0, 255), 1, 11, new Vector3(.5f, 30, 1), startRot: 90, alpha: .01f, hittable: false, layer: "Minigame_Background_Shapes", warning: false);
        SpawnStationary(square, new Vector2(.5f, .5f), new Color(0, 0, 255), 1, 11, new Vector3(.5f, 30, 1), startRot: 45, alpha: .01f, hittable: false, layer: "Minigame_Background_Shapes", warning: false);
        SpawnStationary(square, new Vector2(.5f, .5f), new Color(0, 0, 255), 1, 11, new Vector3(.5f, 30, 1), startRot: 135, alpha: .01f, hittable: false, layer: "Minigame_Background_Shapes", warning: false);

        for (int i = 0; i < 30; i++) {
            yield return new WaitForSeconds(.5f);
            SpawnStationary(triangle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), -4, 2, new Vector3(1, 1, 1));
        }
        yield return new WaitForSeconds(5);

        // MAKE SURE THAT ALL SHAPES ARE DESTROYED BEFORE ENDING THE MINIGAME!!!
        lv1Music.Stop();
        lScript.endMinigame();
    }

    public IEnumerator Level2() //Tetris 99 theme
    {
        tempo = 0.428571428571f;
        yield return new WaitForSeconds(1);
        lv2Music.Play();
        for (int j = 0; j < 2; j++) {
            for (int i = 0; i < 14; i++)
            {
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
                yield return new WaitForSeconds(tempo);
            }

            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 2);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 2);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 2);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 2);
        }

        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
                yield return new WaitForSeconds(tempo);
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
                yield return new WaitForSeconds(tempo/2);
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
                yield return new WaitForSeconds(tempo/2);
            }

            for (int i = 0; i < 4; i++)
            {
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
                yield return new WaitForSeconds(tempo);
            }

            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 2);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 2);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 2);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 2);
        }

        yield return new WaitForSeconds(tempo);
        for (int i = 0; i < 3; i++)
        {
            SpawnWiper(square, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.NoTrail, 0, 8, new Vector3(10.47177f, -0.0464595f, 1), Direction.Right);
            yield return new WaitForSeconds(tempo*2);
            SpawnWiper(square, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.NoTrail, 0, 8, new Vector3(-0.0464595f, 10.47177f, 1), Direction.Down);
            yield return new WaitForSeconds(tempo * 2);
            SpawnWiper(square, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.NoTrail, 0, 8, new Vector3(10.47177f, -0.0464595f, 1), Direction.Left);
            yield return new WaitForSeconds(tempo * 2);
            SpawnWiper(square, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.NoTrail, 0, 8, new Vector3(-0.0464595f, 10.47177f, 1), Direction.Up);
            yield return new WaitForSeconds(tempo * 2);
        }

        SpawnWiper(square, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.NoTrail, 0, 8, new Vector3(10.47177f, -0.0464595f, 1), Direction.Right);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(square, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.NoTrail, 0, 8, new Vector3(-0.0464595f, 10.47177f, 1), Direction.Down);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(square, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.NoTrail, 0, 8, new Vector3(-0.0464595f, 10.47177f, 1), Direction.Up);
        yield return new WaitForSeconds(tempo * 2);
        SpawnWiper(square, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.NoTrail, 0, 8, new Vector3(- 0.0464595f, 10.47177f, 1), Direction.Down);
        yield return new WaitForSeconds(tempo * 4);

        lv2Music.Stop();
        lScript.endMinigame();
    }

    public IEnumerator Level3() //Dive Into the Void
    {
        tempo = 0.521739130435f;
        yield return new WaitForSeconds(1);
        lv3Music.Play();
        
        for (int i = 0; i < 12; i++)
        {
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(10.47177f, -0.0464595f, 1));
            yield return new WaitForSeconds(tempo);
        }
    
    }


}
