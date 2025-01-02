using UnityEngine;

public class SpawnFishes : MonoBehaviour
{
    // Variables for spawn area
    [Header("Spawn Area")]
    [SerializeField] private int fishesDensity;
    [SerializeField] public float clusterRangeHorizontal;
    [SerializeField] public float clusterRangeVertical;
    [SerializeField] private float minSpawnDistance;

    [SerializeField] private GameObject blastTrapsPrefab;

    // Update is called once per frame
    private void Start()
    {
        // ystem time-based unique seed
        Random.InitState(System.Environment.TickCount);

        for (int i = 0; i < fishesDensity; i++)
        {
            AssignValues:
            Vector3 spwanPosition = transform.position + new Vector3(Random.Range(-clusterRangeHorizontal, clusterRangeHorizontal), Random.Range(-clusterRangeVertical, clusterRangeVertical), Random.Range(-clusterRangeHorizontal, clusterRangeHorizontal));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(-10, 10), Random.Range(0, 360), 0);

            if (Vector3.Distance(spwanPosition, blastTrapsPrefab.transform.position) <= minSpawnDistance)
                goto AssignValues;

            GameObject blastTraps = Instantiate(blastTrapsPrefab, spwanPosition, spawnRotation);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(2 * clusterRangeHorizontal, 2 * clusterRangeVertical, 2 * clusterRangeHorizontal));
    }
}
