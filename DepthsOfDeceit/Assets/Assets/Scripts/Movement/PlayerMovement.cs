using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private Rigidbody2D rb;
    public bool canMove = true;

    // Update is called once per frame
    void Update()
    {
         // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(canMove == true)
        {
            rb.velocity = new UnityEngine.Vector2(horizontalInput * Speed, verticalInput * Speed);
        }

        // Flipping Character
        if(horizontalInput > 0.01f && canMove == true)
        {
            transform.localScale = new UnityEngine.Vector3(1, 1, 1);
        }
        else if(horizontalInput < -0.01f  && canMove == true)
        {
            transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
        }

        print(horizontalInput);
    }
}
