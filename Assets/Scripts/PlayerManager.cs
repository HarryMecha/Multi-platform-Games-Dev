using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Dictionary<Collectible,int> Inventory = new Dictionary<Collectible, int>();
    public float maxHealth = 100;
    public float currentHealth;
    public Health_Bar healthBar;
    public bool FistsEquipped;
    public bool HarpoonEquipped;
    // private int damageAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
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

    public void addToInventory(Collectible collectible)
    {
        foreach (Collectible collected in Inventory.Keys)
        {
            if (collected.Name == collectible.Name)
            {
                Inventory[collected] += 1;
                Debug.Log("Additonal Item Added, Item Count : " + Inventory[collected]);
                return;
            }

        }
        Collectible collectibleToAdd = collectible;
            Inventory.Add(collectible, 1);
            Debug.Log("Item Added");
        
    }

    public bool searchInventory(string collectibleName)
    {
        if (collectibleName == "Diving Suit") FistsEquipped = true;
        if (collectibleName == "HarpoonGun") HarpoonEquipped = true;
        foreach (Collectible collected in Inventory.Keys)
        {
            if (collected.Name == collectibleName)
            {
                Inventory[collected] += 1;
                Debug.Log("Additonal Item Added, Item Count : " + Inventory[collected]);
                return true;
            }

        }
        return false;

    }

}
