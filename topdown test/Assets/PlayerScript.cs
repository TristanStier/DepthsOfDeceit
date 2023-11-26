using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerScript : MonoBehaviour
{

    public float moveSpeed = 5f;
    private bool facingRight = true;
    private bool facingUp = true;
    private bool canDash = true;
    private bool isDashing;
    public float dashPower = 10f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    private Direction[] d = {Direction.Right, Direction.None}; 
    
    public Sprite spriteLeft;
    public Sprite spriteRight;
    public Rigidbody2D rb;
    Vector2 movement;
    [SerializeField] private TrailRenderer tr;

    private enum Direction {
        None,
        Right,
        Left,
        Up,
        Down
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) {
            return;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x < 0 && movement.y == 0) {
            d[0] = Direction.Left;
            d[1] = Direction.None;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteLeft;
        } else if (movement.x > 0 && movement.y == 0) {
            d[0] = Direction.Right;
            d[1] = Direction.None;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteRight;
        } else if (movement.y < 0 && movement.x == 0) {
            d[0] = Direction.None;
            d[1] = Direction.Up;
        } else if (movement.y > 0 && movement.x == 0) {
            d[0] = Direction.None;
            d[1] = Direction.Down;
        } else if (movement.y < 0 && movement.x < 0) {
            d[0] = Direction.Left;
            d[1] = Direction.Up;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteLeft;
        } else if (movement.y < 0 && movement.x > 0) {
            d[0] = Direction.Right;
            d[1] = Direction.Up;
             gameObject.GetComponent<SpriteRenderer>().sprite = spriteRight;
        } else if (movement.y > 0 && movement.x < 0) {
            d[0] = Direction.Left;
            d[1] = Direction.Down;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteLeft;
        } else if (movement.y > 0 && movement.x > 0) {
            d[0] = Direction.Right;
            d[1] = Direction.Down;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteRight;
        }

        /*if (transform.position.y < bottom || transform.position.y > top || transform.position.x < left || transform.position.x > right) {
            canMove = false;
        }*/
        if (Input.GetKeyDown(KeyCode.Space) && canDash) {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate() {
        if (isDashing) {
            return;
        }
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement.normalized);
    }

    private IEnumerator Dash() {
        canDash = false;
        isDashing = true;
        if (d[0] == Direction.Right && d[1] == Direction.None) {
            rb.velocity = new Vector2(dashPower, 0f);
        } else if (d[0] == Direction.Left && d[1] == Direction.None) {
            rb.velocity = new Vector2(-dashPower, 0f);
        } else if (d[0] == Direction.None && d[1] == Direction.Up) {
            rb.velocity = new Vector2(0f, -dashPower);
        } else if (d[0] == Direction.None && d[1] == Direction.Down) {
            rb.velocity = new Vector2(0f, dashPower);
        } else if (d[0] == Direction.Right && d[1] == Direction.Up) {
            rb.velocity = new Vector2(dashPower, -dashPower);
        } else if (d[0] == Direction.Left && d[1] == Direction.Up) {
            rb.velocity = new Vector2(-dashPower, -dashPower);
        } else if (d[0] == Direction.Right && d[1] == Direction.Down) {
            rb.velocity = new Vector2(dashPower, dashPower);
        } else if (d[0] == Direction.Left && d[1] == Direction.Down) {
            rb.velocity = new Vector2(-dashPower, dashPower);
        }
        tr.emitting = true;

        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
