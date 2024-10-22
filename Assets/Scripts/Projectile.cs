using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hits the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Health system implementation

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