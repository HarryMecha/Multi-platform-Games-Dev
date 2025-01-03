using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Variables for references
    [Header("Refrences")]
    [SerializeField] private Transform playerObject; // Reference to the player's Transform component
    [SerializeField] private LayerMask ground;       // Layer mask to identify what is considered the ground

    // Variables for tracking different states
    [Header("Define Range")]
    [SerializeField] private float sightRange;  // Range within which the villain can see the player
    [SerializeField] private float attackRange; // Range within which the villain can attack the player

    // Variables for attacking behavior
    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks; // Time delay between attacks
    [SerializeField] private float chaseSpeed;         // Speed at which the Villian moves while chasing
    [SerializeField] private int damageAmount;         // Damage amount villian causes to the player

    // Variables for patrolling behavior
    [Header("Patrolling")]
    [SerializeField] private float patrollSpeed; // Speed at which the Villian moves while patrolling
    [SerializeField] private float patrollRange; // Patrolling Range of Villian

    private BaseState currentState; // The current state of the enemy
    private Animator animator;      // Animator component to control enemy animations
    private NavMeshAgent agent;     // NavMeshAgent component to handle enemy navigation

    private Vector3 walkPoint;            // Target point for patrolling
    private bool waypointSet = false;     // Flag to check if a waypoint has been set
    public bool waypointDirection = true; // Determines the direction of patrolling

    private void Awake()
    {
        // Initialize references
        playerObject = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Initialize the enemy's state to Idle
        currentState = new IdleState(this);
        currentState.Enter();
    }

    private void Update()
    {
        // Update the current state behavior every frame
        currentState.Update();
    }

    // Method to change the current state of the enemy
    public void ChangeState(BaseState newState)
    {
        currentState.Exit();     // Exit the current state
        currentState = newState; // Switch to the new state
        currentState.Enter();    // Enter the new state
    }

    // Check if the player is within sight range
    public bool playerInSightRange()
    {
        if (Vector3.Distance(playerObject.position, transform.position) <= sightRange) return true;
        else return false;
    }

    // Check if the player is within attack range
    public bool playerInAttackRange()
    {
        if (Vector3.Distance(playerObject.position, transform.position) <= attackRange) return true;
        else return false;
    }

    // Set the movement speed of the enemy and toggle the attack animation
    public void SetMotionSpeed(float speed)
    {
        animator.SetBool("isAttack", false);
        animator.SetFloat("Speed", speed);
    }

    // Trigger the dead animation
    public void SetDeadAnimation(bool value)
    {
        animator.SetBool("isDead", value);
    }

    // Destroy the enemy object after a given time
    public void Dead(float time)
    {
        if(time >= 5f) Destroy(gameObject);
    }

    // Handle the patrolling behavior
    public void Patrolling()
    {
        animator.SetFloat("Speed", 0.5f); // Set the patrolling animation
        agent.speed = patrollSpeed; // Set the patrolling speed

        if (!waypointSet) SearchWaypoint(); // Find a new waypoint if none is set
        else agent.SetDestination(walkPoint); // Move towards the set waypoint

        // Reset waypoint flag if close enough to the waypoint
        if (Vector3.Distance(transform.position, walkPoint) < 1f) waypointSet = false;
    }

    // Find a new waypoint for patrolling
    public void SearchWaypoint()
    {
        switch (waypointDirection)
        {
            // Move in the positive direction
            case true:
                walkPoint = transform.position + new Vector3(Random.Range(0, patrollRange), 0, 0);
                waypointDirection = false;
                if (Physics.Raycast(walkPoint, -transform.up, 2f, ground) && Vector3.Distance(walkPoint, transform.position) >= 6) waypointSet = true; // Check if the new waypoint is valid
                break;

            // Move in the negative direction
            case false:
                walkPoint = transform.position + new Vector3(Random.Range(-patrollRange, 0), 0, 0);
                waypointDirection = true;
                if (Physics.Raycast(walkPoint, -transform.up, 2f, ground) && Vector3.Distance(walkPoint, transform.position) >= 6) waypointSet = true; // Check if the new waypoint is valid
                break;
        }
    }

    // Handle the chasing behavior
    public void Chasing()
    {
        agent.SetDestination(playerObject.position); // Set the player's position as the destination
        agent.speed = chaseSpeed; // Adjust the speed for chasing
    }

    // Handle the attacking behavior
    public void Attacking()
    {
        animator.SetBool("isAttack", true); // Play the attack animation
        agent.velocity = Vector3.zero; // Stop the enemy's movement
    }

    // Visualize the sight and attack ranges in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireCube(transform.position, new Vector3(patrollRange, 1, 5));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
