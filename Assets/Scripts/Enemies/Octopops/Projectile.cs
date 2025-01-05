using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hits the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Health system implementation
            GameObject enviromentManagerObject = GameObject.Find("EnviromentManager");
            EnviromentManager enviromentManager = enviromentManagerObject.GetComponent<EnviromentManager>();
            switch (enviromentManager.difficulty)
            {
                case (EnviromentManager.Difficulty.Easy):
                    enviromentManagerObject.GetComponent<PlayerManager>().TakeDamage(10);
                    break;
                case (EnviromentManager.Difficulty.Hard):
                    enviromentManagerObject.GetComponent<PlayerManager>().TakeDamage(20);
                    break;
            }

            

            // Destroy the projectile
            Destroy(gameObject);
        }

        // Check if the projectile hits the ground
        else if (collision.gameObject.CompareTag("Ground"))
        {
            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}