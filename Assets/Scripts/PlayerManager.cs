using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<InventoryItem> Inventory;
    public float maxHealth = 100;
    public float currentHealth;
    public GameObject playerHealthBar;
    public Health_Bar healthBar;
    public bool FistsEquipped;
    public bool HarpoonEquipped;
    public string currentHarpoonEquipped;
    private EnviromentManager enviromentManager;
    // private int damageAmount = 0;

    private void Start()
    {
        currentHarpoonEquipped = "none";
        enviromentManager = transform.GetComponent<EnviromentManager>();
    }
    public void setupHealthBar()
    {
        playerHealthBar = GameObject.Find("Player Health Bar");
        healthBar = playerHealthBar.GetComponent<Health_Bar>();
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
        enviromentManager.Controller.HUDCanvas.GetComponent<PauseMenu>().ActivateDeathMenu();
        IncreaseHealth(100);
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
        Debug.Log(collectible);

        // Check if the item already exists in the inventory
        foreach (InventoryItem collected in Inventory)
        {
            if (collected.getCollectible().Name == collectible.Name)
            {
                collected.addToCount();  // Increase the count of the item in the inventory
                Debug.Log("Additional Item Added, Item Count : " + collected.getCount());
                return;  // Exit the method as the item already exists
            }
        }

        // If the item doesn't exist in the inventory, create a new GameObject for it
        GameObject newInventoryItemObject = new GameObject("InventoryItem_" + collectible.Name);

        // Add the Collectible component to the new GameObject
        Collectible collectibleToAdd = newInventoryItemObject.AddComponent<Collectible>();
        collectibleToAdd.Name = collectible.Name;
        collectibleToAdd.Description = collectible.Description;
        collectibleToAdd.InventoryPicture = collectible.InventoryPicture;
        collectibleToAdd.isUseable = collectible.isUseable;
        collectibleToAdd.isEquippable = collectible.isEquippable;
        collectibleToAdd.EquipType = collectible.EquipType;
        if (collectibleToAdd.isUseable)
        {
            if (collectible.HealthIncrease > 0)
            {
                collectibleToAdd.HealthIncrease = collectible.HealthIncrease;
            }
        }

        // Add the InventoryItem component to the new GameObject
        InventoryItem inventoryItem = newInventoryItemObject.AddComponent<InventoryItem>();
        inventoryItem.setCollectible(collectibleToAdd);  // Set the collectible on the InventoryItem
        inventoryItem.setCount(1);  // Initialize count to 1 for the new item

        // Add the new InventoryItem to the inventory list
        Inventory.Add(inventoryItem);

        // Optional: Parent the new InventoryItem object to the current GameObject
        newInventoryItemObject.transform.parent = transform;

        Debug.Log("Item Added: " + inventoryItem.getCollectible().Name);
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

    public void equipItem(Collectible collectible)
    {
        Debug.Log("euipping");
        if (collectible.isEquippable == true)
        {
            Debug.Log("first check");
            switch (collectible.EquipType)
            {
                case (Collectible.equipType.harpoon):
                    Debug.Log("second check");
                    currentHarpoonEquipped = collectible.Name;
                    break;

            }
        }

    }

    public void unequipItem(Collectible collectible)
    {
        if (collectible.isEquippable == true)
        {
            switch (collectible.EquipType)
            {
                case (Collectible.equipType.harpoon):
                    currentHarpoonEquipped = "none";
                    break;

            }
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

    public int getItemCount(string collectibleName)
    {
        foreach (InventoryItem collected in Inventory)
        {
            if (collected.getCollectible().Name == collectibleName)
            {
                return collected.getCount();
            }

        }
        return 0;
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
