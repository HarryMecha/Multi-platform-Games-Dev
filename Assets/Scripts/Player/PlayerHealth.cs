using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth= 100;
    public float currentHealth;
    public Health_Bar healthBar;
   // private int damageAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth=maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    void TakeDamage(float damage)
        {
            currentHealth-=damage;
            healthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        void Die()
        {
            Debug.Log("Player died.");
        }

    void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                TakeDamage(10);
            }
            else if (collision.gameObject.CompareTag("Projectile"))
            {
                TakeDamage(5);
            }
            else if (collision.gameObject.CompareTag("Trap"))
            {
                TakeDamage(7.5f);
            }
        }
}
