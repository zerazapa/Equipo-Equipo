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
    public bool isFacingRight = true; // variable para saber si el personaje está mirando hacia la derecha
    public float dashVerticalForce = 10f;
    public float dashHorizontalForce = 10f;
    public float dashDuration = 1f;
    private bool isDashing = false;
    private float dashTimer = 0f;

    [SerializeField] public Transform groundCheck;
    [SerializeField] public Vector2 groundCheckSize;
    [SerializeField] public Transform wallCheckL;
    [SerializeField] public Vector2 wallCheckLSize;
    [SerializeField] public Transform wallCheckR;
    [SerializeField] public Vector2 wallCheckRSize;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float gravityScale = rb.gravityScale;
        rb.gravityScale = 2f;
    }

    void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");

        transform.position += Vector3.right * horizontalMovement * speed * Time.deltaTime;

        // actualizamos la variable isFacingRight dependiendo de la dirección del movimiento horizontal
        if (horizontalMovement < 0)
        {
            isFacingRight = false;
        }
        else if (horizontalMovement > 0)
        {
            isFacingRight = true;
        }

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

        if ((Input.GetKeyUp(KeyCode.Space) && (!wallCheckL && !wallCheckR) || Input.GetKeyUp(KeyCode.K) && (!wallCheckL && !wallCheckR)))
        {
            rb.gravityScale = 12f;
        }

        if (Input.GetKeyDown(KeyCode.L) && !isDashing)
        {
            isDashing = true;
            dashTimer = dashTimer+0.1f;

            if (isFacingRight)
            {
                rb.AddForce(Vector2.right * dashHorizontalForce, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(Vector2.left * dashHorizontalForce, ForceMode2D.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K))
            {
                float dashVerticalDirection = Mathf.Sign(rb.velocity.y);
                rb.AddForce(Vector2.up * dashVerticalDirection * dashVerticalForce, ForceMode2D.Impulse);
            }
        }

        if (isDashing)
        {
            dashTimer += Time.deltaTime;

            if (dashTimer >= dashDuration)
            {
                isDashing = false;
            }
        }
        Debug.Log(isTouchingGround);
        //Debug.Log(wallCheckR);
        //Debug.Log(dashTimer);
        //Debug.Log(dashDuration);
        //Debug.Log(isFacingRight);
    }   

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isTouchingGround = true;
            }
            if (collision.gameObject.CompareTag("Wall"))
            {
                isTouchingWall = true;
            }

        }
        void OnCollisionExit2D(Collision2D collision)
        {
            Collider2D[] groundCollider = Physics2D.OverlapBoxAll(groundCheck.position,groundCheckSize, 0f);
            bool isCollidingWithGround = false;
            foreach (Collider2D col in groundCollider)
            {
                if (col.gameObject.CompareTag("Ground"))
                {
                    isCollidingWithGround = true;
                    break;
                }
            }

            // Check if touching left wall
            Collider2D[] wallColliderLeft = Physics2D.OverlapBoxAll(wallCheckL.position, wallCheckLSize, 0f);
            bool isCollidingWithWallLeft = false;
            foreach (Collider2D col in wallColliderLeft)
            {
                if (col.gameObject.CompareTag("Wall"))
                {
                    isCollidingWithWallLeft = true;
                    break;
                }
            }

            // Check if touching right wall
            Collider2D[] wallColliderRight = Physics2D.OverlapBoxAll(wallCheckR.position, wallCheckRSize, 0f);
            bool isCollidingWithWallRight = false;
            foreach (Collider2D col in wallColliderRight)
            {
                if (col.gameObject.CompareTag("Wall"))
                {
                    isCollidingWithWallRight = true;
                    break;
                }
            }
            isTouchingWall = isCollidingWithWallLeft || isCollidingWithWallRight;
            isTouchingGround = isCollidingWithGround;
        //Debug.Log("isCollidingWithWallLeft: " + isCollidingWithWallLeft);
        //Debug.Log("isCollidingWithWallRight: " + isCollidingWithWallRight);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawRay(groundCheck.position, groundCheckSize);
        Gizmos.DrawCube(groundCheck.position, groundCheckSize);
        Gizmos.DrawCube(wallCheckL.position, wallCheckLSize);
        Gizmos.DrawCube(wallCheckR.position, wallCheckRSize);

    }

}
