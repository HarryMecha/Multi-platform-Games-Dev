using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BlastTrap : MonoBehaviour
{
    // Variables for references
    [Header("References")]
    [SerializeField] private Transform playerObject; // Reference to the player's Transform component

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

    private bool playerInTriggerRange;  // Is the player within the trigger range
    private Vector3 targetPosition;     // Current target position while roaming
    private Vector3 movementCenter;     // The center of the movement area
    private Collider explosionCollider; // Refrence to the collider on the object

    // Start is called before the first frame update
    void Start()
    {   
        // Assign spawn postion of the object as the center of the movement area
        movementCenter = transform.position;

        // Get collider component attached to this GameObject
        explosionCollider = GetComponent<Collider>();

        // Disable the collider on start
        explosionCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for trigger range
        if (Vector3.Distance(playerObject.position, transform.position) <= interactRadius) playerInTriggerRange = true;

        RandomMovement();

        // If the player is trigger range, explode the object else move randomly
        if (playerInTriggerRange)
        {
            // Enable the collider
            explosionCollider.enabled = true;

            // Invoke the explode method after some delay
            Invoke(nameof(Explode), explodeDelay);
        }
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
        // Set a new random tagert
        targetPosition = movementCenter + new Vector3(Random.Range(-movementRange, movementRange), Random.Range(-2, 2), Random.Range(-movementRange, movementRange));
    }

    private void OnDrawGizmos()
    {
        // Set Gizmo color
        Gizmos.color = Color.red;

        // Draw a wireframe sphere at the object's position with the specified radius
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
