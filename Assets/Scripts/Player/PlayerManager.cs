using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Fields
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
    #endregion

    private void Start()
    {
        // Initialize variables at the beginning of the scene
        currentHarpoonEquipped = "none";
        enviromentManager = transform.GetComponent<EnviromentManager>();
    }

    // Sets up the player's health bar with maximum health at the beginning of a scene
    public void setupHealthBar()
    {
        playerHealthBar = GameObject.Find("Player Health Bar");
        healthBar = playerHealthBar.GetComponent<Health_Bar>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Called when the player takes damage, updates the health bar and checks if health reaches zero
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Increases the player's health and updates the health bar
    public void IncreaseHealth(float increase)
    {
        currentHealth += increase;
        if (currentHealth > 100)
        {
            currentHealth = 100;
        }

        healthBar.SetHealth(currentHealth);
    }

    // Called when the player dies, activates the death menu and resets health
    void Die()
    {
        enviromentManager.Controller.HUDCanvas.GetComponent<PauseMenu>().ActivateDeathMenu();
        IncreaseHealth(100);
    }

    // Handles collisions with different objects and applies corresponding damage
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

    // Adds a collectible item to the player's inventory, creating a new entry if it doesn't already exist
    public void addToInventory(Collectible collectible)
    {
        Debug.Log(collectible);

        // Check if the item already exists in the inventory
        foreach (InventoryItem collected in Inventory)
        {
            if (collected.getCollectible().Name == collectible.Name)
            {
                // If the item exists, increase its count and exit the method
                collected.addToCount();
                Debug.Log("Additional Item Added, Item Count : " + collected.getCount());
                return;
            }
        }

        // Create a new inventory item if it doesn't exist
        GameObject newInventoryItemObject = new GameObject("InventoryItem_" + collectible.Name);

        // Copy the properties of the collectible into the new object
        Collectible collectibleToAdd = newInventoryItemObject.AddComponent<Collectible>();
        collectibleToAdd.Name = collectible.Name;
        collectibleToAdd.Description = collectible.Description;
        collectibleToAdd.InventoryPicture = collectible.InventoryPicture;
        collectibleToAdd.isUseable = collectible.isUseable;
        collectibleToAdd.isEquippable = collectible.isEquippable;
        collectibleToAdd.EquipType = collectible.EquipType;
        if (collectibleToAdd.isUseable && collectible.HealthIncrease > 0)
        {
            collectibleToAdd.HealthIncrease = collectible.HealthIncrease;
        }

        // Add the collectible to the inventory as a new item
        InventoryItem inventoryItem = newInventoryItemObject.AddComponent<InventoryItem>();
        inventoryItem.setCollectible(collectibleToAdd);
        inventoryItem.setCount(1);
        Inventory.Add(inventoryItem);

        // Parent the new object to the player's transform
        newInventoryItemObject.transform.parent = transform;

        Debug.Log("Item Added: " + inventoryItem.getCollectible().Name);
    }

    // Uses an item from the inventory and applies its effect
    public void useItem(Collectible collectible)
    {
        if (collectible.isUseable && collectible.HealthIncrease > 0)
        {
            if (currentHealth == 100) return;
            IncreaseHealth(collectible.HealthIncrease);
        }

        int index = 0;
        foreach (InventoryItem collected in Inventory)
        {
            if (collected.getCollectible().Name == collectible.Name)
            {
                if (collected.getCount() > 0)
                {
                    // Reduce the count of the item and remove it if the count reaches zero
                    collected.removeFromCount();

                    if (collected.getCount() <= 0)
                    {
                        Inventory.RemoveAt(index);
                        return;
                    }

                    return;
                }
            }
            index++;
        }
    }

    // Equips an item if it is equippable, based on its type
    public void equipItem(Collectible collectible)
    {
        Debug.Log("equipping");
        if (collectible.isEquippable)
        {
            Debug.Log("first check");
            switch (collectible.EquipType)
            {
                case (Collectible.equipType.harpoon):
                    // If the item is a harpoon, set it as the currently equipped item
                    Debug.Log("second check");
                    currentHarpoonEquipped = collectible.Name;
                    break;

            }
        }
    }

    // Unequips an item if it is equippable, resetting the equipped state
    public void unequipItem(Collectible collectible)
    {
        if (collectible.isEquippable)
        {
            switch (collectible.EquipType)
            {
                case (Collectible.equipType.harpoon):
                    // If the item is a harpoon, reset the equipped harpoon to "none"
                    currentHarpoonEquipped = "none";
                    break;

            }
        }
    }

    // Swaps positions of two items in the inventory by their indices
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
        // Swap the items if both indices are found
        InventoryItem Temp = Inventory[indexOf2];
        Inventory[indexOf2] = Inventory[indexOf1];
        Inventory[indexOf1] = Temp;
    }

    // Checks if a specific item is in the inventory
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

    // Gets the count of a specific item in the inventory
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

    // Returns the current inventory list
    public List<InventoryItem> getInventory()
    {
        return Inventory;
    }

    // Sets if fists are equipped
    public void setFistsEquipped(bool fistsEquipped)
    {
        FistsEquipped = fistsEquipped;
    }

    // Sets if the harpoon is equipped
    public void setHarpoonEquipped(bool harpoonEquipped)
    {
        HarpoonEquipped = harpoonEquipped;
    }

    // Prints the names of all items in the inventory to the console
    public void printKeys()
    {
        foreach (InventoryItem collected in Inventory)
        {
            Debug.Log(collected.getCollectible().Name);
        }
    }
}
