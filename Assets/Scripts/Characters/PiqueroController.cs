using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiqueroController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public float rayDistance = 10f;
    public Transform player;

    public float awareRatius = 6f;
    public float attackRatius = 6f;
    public int damageAmount = 3;
    public bool canAttack = true;
    private bool isAttacking = false;
    private bool isCooldownActive = false;

    public bool playerisLeft = true;
    public float playerX = 0f;
    public float playerY = 0f;
    public float yMinAttackable = 1f;
    public float yMaxAttackable = 2;
    public bool isInArea = false;

    public GameObject back;
    private Health backHealth;

    public int previousHP = 0;
    public int currentHP = 0;
    private bool isUnderAttack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        backHealth = back.GetComponent<Health>();
        previousHP = backHealth.HP;
        currentHP = backHealth.HP;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float positionPlayer = Vector2.SignedAngle(transform.position, player.position);
        bool isAnimationPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
        playerX = player.position.x;
        playerY = player.position.y;

        int currentHP = backHealth.HP; // Obtener el valor actual de HP
        if (currentHP < previousHP && currentHP > 0) // Comparar con el valor anterior de HP
        {
            animator.SetBool("damaged", true); // Establecer el parámetro "damaged" en true
        } else{
            animator.SetBool("damaged", false);
        }
        previousHP = currentHP;

        if (playerX < transform.position.x)     //determina si player is left o no
        {
            playerisLeft = true;
         } else {
            playerisLeft = false;
        }

        if (!playerisLeft && (distanceToPlayer < awareRatius))
        {
            StartCoroutine(FlipR());
        }else if (playerisLeft && (distanceToPlayer < awareRatius))
        {
            StartCoroutine(FlipL());
        }

        if ((playerY > yMinAttackable) && (playerY < yMaxAttackable)) //si está entre yMin y yMax, está dentro del area
        {
            isInArea = true;
         }else{
            isInArea = false;
        }

        if ((distanceToPlayer < attackRatius) && canAttack && isInArea && !isCooldownActive)  //si está dentro del rango de ataque y area de vision, fuera del rango de escape y puede disparar, atacar
        {   
            if (playerisLeft && (transform.localScale == new Vector3(7, 7, 1)))
            {
            StartCoroutine(AttackCooldown());
            }
            if (!playerisLeft && (transform.localScale == new Vector3(-7, 7, 1)))
            {
            StartCoroutine(AttackCooldown());
            }
        }

        if (isAttacking)     // si está disparando, no puede disparar más y se queda quieto
        {
            canAttack = false;
         }else{
            canAttack = true;            
        }

        if (backHealth.dead)
        {
            StartCoroutine(Death());
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

    private IEnumerator AttackCooldown()     // al disparar está disparando, se activa la animación de ataque y pasados 3 segundos ya no está disparando
    {
        isCooldownActive = true;
        isAttacking = true;
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(0.7f);
        animator.SetBool("attack", false);
        yield return new WaitForSeconds(3);
        isAttacking = false;
        isCooldownActive = false;
    }

    private IEnumerator FlipL()
    {   
        yield return new WaitForSeconds(2);
        transform.localScale = new Vector3(7, 7, 1);
    }

    private IEnumerator FlipR()
    {   
        yield return new WaitForSeconds(2);
        transform.localScale = new Vector3(-7, 7, 1);
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
