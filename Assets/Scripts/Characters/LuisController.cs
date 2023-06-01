using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuisController : MonoBehaviour
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
    public float dashVerticalForce = 7f;
    public float dashHorizontalForce = 10f;
    public bool canDash = true;
    public bool isDashing = false;
    public float dashDuration = 1f;
    private float dashStartTime;
    private Animator animator;

    public bool canShoot= true;
    public bool canMelee = true;

    public bool isFacingLeft = false;
    private float lastFacing = 0f;
    private float vFacing = 0f;
    private float hFacing = 0f;

    private Health health;
    public int previousHP = 0;
    public int currentHP = 0;

    public GameObject GameOver;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        float gravityScale = rb.gravityScale;
        rb.gravityScale = 2f;

        health = GetComponent<Health>();
        previousHP = health.HP;
        currentHP = health.HP;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            lastFacing = 1;
        }
        else
        {
            lastFacing = 0;
        }

        if (!canJump)
        {
            animator.SetBool("jump", true);
        } else
        {
            animator.SetBool("jump", false);
        }


        if (isTouchingGround || isTouchingWall)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            canJump = true;
            rb.gravityScale = 4f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump || Input.GetKeyDown(KeyCode.K) && canJump)
        {
            Jump();
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

        if (((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.K)) && (!isTouchingWall) || Input.GetKeyUp(KeyCode.K) && (!isTouchingWall)))
        {
            rb.gravityScale = 12f;
        }

        if (Input.GetKeyDown(KeyCode.L) && canDash && !isDashing)
        {
            Dash();
            dashStartTime = Time.time;
        }

        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        transform.position += Vector3.right * horizontalMovement * speed * Time.deltaTime;

        if (horizontalMovement == 0f)
        {
            animator.SetBool("idle", true);
        }
        else if (horizontalMovement == -1f)
        {
            animator.SetBool("idle", false);
            isFacingLeft = true;
            transform.localScale = new Vector3(-7, 7, 1);
        }
        else if (horizontalMovement == 1f)
        {
            animator.SetBool("idle", false);
            isFacingLeft = false;
            transform.localScale = new Vector3(7, 7, 1);
        }

        // Si ha pasado el tiempo máximo del dash, ya no permitir el dash
        if (isDashing && Time.time >= dashStartTime + dashDuration)
        {
            isDashing = false;
            rb.velocity = Vector2.zero;
            canDash = true;
        }

        int currentHP = health.HP; // Obtener el valor actual de HP
        if (currentHP < previousHP && currentHP > 0) // Comparar con el valor anterior de HP
        {
            animator.SetBool("damaged", true); // Establecer el parámetro "damaged" en true
        }
        else
        {
            animator.SetBool("damaged", false);
        }
        previousHP = currentHP;

        if (health.dead)
        {
            StartCoroutine(Death());
        }
    }
    
    void Jump()
    {
        if (rb.velocity.y > maxJumpHeight / 2f)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxJumpHeight / 2f);
            }
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    void Dash()
    {
        if (lastFacing == 0f && !isFacingLeft)
        {
            rb.AddForce(Vector2.right * dashHorizontalForce, ForceMode2D.Impulse);
        }
        else if (lastFacing == 0f && isFacingLeft)
        {
            rb.AddForce(Vector2.left * dashHorizontalForce, ForceMode2D.Impulse);
        }
        else if (lastFacing == 1f && vFacing == 1f)
        {
            rb.AddForce(Vector2.up * dashVerticalForce, ForceMode2D.Impulse);
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

    private IEnumerator Death()     // al disparar está disparando, se activa la animación de ataque y pasados 3 segundos ya no está disparando
    {
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        GameOver.SetActive(true);
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
