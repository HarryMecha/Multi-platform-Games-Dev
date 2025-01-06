using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    #region Fields
    private int maxHealth; // Public variable to set player's health in the inspector

    private int currentHealth; // Current health of the player

    public Health_Bar healthBar;

    private GameObject healthBarGO;

    [SerializeField] private GameObject replace;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        EnviromentManager enviromentManager = GameObject.Find("EnviromentManager").GetComponent<EnviromentManager>();
        switch (enviromentManager.difficulty)
        {
            //will change based on current difficulty
            case (EnviromentManager.Difficulty.Easy):
                maxHealth = 10;
                break;
            case (EnviromentManager.Difficulty.Hard):
                maxHealth = 20;
                break;
        }
        // Set player's health to the maximum at the start
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBarGO = healthBar.gameObject;
    }

    // Function to deal damage to the player
    public void TakeDamage(int damageAmount)
    {
        // Reduce current health by damage amount
        currentHealth -= damageAmount;
        healthBar.SetHealth(currentHealth);

        Debug.Log("Enemy took damage. Current Health: " + currentHealth);

        // Check if the player health has reached zero or below
        if (currentHealth <= 0)
        {
            Die(); // Call the Die function if health is 0 or less
        }
    }

    // Function to handle player death
    private void Die()
    {
        Debug.Log("Enemy died.");

        if (replace != null)
        {
            replace.SetActive(true);
        }

        // Destory the enemy
        Destroy(gameObject);
    }
    
    // will show health bar attached to object when enemy is looked at
    public void showHealthBar()
    {
        if (!healthBarGO.activeSelf)
        {
            healthBarGO.SetActive(true);
        }
    }

    //will hide health bar attached to object when player looks away
    public void hideHealthBar()
    {
        healthBarGO.SetActive(false);
    }

}
