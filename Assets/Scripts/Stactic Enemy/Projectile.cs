using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile hits the player or the ground
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
        {
            // Health system implementation

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
