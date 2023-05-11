using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speed = 5f; // velocidad de movimiento del personaje
    public float jumpForce = 5f; // fuerza del salto del personaje
    public float wallSlideSpeed = 1f;
    public float wallJumpForce = 5f;

    private Rigidbody2D rb;
    public bool isTouchingGround = false; // verifica si el personaje esta en el suelo
    public bool isTouchingWall = false; // verifica si el personaje está tocando una pared
    public bool canJump = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // obtenemos la referencia al Rigidbody2D del personaje

        // obtener y modificar el valor de gravityScale
        float gravityScale = rb.gravityScale;
        rb.gravityScale = 2f;
    }

    void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal"); // obtenemos la entrada horizontal (izquierda/derecha)

        // actualizamos la posicion del transform del personaje
        transform.position += Vector3.right * horizontalMovement * speed * Time.deltaTime;
        

        // si el personaje está tocando una pared y no está en el suelo, aplica la gravedad de la pared
        if (isTouchingWall == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            canJump = true;
            rb.gravityScale = 4f;
        }

        // saltar
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K)) && (isTouchingGround || isTouchingWall))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // si el personaje está en el suelo o no toca la pared, aplica la gravedad normal
        if (isTouchingGround && !isTouchingWall)
        {
            canJump = true;
            rb.gravityScale = 4f;
        }

        if (!isTouchingGround && !isTouchingWall)
        {
            canJump = false;
        }

        // si se deja de presionar la tecla de salto, aplica la gravedad normal
        if ((Input.GetKeyUp(KeyCode.Space) && !isTouchingWall || Input.GetKeyUp(KeyCode.K) && !isTouchingWall))
        {
            rb.gravityScale = 12f;
        }
//   importanteeeeee

        Debug.Log(isTouchingGround);
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
