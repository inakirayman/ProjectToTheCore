using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnArea
    {
        public Transform SpawnPoint;
        public float SpawnAreaWidth = 10f;
        public float SpawnAreaHeight = 5f;
        public Quaternion SpawnRotation
        {
            get
            {
                return SpawnPoint.rotation;
            }
        }
    }

    public GameObject EnemyPrefab;
    public SpawnArea SpawnAreaFront;
    public SpawnArea SpawnAreaBack;
    public SpawnArea SpawnAreaLeft;
    public SpawnArea SpawnAreaRight;

    public int NumberOfEnemies = 5;
    public LayerMask GroundLayerMask;
    public LayerMask CollisionLayerMask;
    public int MaxSpawnAttempts = 10;

    [ContextMenu("Spawn Enemy")]
    public void SpawnEnemiesInRandomArea()
    {
        List<SpawnArea> spawnAreas = new List<SpawnArea>
        {
            SpawnAreaFront,
            SpawnAreaBack,
            SpawnAreaLeft,
            SpawnAreaRight
        };

        int spawnedEnemies = 0;
        int attempts = 0;

        while (spawnedEnemies < NumberOfEnemies && attempts < 4)
        {
            SpawnArea spawnArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
            Vector3 spawnPosition = GetRandomSpawnPosition(spawnArea);
            Quaternion spawnRotation = spawnArea.SpawnRotation;

            if (spawnPosition != Vector3.zero)
            {
                GameObject enemy = Instantiate(EnemyPrefab, spawnPosition, spawnRotation);
                enemy.transform.parent = spawnArea.SpawnPoint;
                spawnedEnemies++;
            }
            spawnAreas.Remove(spawnArea);
            attempts++;
        }

        Debug.Log("Spawned " + spawnedEnemies + " enemies.");
    }

    public void SpawnEnemies(SpawnArea spawnArea)
    {
        int spawnedEnemies = 0;

        for (int i = 0; i < NumberOfEnemies; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition(spawnArea);
            Quaternion spawnRotation = spawnArea.SpawnRotation;

            if (spawnPosition != Vector3.zero)
            {
                GameObject enemy = Instantiate(EnemyPrefab, spawnPosition, spawnRotation);
                enemy.transform.parent = spawnArea.SpawnPoint;
                spawnedEnemies++;
            }
        }

        Debug.Log("Spawned " + spawnedEnemies + " enemies in the " + spawnArea.SpawnPoint.name);
    }

    private Vector3 GetRandomSpawnPosition(SpawnArea spawnArea)
    {
        bool isValidPosition = false;
        int spawnAttempts = 0;
        Vector3 spawnPosition = Vector3.zero;

        while (!isValidPosition && spawnAttempts < MaxSpawnAttempts)
        {
            float randomX = Random.Range(-spawnArea.SpawnAreaWidth / 2f, spawnArea.SpawnAreaWidth / 2f);
            float randomZ = Random.Range(-spawnArea.SpawnAreaHeight / 2f, spawnArea.SpawnAreaHeight / 2f);

            spawnPosition = spawnArea.SpawnPoint.position + spawnArea.SpawnPoint.rotation * new Vector3(randomX, 0f, randomZ);

            Collider[] colliders = Physics.OverlapSphere(spawnPosition, 1f, CollisionLayerMask);
            bool hasGroundBelow = Physics.Raycast(spawnPosition, Vector3.down, out RaycastHit hit, 10f, GroundLayerMask);

            if (colliders.Length == 0 && hasGroundBelow)
            {
                spawnPosition.y = hit.point.y;
                isValidPosition = true;
            }

            spawnAttempts++;
        }

        if (!isValidPosition)
        {
            spawnPosition = Vector3.zero;
        }

        return spawnPosition;
    }

    private void OnDrawGizmos()
    {
        DrawSpawnAreaGizmo(SpawnAreaFront);
        DrawSpawnAreaGizmo(SpawnAreaBack);
        DrawSpawnAreaGizmo(SpawnAreaLeft);
        DrawSpawnAreaGizmo(SpawnAreaRight);
    }

    private void DrawSpawnAreaGizmo(SpawnArea spawnArea)
    {
        Gizmos.color = Color.red;
        Vector3 spawnAreaCenter = spawnArea.SpawnPoint.position;
        Quaternion spawnAreaRotation = spawnArea.SpawnPoint.rotation;
        Vector3 spawnAreaSize = new Vector3(spawnArea.SpawnAreaWidth, 0f, spawnArea.SpawnAreaHeight);
        Gizmos.matrix = Matrix4x4.TRS(spawnAreaCenter, spawnAreaRotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, spawnAreaSize);
    }
}
