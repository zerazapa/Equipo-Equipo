using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 10f;
    public float gunCooldown = 3f;
    public float swordCooldown = 0.2f;
    private bool canShoot = true;
    private bool canMelee = true;
    private Animator animator;

    private LuisController luisController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        luisController = GetComponentInParent<LuisController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && canShoot && luisController.canShoot)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetKeyDown(KeyCode.H) && canMelee && luisController.canMelee)
        {
            StartCoroutine(Melee());
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        canMelee = false;
        animator.SetBool("shoot", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("shoot", false);
        canMelee = true;

        if (luisController.isFacingLeft == false)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
            rotation = Quaternion.Euler(0f, 0f, 0f);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, rotation);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            Vector3 bulletDirection = bullet.transform.right;
            bulletRigidbody.velocity = bulletDirection * bulletSpeed;
        } else
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
            rotation = Quaternion.Euler(0f, -180f, 0f);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, rotation);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            Vector3 bulletDirection = bullet.transform.right;
            bulletRigidbody.velocity = bulletDirection * bulletSpeed;
        }
        

        yield return new WaitForSeconds(gunCooldown);

        canShoot = true;
    }

    private IEnumerator Melee()
    {
        animator.SetBool("melee", true);
        Debug.Log("paaaaa");
        canMelee = false;
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("melee", false);
        yield return new WaitForSeconds(swordCooldown);
        canMelee = true;
    }
}
