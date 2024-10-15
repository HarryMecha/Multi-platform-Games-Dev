using System.Collections.Generic;
using UnityEngine;

public class WaypointNetwork : MonoBehaviour
{
    // A public list of Transforms that will store the waypoints for the network path
    [Header("Way Points List")]
    public List<Transform> Waypoints = new List<Transform>();
}
