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
    [SerializeField] private float patrollRange; //

    private BaseState currentState; // Reference to state managers
    private Animator animator;      // Reference to animator component to handle animation
    private NavMeshAgent agent;     // Reference to the NavMeshAgent component to handle movement

    private Vector3 walkPoint;        //
    private bool waypointSet = false; //

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentState = new IdleState(this);
        currentState.Enter();
    }

    private void Update()
    {
        currentState.Update();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public bool playerInSightRange()
    {
        if (Vector3.Distance(playerObject.position, transform.position) <= sightRange) return true;
        else return false;
    }

    public bool playerInAttackRange()
    {
        if (Vector3.Distance(playerObject.position, transform.position) <= attackRange) return true;
        else return false;
    }

    public void SetMotionSpeed(float speed)
    {
        animator.SetBool("isAttack", false);
        animator.SetFloat("Speed", speed);
    }

    public void SetDeadAnimation(bool value)
    {
        animator.SetBool("isDead", value);
    }

    public void Dead(float time)
    {
        if(time >= 5f) Destroy(gameObject);
    }

    public void Patrolling()
    {
        animator.SetFloat("Speed", 0.5f);
        agent.speed = patrollSpeed;

        if (!waypointSet) SearchWaypoint();
        else agent.SetDestination(walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < 1f) waypointSet = false;
    }

    public void SearchWaypoint()
    {
        walkPoint = transform.position + new Vector3(Random.Range(-patrollRange, patrollRange),0,0);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, ground) && Vector3.Distance(walkPoint, transform.position) >= 6) waypointSet = true;
    }

    public void Chasing()
    {
        agent.SetDestination(playerObject.position);
        agent.speed = chaseSpeed;
    }

    public void Attacking()
    {
        animator.SetBool("isAttack", true);
        agent.velocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireCube(transform.position, new Vector3(patrollRange, 1, 5));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
