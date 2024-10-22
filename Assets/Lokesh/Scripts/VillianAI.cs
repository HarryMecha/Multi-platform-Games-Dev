using UnityEngine;
using UnityEngine.AI;

public class VillanAI : MonoBehaviour
{
    // Variables for references
    [Header("References")]
    [SerializeField] private Transform playerObject; // Reference to the player's Transform component
    [SerializeField] private LayerMask ground;       // Layer mask to identify what is considered the ground
    [SerializeField] private LayerMask player;       // Layer mask to identify the player

    // Variables for patrolling behavior
    [Header("Patrolling")]
    [SerializeField] private Vector3 walkPoint;    // Current target position for patrolling
    [SerializeField] private float walkPointRange; // Range within which the villain will randomly patrol
    [SerializeField] private float patrollSpeed;   // Speed at which the Villian moves while patrolling

    // Variables for attacking behavior
    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks; // Time delay between attacks
    [SerializeField] private float chaseSpeed;         // Speed at which the Villian moves while chasing

    // Variables for tracking different states
    [Header("States")]
    [SerializeField] private float sightRange;  // Range within which the villain can see the player
    [SerializeField] private float attackRange; // Range within which the villain can attack the player

    private bool walkPointSet = false;    // Whether a valid walk point has been set for patrolling
    private bool alreadyAttacked = false; // Whether the villain has already attacked and is waiting for cooldown
    private NavMeshAgent agent;           // Reference to the NavMeshAgent component to handle movement
    private bool playerInSightRange;      // Is the player within the villain's sight range
    private bool playerInAttackRange;     // Is the player within the villain's attack range

    // Start is called before the first frame update
    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Check for Sight Range and Attack Range
        if (Vector3.Distance(playerObject.position, transform.position) <= sightRange) playerInSightRange = true;
        else playerInSightRange = false;
        
        if (Vector3.Distance(playerObject.position, transform.position) <= attackRange) playerInAttackRange = true;
        else playerInAttackRange = false;

        // If the player is neither in sight nor attack range, continue patrolling
        if (!playerInSightRange && !playerInAttackRange)
            Patrolling();

        // If the player is in sight but not in attack range, chase the player
        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();

        // If the player is both in sight and attack range, attack the player
        if (playerInSightRange && playerInAttackRange)
            AttackPlayaer();
    }

    // Patrolling logic
    private void Patrolling()
    {
        //Set movemnt speed to patrolling speed
        agent.speed = patrollSpeed;

        // If no walk point has been set, search for a new random walk point
        if (!walkPointSet)
            SearchWalkPoint();

        // If a walk point is set, move towards it
        else
            agent.SetDestination(walkPoint);

        // Check the distance to the walk point, if close enough, reset the walk point
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    // Randomly searches for a valid walk point within a defined range
    private void SearchWalkPoint()
    {
        // Generate random x and z coordinates for the walk point
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        // Set the walk point relative to the villain's current position
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Check if the randomly selected walk point is on the ground using a raycast
        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground))
            walkPointSet = true;
    }

    // Chase the player by setting the agent's destination to the player's position
    private void ChasePlayer()
    {
        agent.SetDestination(playerObject.position);

        // Set movement speed to chase speed
        agent.speed = chaseSpeed;
    }

    // Attack the player by stopping the agent's movement and managing the attack cooldown
    private void AttackPlayaer()
    {
        agent.SetDestination(transform.position);

        LookAtPlayer();

        // Ensure the attack is only triggered once during the cooldown
        if (!alreadyAttacked)
        {
            //attack and health system implementation
            Debug.Log("attacking");

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

    private void LookAtPlayer()
    {
        // Calculate the direction from this object to the player
        Vector3 directionToPlayer = playerObject.position - transform.position;

        // Remove any vertical difference so the object only rotates on the Y axis (optional, depends on use case)
        directionToPlayer.y = 0;

        // Calculate the target rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}