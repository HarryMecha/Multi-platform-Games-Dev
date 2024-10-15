using UnityEngine;
using UnityEngine.AI;

// Ensures that a NavMesh component is present on the same GameObject.
[RequireComponent(typeof(NavMesh))]
public class WaypointNavigation : MonoBehaviour
{
    [Header("Path Refrence")]
    // Reference to the WaypointNetwork that contains the list of waypoints to navigate through
    [SerializeField] private WaypointNetwork networkPath;


    [SerializeField] private Transform playerObject;

    // Keeps track of the current index in the list of waypoints
    private int currentIndex = 0;

    // Reference to the NavMeshAgent component
    private NavMeshAgent navAgent;

    // Start is called before the first frame update
    private void Start()
    {
        // Get the NavMeshAgent component attached to the GameObject
        navAgent = GetComponent<NavMeshAgent>();

        playerObject = GetComponent<Transform>();
        // Check if the WaypointNetwork is assigned. If not, exit the method
        if (networkPath == null) return;

        // Set the first destination in the waypoint network
        SetNextDestination(false);
    }

    // Update is called once per frame
    private void Update()
    {
        // If the agent has reached its destination and is not calculating a new path, move to the next waypoint
        if (!navAgent.hasPath && !navAgent.pathPending)
            SetNextDestination (true);

        // If the agent's path is considered invalid, reset the destination to the current waypoint
        else if (navAgent.isPathStale)
            SetNextDestination (false);
    }

    // Sets the agent's next destination, either by incrementing to the next waypoint or staying at the current one
    private void SetNextDestination(bool increment)
    {
        // If no networkPath is assigned, exit the method
        if (!networkPath) return;

        // Determine whether to increment to the next waypoint
        int incStep = increment ? 1 : 0;
        Transform nextWaypointTransform = null;

        // Calculate the next waypoint index. If the index exceeds the list, wrap around to the first one
        int nextWaypoint = (currentIndex + incStep >= networkPath.Waypoints.Count) ? 0 : currentIndex + incStep;
        nextWaypointTransform = networkPath.Waypoints[nextWaypoint];

        // If the next waypoint is valid, set the destination for the NavMeshAgent and update the current index
        if (nextWaypointTransform != null)
        {
            currentIndex = nextWaypoint;
            navAgent.destination = nextWaypointTransform.position;
            return;
        }

        // Increment the current index if no valid waypoint is found
        currentIndex++;
    }
}
