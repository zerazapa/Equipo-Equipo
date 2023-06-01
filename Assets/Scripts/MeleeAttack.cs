using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public int damage = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PSword"))
        {
            Health objectHealth = collision.gameObject.GetComponent<Health>();
            objectHealth.HP -= damage;
        }
        if(collision.gameObject.CompareTag("Player") && gameObject.CompareTag("ESword"))
        {
            Health objectHealth = collision.gameObject.GetComponent<Health>();
            objectHealth.HP -= damage;
        }
    }

}
