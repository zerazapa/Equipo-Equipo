using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{     
    public GameObject MovementController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
        Health objectHealth = collision.gameObject.GetComponent<Health>();
        objectHealth.HP = 0;
        }
    }

}
