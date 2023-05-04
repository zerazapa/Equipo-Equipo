using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speed = 5f; // velocidad de movimiento del personaje
    public float jumpForce = 5f; // fuerza del salto del personaje

    private Rigidbody2D rb;
    private bool isGrounded = true; // verifica si el personaje está en el suelo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // obtenemos la referencia al Rigidbody2D del personaje
    }

    void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal"); // obtenemos la entrada horizontal (izquierda/derecha)

        // actualizamos la posición del transform del personaje
        transform.position += Vector3.right * horizontalMovement * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}