using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    // Variables for references
    [Header("References")]
    [SerializeField] private Transform playerObject;      // Reference to the player's Transform component
    [SerializeField] private GameObject projectilePrefab; // Reference to the projectile prefab
    [SerializeField] private Transform throwProjectile;   // Reference to the poistion of Throw Projectile point

    // Variables for attacking behavior
    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks; // Time delay between attacks
    [SerializeField] private float attackRange;        // Range within which the villain can attack the player
    [SerializeField] public float projectileSpeed;     // Speed of the projectile

    private bool alreadyAttacked = false; // Whether the villain has already attacked and is waiting for cooldown
    private bool playerInAttackRange;     // Is the player within the villain's attack range

    private void Start()
    {
        playerObject = GameObject.FindWithTag("Player").transform;
    }
    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        // Check for Attack Rang
        if (Vector3.Distance(playerObject.position, transform.position) <= attackRange) playerInAttackRange = true; 
        else playerInAttackRange = false;

        // If the player is in attack range, look at player and attack the player
        if (playerInAttackRange)
        {
            LookAtPlayer();
        }

        // If the player is not in attack range, reset the rotation after 1 second
        else if (!playerInAttackRange)
            Invoke(nameof(ResetRotation), 1f);
    }

    // Attack the player by firing projectile and managing the attack cooldown
    private void AttackPlayer()
    {
        // Ensure the attack is only triggered once during the cooldown
        if (!alreadyAttacked)
        {
            // Attack system
            ThrowProjectile();
            Debug.Log("projectile attacking");
            alreadyAttacked = true;

            // Reset the attack after the specified cooldown
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    // Resets the attack state, allowing the villain to attack again
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // Look at the player when player is in attack range
    private void LookAtPlayer()
    {
        // Calculate the direction from this object to the player
        Vector3 directionToPlayer = playerObject.position - transform.position;

        // Remove any vertical difference so the object only rotates on the Y axis
        directionToPlayer.y = 0;

        // Calculate the target rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Attack the player after rotation is completed
        Invoke(nameof(AttackPlayer), 1f);
    }

    // Reset the rotation when player is not in attack range
    private void ResetRotation()
    {
        // Initialize the target rotation to Quaternion identity (0, 0, 0)
        Quaternion targetRotation = Quaternion.identity;

        // Smoothly interpolate the rotation from the current rotation to the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);

        // Stop rotating when the rotation is almost equal to the target
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            // Snap to the target rotation to avoid floating-point precision issues
            transform.rotation = targetRotation;
        }
    }

    private void ThrowProjectile()
    {
        // Create the projectile at the GameObject's position
        GameObject projectile = Instantiate(projectilePrefab, throwProjectile.position, Quaternion.identity);

        // Get the direction to the player
        Vector3 directionToPlayer = (playerObject.position - transform.position).normalized;

        // Add velocity to the projectile in the direction of the player
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
            projectileRb.velocity = directionToPlayer * projectileSpeed;
    }

}