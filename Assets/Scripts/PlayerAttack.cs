using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 10f;
    public float gunCooldown = 3f;
    public float swordCooldown = 0.2f;
    private bool canShoot = true;
    private MovementController movementController;
    public float facing = 0;
    public bool canMelee = true;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        if (!canMelee)
        {
            // Si el ataque cuerpo a cuerpo está en progreso, no permitir movimiento
            movementController.enabled = false;
        }
        else
        {
            movementController.enabled = true;
        }

        movementController = GetComponent<MovementController>();
        facing = movementController.hFacing;

        if (Input.GetKeyDown(KeyCode.J) && canShoot)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetKeyDown(KeyCode.H) && canMelee)
        {
            StartCoroutine(Melee());
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;

        // Obtener la rotación del personaje desde la variable "whereFacing" del script MovementController
        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        if (facing == -1)
        {
            // Disparar a la izquierda
            rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (facing == 0)
        {
            // Disparar a la derecha
            rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        // Crear la bala y obtener su Rigidbody
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, rotation);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();

        // Aplicar velocidad a la bala en la dirección deseada
        Vector3 bulletDirection = bullet.transform.right;
        bulletRigidbody.velocity = bulletDirection * bulletSpeed;

        yield return new WaitForSeconds(gunCooldown);

        canShoot = true;
    }

    private IEnumerator Melee()
    {
        canMelee = false;
        animator.SetBool("isMelee", true);
        yield return new WaitForSeconds(swordCooldown);
        canMelee = true;
        animator.SetBool("isMelee", false);
    }
}
