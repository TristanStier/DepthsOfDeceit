using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<GameObject> shapesArray = new();
    public AudioClip lv1Music;
    public AudioClip lv2Music;
    public AudioClip lv3Music;
    public AudioClip lv4Music;
    public AudioClip lv5Music;
    public AudioClip lv6Music;
    public AudioSource music;
    public GameObject musicObj;

    void Start() {
        lScript = logic.GetComponent<LogicScript>();
        shapesArray.Add(circle);
        shapesArray.Add(square);
        shapesArray.Add(triangle);
        shapesArray.Add(hexagon);
        shapesArray.Add(rhombus);
        shapesArray.Add(capsule);
        shapesArray.Add(roundedSquare);
        music = musicObj.GetComponent<AudioSource>();
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

    /*public IEnumerator LevelTest() {
        yield return new WaitForSeconds(1);
        lScript.currentMusic = lv1Music;
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
        lScript.endMinigame(true);
    }*/

     public IEnumerator Level1() {
        yield return new WaitForSeconds(1);
        //lScript.currentMusic = lv1Music;
        music.clip = lv1Music;
        music.Play();
        //lScript.RpcPlaySound(lv1Music);
        for (int i = 0; i < 29; i++) {
            yield return new WaitForSeconds(.3f);
            int s = Random.Range(-3, 3);
            while (s == 0) {
                s = Random.Range(-3, 3);
            }
            SpawnStationary(triangle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), s, 2, new Vector3(1, 1, 1));
        }
        for (int i = 0; i < 33; i++) {
            yield return new WaitForSeconds(.3f);
            int s = Random.Range(-3, 3);
            while (s == 0) {
                s = Random.Range(-3, 3);
            }
            SpawnStationary(hexagon, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), s, 2, new Vector3(1, 1, 1));
        }
        SpawnStationary(square, new Vector2(.5f, .5f), new Color(0, 0, 255), 1, 21, new Vector3(.5f, 30, 1), startRot: 0, alpha: .01f, hittable: false, layer: "Minigame_Background_Shapes", warning: false);
        SpawnStationary(square, new Vector2(.5f, .5f), new Color(0, 0, 255), 1, 21, new Vector3(.5f, 30, 1), startRot: 90, alpha: .01f, hittable: false, layer: "Minigame_Background_Shapes", warning: false);
        SpawnStationary(square, new Vector2(.5f, .5f), new Color(0, 0, 255), 1, 21, new Vector3(.5f, 30, 1), startRot: 45, alpha: .01f, hittable: false, layer: "Minigame_Background_Shapes", warning: false);
        SpawnStationary(square, new Vector2(.5f, .5f), new Color(0, 0, 255), 1, 21, new Vector3(.5f, 30, 1), startRot: 135, alpha: .01f, hittable: false, layer: "Minigame_Background_Shapes", warning: false);
        for (int i = 0; i < 9; i++) {
            float s = Random.Range(1, 4);
            yield return new WaitForSeconds(1.2f);
            SpawnStationary(circle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, .8f, new Vector3(s, s, 0));
            //yield return new WaitForSeconds(1f);
        }
        SpawnWiper(hexagon, .3f, new Color(0, 0, 255), Trail.HasTrail, 4, 3, new Vector3(1, 1, 1), Direction.Right);
        SpawnWiper(hexagon, .6f, new Color(0, 0, 255), Trail.HasTrail, 4, 4, new Vector3(1, 1, 1), Direction.Right);
        SpawnWiper(hexagon, .9f, new Color(0, 0, 255), Trail.HasTrail, 4, 3, new Vector3(1, 1, 1), Direction.Right);
        for (int i = 0; i < 6; i++) {
            float s = Random.Range(5, 8);
            yield return new WaitForSeconds(1.2f);
            SpawnStationary(circle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, .8f, new Vector3(s, s, 0));
            //yield return new WaitForSeconds(1f);
        }
        SpawnWiper(square, .8f, new Color(0, 0, .4f), Trail.NoTrail, 1, 15, new Vector3(6, 6, 1), Direction.Right, layer: "Minigame_Foreground_Shapes", hittable: false);
        SpawnWiper(circle, .2f, new Color(0, 0, .4f), Trail.NoTrail, 1, 15, new Vector3(6, 6, 1), Direction.Left, layer: "Minigame_Foreground_Shapes", hittable: false);
        yield return new WaitForSeconds(2f);
        SpawnWiper(triangle, .5f, new Color(0, 0, 1), Trail.NoTrail, 2, 12, new Vector3(5, 5, 1), Direction.Up);
        yield return new WaitForSeconds(2f);

        //lScript.RpcStopSound(lv1Music);
        music.Stop();
        lScript.endMinigame(true);
    }

    public IEnumerator Level2() //Tetris 99 theme
    {
        tempo = 0.428571428571f;
        yield return new WaitForSeconds(1);
        music.clip = lv2Music;
        music.Play();
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

        music.Stop();
        lScript.endMinigame(true);
    }

    public IEnumerator Level3() //Dive Into the Void
    {
        tempo = 0.521739130435f;

        yield return new WaitForSeconds(tempo*6);

        yield return new WaitForSeconds(1);
        music.clip = lv3Music;
        music.Play();

        

        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo);
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo);
        music.Play();

        for (int i = 0; i < 6; i++)
        {
            SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
            yield return new WaitForSeconds(tempo);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo);
        }

        for (int i = 0; i < 3; i++)
        {
            SpawnStationary(circle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo/4, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo/4);
        }

        SpawnStationary(circle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo/2, new Vector3(1, 1, 1));
        yield return new WaitForSeconds(tempo/2);

        for (int i = 0; i < 3; i++)
        {
            SpawnStationary(circle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 4, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 4);
        }

        for (int i = 0; i < 4; i++)
        {
            SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
            yield return new WaitForSeconds(tempo);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo);
        }
        
        for (int i = 0; i < 2; i++) {

            for (int j = 0; j < 2; j++)
            {
                yield return new WaitForSeconds(tempo / 4);
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
                yield return new WaitForSeconds(tempo / 4);
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
                yield return new WaitForSeconds(tempo / 4);
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
                yield return new WaitForSeconds(tempo / 4);
            }

            for (int j = 0; j < 4; j++)
            {
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 2, new Vector3(-0.2464595f, 50.47177f, 1));
                yield return new WaitForSeconds(tempo / 2);
            }
        }

        //second melody
        for (int i = 0; i < 4; i++)
        {
            SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.4464595f, 1));
            yield return new WaitForSeconds(tempo);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo, new Vector3(-0.4464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo);
        }

        for (int i = 0; i < 2; i++)
        {

            for (int j = 0; j < 2; j++)
            {
                yield return new WaitForSeconds(tempo / 4);
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
                yield return new WaitForSeconds(tempo / 4);
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
                yield return new WaitForSeconds(tempo / 4);
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
                yield return new WaitForSeconds(tempo / 4);
            }

            for (int j = 0; j < 4; j++)
            {
                SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 2, new Vector3(-0.2464595f, 50.47177f, 1));
                yield return new WaitForSeconds(tempo / 2);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.4464595f, 1));
            yield return new WaitForSeconds(tempo);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo, new Vector3(-0.4464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo);
        }

        for (int j = 0; j < 2; j++)
        {
            yield return new WaitForSeconds(tempo / 4);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 4);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 4);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 4);
        }

        for (int j = 0; j < 4; j++)
        {
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 2, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 2);
        }

        for (int j = 0; j < 2; j++)
        {
            yield return new WaitForSeconds(tempo / 4);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 4);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 4);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 4);
        }

        yield return new WaitForSeconds(tempo * 4);

        music.Stop();
        lScript.endMinigame(true);

    }

    public IEnumerator Level4() //can can
    {
        tempo = 0.365853658537f;
        music.clip = lv4Music;



        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo);
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo);
        music.Play();

        for (int j = 0; j < 2; j++)
        {
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo);
        }

        for (int i = 0; i < 20; i++)
        {
            SpawnStationary(triangle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo/2);
        }

        for (int j = 0; j < 7; j++)
        {
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), .5f), new Color(0, 0, 255), 0, tempo * 3 / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo*3/4);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), .5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 4);
        }

        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo);

        //CAN CAN CHORUS!!!
        for (int j = 0; j < 2; j++)
        {
            //phrase 1
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo*2, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo * 2);

            for (int k = 0; k < 2; k++) {
                for (int i = 0; i < 4; i++)
                {
                    SpawnStationary(triangle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
                    yield return new WaitForSeconds(tempo / 2);
                }

                for (int i = 0; i < 2; i++)
                {
                    SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
                    yield return new WaitForSeconds(tempo);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo/2, new Vector3(50.47177f, -0.2464595f, 1));
                yield return new WaitForSeconds(tempo/2);
            }

            for (int i = 0; i < 8; i++)
            {
                SpawnStationary(triangle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
                yield return new WaitForSeconds(tempo / 2);
            }

            //phrase 2
            yield return new WaitForSeconds(tempo);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo * 2, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo*2);

            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    SpawnStationary(rhombus, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
                    yield return new WaitForSeconds(tempo / 2);
                }

                for (int i = 0; i < 2; i++)
                {
                    SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
                    yield return new WaitForSeconds(tempo);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(50.47177f, -0.2464595f, 1));
                yield return new WaitForSeconds(tempo / 2);
            }

            for (int i = 0; i < 2; i++)
            {
                SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
                yield return new WaitForSeconds(tempo);
            }
        }

        yield return new WaitForSeconds(tempo*4);

        music.Stop();
        lScript.endMinigame(true);
    }

    public IEnumerator Level5() //Hail to the Chief
    {
        tempo = 0.582524271845f;
        music.clip = lv5Music;
        music.Play();

        //phrase 1
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo);

        //phrase 2
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 2);

        //phrase 3
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
        yield return new WaitForSeconds(tempo);

        //phrase 4
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 2);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo * 3 / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo / 4);
        SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
        yield return new WaitForSeconds(tempo);

        //new melody, phrase 1
        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo); //s
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo * 3 / 4, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo * 3 / 4); //u
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo / 4);  //u
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo);  //u
        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo * 3 / 4, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo * 3 / 4); //s
        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 4, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo / 4); //s
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo * 3 / 4, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo * 3 / 4); //u
        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 4, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo / 4); //s
        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo / 2); //s
        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo / 2); //s
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 2, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo / 2); //u
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 2, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo / 2);  //u
        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo); //s

        //phrase 2 and 3.5
        for (int i = 0; i < 3; i++) {
            SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
            yield return new WaitForSeconds(tempo); //s
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo * 3 / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo * 3 / 4); //u
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 4);  //u
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 2, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 2); //u
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 2, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo / 2);  //u
            SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(50.47177f, -0.2464595f, 1));
            yield return new WaitForSeconds(tempo / 2); //s
            SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(50.47177f, -0.2464595f, 1));
            yield return new WaitForSeconds(tempo / 2); //s
        }

        //phrase 3.5 to 4
        for (int i = 0; i < 3; i++)
        {
            SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
            yield return new WaitForSeconds(tempo);
            SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
            yield return new WaitForSeconds(tempo * 3 / 4);
            SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Left);
            yield return new WaitForSeconds(tempo / 4);
        }

        yield return new WaitForSeconds(tempo*16);

        music.Stop();
        lScript.endMinigame(true);
    }

    public IEnumerator Level6()
    {
        tempo = 0.348837209302f;
        music.clip = lv6Music;

        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo);
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo);
        music.Play();

        //hardstyle part
        for (int i = 0; i < 6; i++)
        {
            SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(50.47177f, -0.2464595f, 1));
            yield return new WaitForSeconds(tempo);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo, new Vector3(-0.2464595f, 50.47177f, 1));
            yield return new WaitForSeconds(tempo);
        }

        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo/2, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo/2);
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo*3/8, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo*3/8);
        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo/8, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo/8);
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo/2, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo/2);
        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo/2, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo/2);

        for (int i = 0; i < 2; i++)
        {
            SpawnStationary(circle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 2);
        }

        for (int i = 0; i < 7; i++)
        {
            SpawnStationary(circle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo);
        }

        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo / 4);
        SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(50.47177f, -0.2464595f, 1));
        yield return new WaitForSeconds(tempo / 2);
        SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo / 4, new Vector3(-0.2464595f, 50.47177f, 1));
        yield return new WaitForSeconds(tempo / 4);

        for (int i = 0; i < 4; i++)
        {
            SpawnStationary(circle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 2, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 2);
        }

        for (int i = 0; i < 4; i++)
        {
            SpawnStationary(circle, new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo / 4, new Vector3(1, 1, 1));
            yield return new WaitForSeconds(tempo / 4);
        }

        yield return new WaitForSeconds(tempo*3);
        //upbeat
        
        yield return new WaitForSeconds(tempo);

        //chorus part 1
        for (int i = 0; i < 8; i++)
        {
            SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo * 3, new Vector3(50.47177f, -0.2464595f, 1));
            SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
            yield return new WaitForSeconds(tempo * 3);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo, new Vector3(-0.2464595f, 50.47177f, 1));
            SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 8, new Vector3(1, 1, 1), Direction.Left);
            yield return new WaitForSeconds(tempo);
        }

        //chorus part 2
        for (int i = 0; i < 8; i++)
        {
            SpawnStationary(square, new Vector2(0.5f, Random.Range(0f, 1f)), new Color(0, 0, 255), 0, tempo * 3, new Vector3(50.47177f, -0.2464595f, 1));
            SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
            yield return new WaitForSeconds(tempo);
            SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
            yield return new WaitForSeconds(tempo);
            SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 4, new Vector3(1, 1, 1), Direction.Right);
            yield return new WaitForSeconds(tempo);
            SpawnStationary(square, new Vector2(Random.Range(0f, 1f), 0.5f), new Color(0, 0, 255), 0, tempo, new Vector3(-0.2464595f, 50.47177f, 1));
            SpawnWiper(hexagon, Random.Range(0f, 1f), new Color(0, 0, 255), Trail.HasTrail, 0, 8, new Vector3(1, 1, 1), Direction.Left);
            yield return new WaitForSeconds(tempo);
        }

        yield return new WaitForSeconds(tempo * 8);

        music.Stop();
        lScript.endMinigame(true);
    }

}
