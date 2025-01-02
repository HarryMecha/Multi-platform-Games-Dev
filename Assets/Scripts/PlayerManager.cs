using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<InventoryItem> Inventory = new List<InventoryItem>();
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

    public void IncreaseHealth(float increase)
    {
        currentHealth += increase;
        if (currentHealth > 100)
        {
            currentHealth = 100;
        }

        healthBar.SetHealth(currentHealth);
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
        foreach (InventoryItem collected in Inventory)
        {
            if (collected.getCollectible().Name == collectible.Name)
            {
                collected.addToCount();
                Debug.Log("Additonal Item Added, Item Count : " + collected.getCount());
                return;
            }

        }
        Collectible collectibleToAdd = collectible;
        Inventory.Add(new InventoryItem(collectibleToAdd, 1));
            //Debug.Log("Item Added "+collectibleToAdd.Name);
    }

    public void useItem(Collectible collectible)
    {
        if (collectible.isUseable == true)
        {
            if (collectible.HealthIncrease > 0)
            {
                if(currentHealth == 100)
                {
                    return;
                }else
                IncreaseHealth(collectible.HealthIncrease);
            }
        }
        int index = 0;
        foreach (InventoryItem collected in Inventory)
        {
            if (collected.getCollectible().Name == collectible.Name)
            {
                if (collected.getCount() > 0)
                {
                    collected.removeFromCount();

                    if (collected.getCount() <= 0)
                    {
                        Inventory.RemoveAt(index);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

            }
            index++;
        }
    }

    
    public void swapItems(Collectible item1, Collectible item2)
    {
        Debug.Log(item1.Name);
        Debug.Log(item2.Name);
        int indexOf1 = -1;
        int indexOf2 = -1;
        int index = 0;
        foreach (InventoryItem collected in Inventory)
        {
            if (collected.getCollectible().Name == item1.Name)
            {
                indexOf1 = index;
                Debug.Log(indexOf1);
            }
            if (collected.getCollectible().Name == item2.Name)
            {
                indexOf2 = index;
                Debug.Log(indexOf2);
            }
            index++;
        }
        InventoryItem Temp = Inventory[indexOf2];
        Inventory[indexOf2] = Inventory[indexOf1];
        Inventory[indexOf1] = Temp;
    }
    

    public bool isInInventory(string collectibleName)
    {
        foreach (InventoryItem collected in Inventory)
        {
            if (collected.getCollectible().Name == collectibleName)
            {
                return true;
            }

        }
        return false;
    }

    public List<InventoryItem> getInventory()
    {

        return Inventory;

    }

    public void setFistsEquipped(bool fistsEquipped)
    {
        FistsEquipped = fistsEquipped;
    }

    public void setHarpoonEquipped(bool harpoonEquipped)
    {
        HarpoonEquipped = harpoonEquipped;
    }
    public void printKeys()
    {
        foreach (InventoryItem collected in Inventory)
        {
            Debug.Log(collected.getCollectible().Name);
        }
        }

}
