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

    // Variables for attacking behavior
    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks; // Time delay between attacks

    // Variables for tracking different states
    [Header("States")]
    [SerializeField] private float sightRange;  // Range within which the villain can see the player
    [SerializeField] private float attackRange; // Range within which the villain can attack the player

    private bool walkPointSet = false;    // Whether a valid walk point has been set for patrolling
    private bool alreadyAttacked = false; // Whether the villain has already attacked and is waiting for cooldown
    private NavMeshAgent agent;           // Reference to the NavMeshAgent component to handle movement
    private bool playerInSightRange;      // Is the player within the villain's sight range
    private bool playerInAttackRange;     // Is the player within the villain's attack range

    private void Start()
    {
        // Find the player's transform at the start of the game
        playerObject = GameObject.Find("Player").transform;

        // Get the NavMeshAgent component attached to this GameObject
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check for Sight Range and Attack Range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, player);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, player);

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
    }

    // Attack the player by stopping the agent's movement and managing the attack cooldown
    private void AttackPlayaer()
    {
        agent.SetDestination(transform.position);

        // Ensure the attack is only triggered once during the cooldown
        if (!alreadyAttacked)
        {
            //attack and health sytem implementation

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
}