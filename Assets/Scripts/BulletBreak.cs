using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBreak : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destruir la bala actual
        }
        if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PBullet"))
        {
            Health objectHealth = collision.gameObject.GetComponent<Health>();
            objectHealth.HP -= 3;
            Destroy(gameObject); // Destruir la bala actual
        }
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("EBullet"))
        {
            Health objectHealth = collision.gameObject.GetComponent<Health>();
            objectHealth.HP -= 2;
            Destroy(gameObject); // Destruir la bala actual
        }
    }
}
