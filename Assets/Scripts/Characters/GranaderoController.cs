using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranaderoController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;

    public float rayDistance = 7f;
    public Transform player;
    public Transform LLimit;
    public Transform RLimit;

    public bool canScape = true;
    public float scapeRatius = 2f;
    public float scapeSpeed = 5f;

    public float attackRatius = 6f;
    public int damageAmount = 3;
    private Health health;
    public bool canShoot = true;
    private bool isShooting = false;
    private bool isCooldownActive = false;

    public GameObject grenadePrefab;
    public Transform grenadeSpawn;
    public float grenadeSpeed = 10f;
    public float grenadeGravity = 10f;
    public float grenadeHeight = 3f;

    public bool playerisLeft = true;
    public float playerX = 0f;
    public float playerY = 0f;
    public float xToLeft = 0f;
    public float xToRight = 0f;
    private bool isInArea = true;

    public int previousHP = 0;
    public int currentHP = 0;
    private bool isUnderAttack = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        previousHP = health.HP;
        currentHP = health.HP;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float distanceToLLimit = Vector2.Distance(transform.position, LLimit.position);
        float distanceToRLimit = Vector2.Distance(transform.position, RLimit.position);
        float positionPlayer = Vector2.SignedAngle(transform.position, player.position);
        bool isAnimationPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
        playerX = player.position.x;
        playerY = player.position.y;

        currentHP = health.HP; // Obtener el valor actual de HP
        if (currentHP < previousHP && currentHP > 0) // Comparar con el valor anterior de HP
        {
            animator.SetBool("damaged", true); // Establecer el parámetro "damaged" en true
        } else{
            animator.SetBool("damaged", false);
        }
        previousHP = currentHP; // Actualizar el valor anterior de HP

        if (playerX < transform.position.x)     //determina si player is left o no
        {
            playerisLeft = true;
         } else {
            playerisLeft = false;
        }

        if ((playerX < xToLeft) || (playerX > xToRight)) //si está entre yMin y yMax, está dentro del area
        {
            isInArea = true;
         }else{
            isInArea = false;
        }

        if ((distanceToPlayer < attackRatius) && canShoot && !isCooldownActive && isInArea)  //si está dentro del rango de ataque y area de vision, fuera del rango de escape y puede disparar, atacar
        {   
            StartCoroutine(Shoot());
        }

        if (isShooting)     // si está disparando, no puede disparar más y se queda quieto
        {
            canShoot = false;
            rb.velocity = Vector2.zero;
         }else{
            canShoot = true;
        }

        if ((distanceToPlayer < scapeRatius) && !isAnimationPlaying)     //si está dentro de rango de escape, escapar, si no, idle y mirar al jugador
        {
            animator.SetBool("idle", false);
            movement = (transform.position - player.position).normalized;
            rb.velocity = movement * scapeSpeed;
            if (!playerisLeft)
            {
                transform.localScale = new Vector3(7, 7, 1);
             }
             else
             {
                transform.localScale = new Vector3(-7, 7, 1);
            }
         }else{
            rb.velocity = Vector2.zero;
            if (playerisLeft)
            {
                transform.localScale = new Vector3(7, 7, 1);
            } else{
                transform.localScale = new Vector3(-7, 7, 1);
            }
         }
         if ((distanceToPlayer < attackRatius) && canShoot && !isCooldownActive && isInArea)  //si está dentro del rango de ataque y area de vision, fuera del rango de escape y puede disparar, atacar
         {   
            StartCoroutine(Shoot());
        }

        if (isAnimationPlaying)     //si la animacion de diparo está activa, mirar al jugador
        {
            if (playerisLeft)
            {
                transform.localScale = new Vector3(7, 7, 1);
            } else{
                transform.localScale = new Vector3(-7, 7, 1);
            }
        }

        if (distanceToLLimit < 1 && !playerisLeft)   //si esta cerca del borde izquierdo y el personaje está a la derecha, deja de escapar
        {
            rb.velocity = Vector2.zero;
            canScape = false;
            animator.SetBool("idle", true);
            if (playerisLeft)
            {
                transform.localScale = new Vector3(7, 7, 1);
            } else{
                transform.localScale = new Vector3(-7, 7, 1);
            }
        }

        if (distanceToRLimit < 1 && playerisLeft)   //si esta cerca del borde derecho y el personaje está a la izquierda, deja de escapar
        {
            rb.velocity = Vector2.zero;
            canScape = false;
            animator.SetBool("idle", true);
            if (playerisLeft)
            {
                transform.localScale = new Vector3(7, 7, 1);
            } else{
                transform.localScale = new Vector3(-7, 7, 1);
            }
        }

        if (health.dead)
        {
            StartCoroutine(Death());
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("damaged") == true)
        {
            isUnderAttack = true;
        }
        else
        {
            isUnderAttack = false;
        }

        if (isUnderAttack && playerisLeft)
        {
            rb.velocity = Vector2.zero;
            transform.localScale = new Vector3(7, 7, 1);
        } else if (isUnderAttack && !playerisLeft)
        {
            rb.velocity = Vector2.zero;
            transform.localScale = new Vector3(-7, 7, 1);
        }
    }

    private IEnumerator Death()     // al disparar está disparando, se activa la animación de ataque y pasados 3 segundos ya no está disparando
    {
        animator.SetBool("dead", true);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    private IEnumerator Shoot()
    {
        isCooldownActive = true;
        isShooting = true;
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("attack", false);

        // Spawn grenade
        GameObject grenade = Instantiate(grenadePrefab, grenadeSpawn.position, Quaternion.identity);
        Rigidbody2D grenadeRb = grenade.GetComponent<Rigidbody2D>();

        // Calculate initial velocity for the grenade
        Vector2 toPlayer = player.position - transform.position;
        float distanceToPlayer = toPlayer.magnitude;
        float adjustedGrenadeSpeed = grenadeSpeed; // Velocidad inicial de la granada

        if (distanceToPlayer <= 1)
        {
            adjustedGrenadeSpeed *= 0.5f; // Reducir la velocidad si está cerca
        }
        else if (distanceToPlayer > 1)
        {
            adjustedGrenadeSpeed *= 1.5f; // Aumentar la velocidad si está lejos
        }

        float timeToReachPlayer = distanceToPlayer / adjustedGrenadeSpeed;
        float initialVelocityY = grenadeGravity * timeToReachPlayer / 2f + grenadeHeight; // Ajustar la altura de la parábola
        Vector2 initialVelocity = toPlayer.normalized * adjustedGrenadeSpeed;
        initialVelocity.y = initialVelocityY;

        // Apply the initial velocity and gravity to the grenade
        grenadeRb.velocity = initialVelocity;
        grenadeRb.gravityScale = grenadeGravity;

        yield return new WaitForSeconds(3);
        isShooting = false;
        isCooldownActive = false;
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
