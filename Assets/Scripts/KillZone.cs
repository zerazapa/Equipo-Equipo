using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{     
    public int damageAmount = 1000;
    public GameObject MovementController;

    private void OnTriggerEnter2D()
    {
        MovementController player = GetComponent<MovementController>();
        player.TakeDamage(damageAmount);

    }


}
