using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    private LogicScript logicScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLevel(int l)
    {
        if (l == 1)
        {
            level1();
        } 
        else if(l == 2)
        {
            level2();
        } 
        else if (l == 3)
        {
            level3();
        } 
        else if (l == 4)
        {
            level3();
        }
    }

    private void level1()
    {

    }

    private void level2()
    {

    }

    private void level3()
    {

    }

    private void level4()
    {

    }
}
