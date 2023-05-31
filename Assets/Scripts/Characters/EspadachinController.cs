using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspadachinController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float rayDistance = 5f;
    public Transform player;
    public Transform LLimit;
    public Transform RLimit;
    public float followRadius = 2.2f;
    public float followSpeed = 4f;
    public float attackRatius = 2f;
    public int damageAmount = 10;
    private Health health;
    private bool isAttacking = false;
    private bool isUnderAttack = false;
    public bool canFollow = true;
    private bool playerisLeft = true;
    public int previousHP = 0;
    public int currentHP = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        previousHP = health.HP;
        currentHP = health.HP;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float distanceToLLimit = Vector2.Distance(transform.position, LLimit.position);
        float distanceToRLimit = Vector2.Distance(transform.position, RLimit.position);
        float positionPlayer = Vector2.SignedAngle(transform.position, player.position);

        //Debug.Log(distanceToRLimit);

        int currentHP = health.HP; // Obtener el valor actual de HP
        if (currentHP < previousHP && currentHP > 0) // Comparar con el valor anterior de HP
        {
            animator.SetBool("damaged", true); // Establecer el parámetro "damaged" en true
        } else{
            animator.SetBool("damaged", false);
        }
        previousHP = currentHP; // Actualizar el valor anterior de HP

        if (positionPlayer < 0)
        {
            playerisLeft = true;
        } else {
            playerisLeft = false;
        }

        if (distanceToLLimit < 1 && playerisLeft)
        {
            rb.velocity = Vector2.zero;
            canFollow = false;
            animator.SetBool("idle", true);
        }
        else if (distanceToLLimit < 1 && !playerisLeft)
        {
            canFollow = true;
        }

        if (distanceToRLimit < 1 && !playerisLeft)
        {
            rb.velocity = Vector2.zero;
            canFollow = false;
            animator.SetBool("idle", true);
        }
        else if (distanceToRLimit < 1 && playerisLeft)
        {
            canFollow = true;
        }

        if ((distanceToPlayer > followRadius) && canFollow)
        {
            animator.SetBool("idle", true);
            rb.velocity = Vector2.zero;
        }
        if ((distanceToPlayer <= followRadius && health.HP > 0) && canFollow)
        {
            animator.SetBool("idle", false);
            animator.SetBool("attack", false);
            movement = (player.position - transform.position).normalized;
            rb.velocity = movement * followSpeed;
            if (movement.x < 0)
            {
                transform.localScale = new Vector3(7, 7, 1);
            }
            else if (movement.x > 0)
            {
                transform.localScale = new Vector3(-7, 7, 1);
            }
            if (distanceToPlayer < attackRatius)
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("attack", true);
            }
        }

        if (health.dead)
        {
            StartCoroutine(Death());
        }

        bool isAnimationPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
        if (isAnimationPlaying)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
        }

        bool isDamagedPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName("damaged");
        if (isDamagedPlaying)
        {
            isUnderAttack = true;
        }
        else
        {
            isUnderAttack = false;
        }

        if (isUnderAttack)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private IEnumerator Death()     // al disparar está disparando, se activa la animación de ataque y pasados 3 segundos ya no está disparando
    {
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            playerHealth.HP -= 1;
        }
    }
}
