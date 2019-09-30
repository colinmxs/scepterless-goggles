using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float GroundRayLength;
    public float FallMultiplier;
    public float MoveSpeed;
    public float JumpStrength;
    private float axisHorizontal;
    private float movement;
    private float smoothing;
    private Vector3 moveVector;
    private bool isGrounded;
    private Rigidbody2D rb;
    private int jumpGraceTime;
    private int jumpGraceCountdown;   

    private void Start()
    {
        smoothing = 0.05f;
        moveVector = Vector3.zero;
        movement = 0f;
        jumpGraceTime = 5;
        rb = gameObject.GetComponent<Rigidbody2D>();
        tag = "Player";
    }

    private void HandleMovement()
    {
        axisHorizontal = Input.GetAxisRaw("Horizontal") * MoveSpeed;

        if (axisHorizontal != 0.0)
        {
            Vector3 targetVelocity = new Vector2(movement * 10f, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref moveVector, smoothing);
        }
    }

    private void CheckForGround()
    {
        int layerMask = LayerMask.GetMask("Solid");

        // Ground Ray 1
        Vector3 rayPosition1;
        rayPosition1 = new Vector3(transform.position.x - 0.3f, transform.position.y - 1.15f, transform.position.z);
        RaycastHit2D hit1 = Physics2D.Raycast(rayPosition1, -Vector2.up, GroundRayLength, layerMask);
        Debug.DrawRay(rayPosition1, -Vector2.up * GroundRayLength, Color.blue);

        // Ground Ray 2
        Vector3 rayPosition2;
        rayPosition2 = new Vector3(transform.position.x, transform.position.y - 1.15f, transform.position.z);
        RaycastHit2D hit2 = Physics2D.Raycast(rayPosition2, -Vector2.up, GroundRayLength, layerMask);
        Debug.DrawRay(rayPosition2, -Vector2.up * GroundRayLength, Color.blue);

        // Ground Ray 3
        Vector3 rayPosition3;
        rayPosition3 = new Vector3(transform.position.x + 0.3f, transform.position.y - 1.15f, transform.position.z);
        RaycastHit2D hit3 = Physics2D.Raycast(rayPosition3, -Vector2.up, GroundRayLength, layerMask);
        Debug.DrawRay(rayPosition3, -Vector2.up * GroundRayLength, Color.blue);

        // If ray hit ground
        if (hit1.collider != null
        || hit2.collider != null
        || hit3.collider != null)
        {
            // Grounded
            isGrounded = true;

            // Change debug color
            Debug.DrawRay(rayPosition1, -Vector2.up * GroundRayLength, Color.yellow);
            Debug.DrawRay(rayPosition2, -Vector2.up * GroundRayLength, Color.yellow);
            Debug.DrawRay(rayPosition3, -Vector2.up * GroundRayLength, Color.yellow);
        }
        else
        {
            // Not rounded
            isGrounded = false;
        }
    }

    private void HandleJump()
    {
        if (isGrounded == false)
        {
            // Count down grace period
            jumpGraceCountdown -= 1;
        }
        else
        {
            // Reset jump grace period
            jumpGraceCountdown = jumpGraceTime;
        }

        // If jump is pressed
        if (Input.GetButtonDown("Jump") == true)
        {
            // And is inside grace period
            if (jumpGraceCountdown > 0)
            {
                // Add jump force
                rb.AddForce(new Vector2(0f, JumpStrength));
            }
        }

        // If falling down OR moving upward and jump is released
        if (rb.velocity.y < 0 || (rb.velocity.y > 0 && Input.GetButton("Jump") == false))
        {
            // Increase downward gravity by fall multiplier
            rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Update()
    {
        HandleMovement();
        CheckForGround();
        HandleJump();
    }

    private void FixedUpdate()
    {
        movement = axisHorizontal * Time.fixedDeltaTime;
    }
}