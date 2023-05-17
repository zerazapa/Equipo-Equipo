    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
using UnityEngine.UIElements;

public class MovementController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float wallSlideSpeed = 2f;
    public float wallJumpForce = 5f;
    public float maxJumpHeight = 6f;

    private Rigidbody2D rb;
    public bool isTouchingGround = false;
    public bool isTouchingWall = false;
    public bool canJump = true;
    public float whereFacing = 1f; // 0=L 1=R 2=U 3=D
    public float dashVerticalForce = 7f;
    public float dashHorizontalForce = 10f;
    public bool canDash = true;
    public bool isDashing = false;
    public float dashDuration = 1f;
    private float dashStartTime;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float gravityScale = rb.gravityScale;
        rb.gravityScale = 2f;
    }

    void Update()
    {
        if (isTouchingWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            canJump = true;
            rb.gravityScale = 4f;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K)) && (isTouchingGround || isTouchingWall))
        {
            if (rb.velocity.y > maxJumpHeight / 2f)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxJumpHeight / 2f);
            }
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (isTouchingGround && !isTouchingWall)
        {
            canJump = true;
            rb.gravityScale = 4f;
        }

        if (!isTouchingGround && !isTouchingWall)
        {
            canJump = false;
        }

        if ((Input.GetKeyUp(KeyCode.Space) && (!isTouchingWall) || Input.GetKeyUp(KeyCode.K) && (!isTouchingWall)))
        {
            rb.gravityScale = 12f;
        }

        if (Input.GetKeyDown(KeyCode.L) && canDash && !isDashing)
        {
            Dash();
            dashStartTime = Time.time;
        }
    }   

    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        transform.position += Vector3.right * horizontalMovement * speed * Time.deltaTime;

        if (horizontalMovement < 0f)
        {
            whereFacing = 0f;
        }
        if (horizontalMovement > 0f)
        {
            whereFacing = 1f;
        }
        if (verticalMovement > 0f)
        {
            whereFacing = 2f;
        }
        if (verticalMovement < -0.1f)
        {
            whereFacing = 3f;
        }
        // Si ha pasado el tiempo máximo del dash, ya no permitir el dash
        if (isDashing && Time.time >= dashStartTime + dashDuration)
        {
            isDashing = false;
            rb.velocity = Vector2.zero;
            canDash = true;
        }
        Debug.Log(isDashing);
    }
    void Dash()
    {
        if (whereFacing == 1f)
        {
           rb.AddForce(Vector2.right * dashHorizontalForce, ForceMode2D.Impulse);
        }
        else if (whereFacing == 0f)
        {
            rb.AddForce(Vector2.left * dashHorizontalForce, ForceMode2D.Impulse);
        }
        else if (whereFacing == 2f)
        {
            rb.AddForce(Vector2.up * dashVerticalForce, ForceMode2D.Impulse);
        }
        else if (whereFacing == 3f)
        {
            rb.AddForce(Vector2.down * dashHorizontalForce, ForceMode2D.Impulse);
        }

    isDashing = true;
    // Detener el dash después de dashDuration segundos
    Invoke("StopDashing", dashDuration);
    }

    void StopDashing()
    {
        isDashing = false;
        canDash = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isTouchingGround = true;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
        }
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Wall")))
        {
            canDash = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isTouchingGround = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
        }
    }
        
}
