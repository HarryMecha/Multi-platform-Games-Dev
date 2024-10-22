using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BlastTrap : MonoBehaviour
{
    // Variables for references
    [Header("References")]
    [SerializeField] private Transform playerObject; // Reference to the player's Transform component
    [SerializeField] private LayerMask player;       // Layer mask to identify the player

    // Variables for movment behavior
    [Header("Movement")]
    [SerializeField] private float swimSpeed;        // Speed of roaming movement
    [SerializeField] private float turnSpeed;        // Speed at which the fish turns
    [SerializeField] private float stoppingDistance; // Distance to stop near a target position
    [SerializeField] private float movementRange;    // Radius of the movement area

    // Variables for attacking behavior
    [Header("Attacking")]
    [SerializeField] private float interactRadius; // Radius in which objects will strat intreacting with player
    [SerializeField] private float explodeDelay;   // Time delay before exploding

    private NavMeshAgent agent;        // Reference to the NavMeshAgent component to handle movement
    private bool playerInTriggerRange; // Is the player within the trigger range
    private Vector3 targetPosition;    // Current target position while roaming
    private Vector3 movementCenter;    // The center of the movement area

    // Start is called before the first frame update
    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        agent = GetComponent<NavMeshAgent>();

        // Assign spawn postion of the object as the center of the movement area
        movementCenter = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for trigger range
        playerInTriggerRange = Physics.CheckSphere(transform.position, interactRadius, player);

        // If the player is trigger range, explode the object else move randomly
        if (playerInTriggerRange)
            Invoke(nameof(Explode), explodeDelay);
        else
            RandomMovement();

    }

    private void Explode()
    {
        Debug.Log("Object exploded!");

        // Destroy the explosive object
        Destroy(gameObject);
    }

    // This method handles random movement for the object.
    private void RandomMovement()
    {
        // Set a starting target
        SetNewTarget();

        // Calculate the direction from the current position to the target position.
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Rotate the object smoothly towards the target direction.
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

        // Move the object forward in the direction it is facing (after rotation) at swimSpeed.
        transform.position += transform.forward * swimSpeed * Time.deltaTime;

        // Check if the object is close enough to the target position and set a new target
        if (Vector3.Distance(transform.position, targetPosition) <= stoppingDistance)
            SetNewTarget();
    }

    // This method sets a new target position for the object to move towards.
    private void SetNewTarget()
    {
        // Check if the player is the range of the object, if not set a new random tagert else set player as a new target
        if (!playerInTriggerRange)
            targetPosition = movementCenter + new Vector3(Random.Range(-movementRange, movementRange), Random.Range(-2, 2), Random.Range(-movementRange, movementRange));
        else
            targetPosition = playerObject.position;
    }
}
