using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hits the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Health system implementation
            GameObject EnviromentManager = GameObject.Find("EnviromentManager");
            EnviromentManager.GetComponent<PlayerManager>().TakeDamage(10);

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