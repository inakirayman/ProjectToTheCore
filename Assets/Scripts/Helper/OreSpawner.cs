using UnityEngine;

public class OreSpawner : MonoBehaviour
{
  
    public GameObject CoalOreVeinPrefab; // Prefab of the coal ore vein
    public GameObject GoldOreVeinPrefab; // Prefab of the gold ore vein
    public GameObject IronOreVeinPrefab; // Prefab of the iron ore vein

    public int CoalOreVeinCount; // Number of coal ore vein spawns
    public int GoldOreVeinCount; // Number of gold ore vein spawns
    public int IronOreVeinCount; // Number of iron ore vein spawns

    public float SpawnAreaWidth; // Width of the spawn area along the X-axis
    public float SpawnAreaHeight; // Height of the spawn area along the Z-axis
    public float MinDistance; // Minimum distance between the spawned objects

    private int _maxAttempts = 100; // Maximum number of attempts to find a valid spawn position

    void Start()
    {
        SpawnObjects(CoalOreVeinPrefab, CoalOreVeinCount);
        SpawnObjects(GoldOreVeinPrefab, GoldOreVeinCount);
        SpawnObjects(IronOreVeinPrefab, IronOreVeinCount);
    }

    void SpawnObjects(GameObject prefab, int count)
    {
        Quaternion spawnRotation = transform.rotation;
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < _maxAttempts; j++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();

                if (IsValidSpawnPosition(spawnPosition))
                {
                    GameObject spawnedObject = Instantiate(prefab, spawnPosition, spawnRotation);
                    spawnedObject.transform.parent = transform;
                    // Do something with the spawned object if needed
                    break;
                }
            }
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-SpawnAreaWidth / 2f, SpawnAreaWidth / 2f);
        float randomZ = Random.Range(-SpawnAreaHeight / 2f, SpawnAreaHeight / 2f);

        // Get the forward direction of the object
        Vector3 spawnDirection = transform.forward;

        // Calculate the spawn position based on the object's position, direction, and random values
        Vector3 spawnPosition = transform.position + spawnDirection * randomZ + transform.right * randomX;

        return spawnPosition;
    }

    bool IsValidSpawnPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, MinDistance);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Ore"))
            {
                return false; // A spawned object is too close, so the position is not valid
            }
        }

        return true;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(SpawnAreaWidth, 0f, SpawnAreaHeight));
    }


}
