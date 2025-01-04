using UnityEngine;

public class SpawnFishes : MonoBehaviour
{
    // Variables for spawn area configurations
    [Header("Spawn Area")]
    [SerializeField] private int fishesDensity;           // Number of fish (or objects) to spawn
    [SerializeField] public float clusterRangeHorizontal; // Horizontal range within which objects can spawn
    [SerializeField] public float clusterRangeVertical;   // Vertical range within which objects can spawn
    [SerializeField] private float minSpawnDistance;      // Minimum distance from blast traps to spawn new objects

    [SerializeField] private GameObject blastTrapsPrefab; // Prefab of the object to spawn

    // Update is called once per frame
    private void Start()
    {
        // system time-based unique seed
        Random.InitState(System.Environment.TickCount);

        // Loop to spawn the specified number of objects
        for (int i = 0; i < fishesDensity; i++)
        {
        AssignValues: // Label to allow re-evaluating spawn position if conditions aren't met

            // Assign Random x,y,z positions within the defiend horizontal and vertical range
            Vector3 spwanPosition = transform.position + new Vector3(Random.Range(-clusterRangeHorizontal, clusterRangeHorizontal), Random.Range(-clusterRangeVertical, clusterRangeVertical), Random.Range(-clusterRangeHorizontal, clusterRangeHorizontal));

            // Assign Random rotaion around x and y axes
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(-10, 10), Random.Range(0, 360), 0);

            // If the calculated spawn position is too close to another blast trap's position, retry
            if (Vector3.Distance(spwanPosition, blastTrapsPrefab.transform.position) <= minSpawnDistance)
                goto AssignValues;

            // Instantiate the object at the generated position and rotation
            GameObject blastTraps = Instantiate(blastTrapsPrefab, spwanPosition, spawnRotation);
        }
    }

    // Visualize the spawn area in the Unity Editor for debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(2 * clusterRangeHorizontal, 2 * clusterRangeVertical, 2 * clusterRangeHorizontal));
    }
}