using System;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    // Variables for references
    [Header("References")]
    [SerializeField] private Transform playerObject;      // Reference to the player's Transform component
    [SerializeField] private GameObject projectilePrefab; // Reference to the projectile prefab
    [SerializeField] private Transform throwProjectile;   // Reference to the poistion of Throw Projectile point

    // Variables for floating movement
    [Header("Movement")]
    [SerializeField] private float floatSpeed;     // Speed of the floating motion
    [SerializeField] private float floatAmplitude; // How far the object floats (amplitude)

    // Variables for attacking behavior
    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks; // Time delay between attacks
    [SerializeField] private float attackRange;        // Range within which the villain can attack the player
    [SerializeField] public float projectileForce;     // Speed of the projectile
    [SerializeField] public float launchAngle;         // The angle at which the projectile is launched

    private bool alreadyAttacked = false;        // Whether the villain has already attacked and is waiting for cooldown
    private bool playerInAttackRange;            // Is the player within the villain's attack range
    private Vector3 startPosition;               // Initial position of the object
    private Vector3 floatDirection = Vector3.up; // Direction of the floating motion

    void Start()
    {
        // Store the starting position of the object
        startPosition = transform.position;
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        EnemyIdleMovement();
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

        // Add velocity to the projectile in the direction of the player
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            Vector3 Velocity = LaunchVelocity();
            projectileRb.velocity = Velocity;
        }
    }

    private Vector3 LaunchVelocity()
    {
        // Calculate the displacement between the target and spawn point
        Vector3 displacement = playerObject.position - transform.position;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);

        // Calculate the horizontal distance and initial velocity
        float horizontalDistance = displacementXZ.magnitude;
        float verticalDistance = displacement.y;

        // Convert the launch angle to radians
        float angleRad = launchAngle * Mathf.Deg2Rad;

        // Calculate the initial velocity magnitude
        float initialVelocity = Mathf.Sqrt((Physics.gravity.magnitude * horizontalDistance * horizontalDistance) / (2 * Mathf.Cos(angleRad) * Mathf.Cos(angleRad) * (horizontalDistance * Mathf.Tan(angleRad) - verticalDistance)));

        // Calculate the velocity components
        Vector3 velocityXZ = displacementXZ.normalized * initialVelocity * Mathf.Cos(angleRad);
        Vector3 velocityY = Vector3.up * initialVelocity * Mathf.Sin(angleRad);

        // Combine the components to get the final velocity
        return velocityXZ + velocityY;
    }

    private void EnemyIdleMovement()
    {
        // Calculate the floating motion
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Apply the floating motion
        transform.position = startPosition + floatDirection.normalized * offset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}