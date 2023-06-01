using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int HP = 4;
    private SpriteRenderer spriteRenderer;
    public Color deadColor; // Color a aplicar cuando la vida llegue a 0
    public bool dead = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
       if ((HP <= 0) && gameObject.CompareTag("Player"))
        {
            dead = true;
        }
       if ((HP <= 0) && gameObject.CompareTag("Enemy"))
       
        {
            dead = true;
        } 
    }

    
}