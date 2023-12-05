using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input for movement (assuming you have a variable like horizontalInput)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Update the Animator parameter based on movement input
        UpdateAnimatorParameters(horizontalInput);
    }

    void UpdateAnimatorParameters(float horizontalInput)
    {
        // Assuming "IsMoving" is the boolean parameter in the Animator
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;

        // Set the "IsMoving" parameter in the Animator
        animator.SetBool("IsMoving", isMoving);

        // Set separate parameters for left and right movement
        if (horizontalInput < 0)
        {
            // Moving left
            animator.SetBool("IsMovingLeft", true);
            animator.SetBool("IsMovingRight", false);
        }
        else if (horizontalInput > 0)
        {
            // Moving right
            animator.SetBool("IsMovingLeft", false);
            animator.SetBool("IsMovingRight", true);
        }
        else
        {
            // Not moving horizontally
            animator.SetBool("IsMovingLeft", false);
            animator.SetBool("IsMovingRight", false);
        }
    }
}
