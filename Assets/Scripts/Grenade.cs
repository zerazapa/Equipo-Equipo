using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D grenadeCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        grenadeCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("break") == true)
            {
                Destroy(gameObject);
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        grenadeCollider.isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        grenadeCollider.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Cambiar dirección de movimiento en el eje x
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("prepare", true);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("explode", true);
        }
    }
}