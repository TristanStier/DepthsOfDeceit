using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public float moveSpeed = 5f;
    public int lifePoints = 2;
    public bool invulnerable = false;
    private bool canDash = true;
    public float invulnerableTime = 1f;
    private bool isDashing;
    public float dashPower = 10f;
    public float dashTime = 0.2f;
    public float dashCooldown = 0.1f;
    //private Direction[] d = {Direction.Right, Direction.None}; 
    
    public Sprite spriteLeft;
    public Sprite spriteRight;
    public Rigidbody2D rb;

    public AudioSource dash;
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
            //d[0] = Direction.Left;
            //d[1] = Direction.None;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteLeft;
        } else if (movement.x > 0 && movement.y == 0) {
            //d[0] = Direction.Right;
            //d[1] = Direction.None;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteRight;
        } else if (movement.y < 0 && movement.x == 0) {
            //d[0] = Direction.None;
            //d[1] = Direction.Up;
        } else if (movement.y > 0 && movement.x == 0) {
            //d[0] = Direction.None;
            //d[1] = Direction.Down;
        } else if (movement.y < 0 && movement.x < 0) {
            //d[0] = Direction.Left;
            //d[1] = Direction.Up;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteLeft;
        } else if (movement.y < 0 && movement.x > 0) {
            //d[0] = Direction.Right;
            //d[1] = Direction.Up;
             gameObject.GetComponent<SpriteRenderer>().sprite = spriteRight;
        } else if (movement.y > 0 && movement.x < 0) {
            //d[0] = Direction.Left;
            //d[1] = Direction.Down;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteLeft;
        } else if (movement.y > 0 && movement.x > 0) {
            //d[0] = Direction.Right;
            //d[1] = Direction.Down;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteRight;
        }

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
        invulnerable = true;
        
        dash.Play();
        rb.velocity = new Vector2(movement.x * dashPower, movement.y * dashPower);
        tr.emitting = true;

        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        isDashing = false;
        invulnerable = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public IEnumerator setInvulnerable() {
        invulnerable = true;
        yield return new WaitForSeconds(invulnerableTime);
        invulnerable = false;
    }
}
