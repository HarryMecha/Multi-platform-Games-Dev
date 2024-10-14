using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMesh))]
public class WaypointNavigation : MonoBehaviour
{
    [SerializeField] private WaypointNetwork networkPath;

    private int currentIndex = 0;
    private bool hasPath = false;
    private bool pathPending = false;

    private NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        if (networkPath == null) return;

        SetNextDestination(false);
    }

    private void SetNextDestination(bool increment)
    {
        if (!networkPath) return;

        int incStep = increment ? 1 : 0;
        Transform nextWaypointTransform = null;

        while (nextWaypointTransform == null)
        {
            int nextWaypoint = (currentIndex + incStep >= networkPath.Waypoints.Count) ? 0 : currentIndex + incStep;
            nextWaypointTransform = networkPath.Waypoints[nextWaypoint];

            if (nextWaypointTransform != null)
            {
                currentIndex = nextWaypoint;
                navAgent.destination = nextWaypointTransform.position;
                return;
            }
        }

        currentIndex++;
    }

    private void Update()
    {
        hasPath = navAgent.hasPath;
        pathPending = navAgent.pathPending;

        if (!hasPath && !pathPending)
            SetNextDestination (true);
    }
}
