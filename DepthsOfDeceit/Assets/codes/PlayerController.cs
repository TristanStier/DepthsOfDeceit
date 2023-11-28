using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float cameraSmoothTime = 0.1f;

    private Rigidbody2D rb;
    private Vector3 cameraVelocity = Vector3.zero;

    PhotonView view;  
    
    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!view.IsMine)
        {
            // Get input from the player
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Move the player
            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * speed * Time.deltaTime;
            transform.Translate(movement);

            // Jumping
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            // Camera follow
            CameraFollowPlayer();
        }
    }

    // OnCollisionEnter2D is the correct method name
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!view.IsMine)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                // Assuming you have a Rigidbody2D component attached to your player
                rb.velocity = Vector3.zero;
            }
        }
    }

    void Jump()
    {
        if(!view.IsMine)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void CameraFollowPlayer()
    {
        // Find the camera by name "MaAin Camera"
        Camera mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (mainCamera != null)
        {
            // Assuming the camera is orthographic and follows the player in the X and Y axes
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref cameraVelocity, cameraSmoothTime);
        }
        else
        {
            Debug.LogError("Main Camera not found. Make sure your camera is named 'MaAin Camera'.");
        }
    }
} 
