using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
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
            rb.velocity = new UnityEngine.Vector2(horizontalInput, verticalInput).normalized*speed;

            // Flipping Character
            if(horizontalInput > 0.01f)
            {
                transform.localScale = new UnityEngine.Vector3(1, 1, 1);
            }
            else if(horizontalInput < -0.01f)
            {
                transform.localScale = new UnityEngine.Vector3(-1, 1, 1);
            }

            // Camera follow
            CameraFollowPlayer();
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
            Debug.LogError("Main Camera not found. Make sure your camera is named 'Main Camera'.");
        }
    }
} 