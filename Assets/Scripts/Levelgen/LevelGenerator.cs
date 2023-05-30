using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject startPrefab;    // Start prefab game object
    public GameObject[] leftPrefabs;   // Array of left prefabs game objects
    public GameObject[] rightPrefabs;  // Array of right prefabs game objects
    public GameObject[] straightPrefabs; // Array of straight prefabs game objects
    public GameObject endPrefab;      // End prefab game object

    public int numberOfPrefabs = 10;  // Number of regular prefabs to generate

    private GameObject previousPrefab; // Previous prefab
    private List<Transform> _transformsList; // List to store transforms in order

    private void Start()
    {
        GenerateLevel();

        GameManager.Instance.MinecartWaypoints = _transformsList;
    }

    private void GenerateLevel()
    {
        _transformsList = new List<Transform>();

        // Generate the start of the level
        GameObject startObj = Instantiate(startPrefab, transform.position, Quaternion.identity);
        previousPrefab = startObj;
        startObj.transform.parent = transform;
        AddTransformsToList(startObj);

        // Initialize the initial direction to go straight
        int direction = 0; // 0 for straight, 1 for right, -1 for left

        // Probability factor for going straight
        float straightProbability = 0.7f; // Adjust this value to increase or decrease the likelihood of going straight

        bool Isright = false;
        bool IsLeft = false;

        // Generate the middle prefabs
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            GameObject prefab;

            if (direction == 0 || Random.value < straightProbability || Isright && direction == 1 || IsLeft && direction == -1)
            {
                // Go straight
                prefab = Instantiate(straightPrefabs[Random.Range(0, straightPrefabs.Length)], Vector3.zero, Quaternion.identity);
                direction = Random.Range(-1, 2);
            }
            else if (direction == 1)
            {
                // Turn right
                prefab = Instantiate(rightPrefabs[Random.Range(0, rightPrefabs.Length)], Vector3.zero, Quaternion.identity);
                direction = Random.Range(-1, 2);
                Isright = true;
                IsLeft = false;

            }
            else
            {
                // Turn left
                prefab = Instantiate(leftPrefabs[Random.Range(0, leftPrefabs.Length)], Vector3.zero, Quaternion.identity);
                direction = Random.Range(-1, 2);
                IsLeft = true;
                Isright = false;
            }

            AlignEntranceToExit(prefab, previousPrefab);
            prefab.transform.parent = transform;
            AddTransformsToList(prefab);

            previousPrefab = prefab;
        }

        // Generate the end of the level
        GameObject endObj = Instantiate(endPrefab, Vector3.zero, Quaternion.identity);
        AlignEntranceToExit(endObj, previousPrefab);
        endObj.transform.parent = transform;
        AddTransformsToList(endObj);

        // Print the transforms in order
        foreach (Transform t in _transformsList)
        {
            Debug.Log(t.name);
        }
    }

    private void AlignEntranceToExit(GameObject prefab, GameObject previousPrefab)
    {
        prefab.transform.rotation = previousPrefab.transform.Find("ExitPoint").transform.rotation;

        Transform entrance = prefab.transform.Find("EntrancePoint");
        Transform previousExit = previousPrefab.transform.Find("ExitPoint");

        Vector3 entranceOffset = entrance.position - prefab.transform.position;
        Vector3 exitOffset = previousExit.position - previousPrefab.transform.position;

        prefab.transform.position = previousExit.position - entranceOffset;
    }

    private void AddTransformsToList(GameObject prefab)
    {
        Transform entrance = prefab.transform.Find("EntrancePoint");
        Transform middle = prefab.transform.Find("MiddlePoint");
        Transform exit = prefab.transform.Find("ExitPoint");

        if (entrance != null)
        {
            _transformsList.Add(entrance);
        }

        if (middle != null)
        {
            _transformsList.Add(middle);
        }

        if (exit != null)
        {
            _transformsList.Add(exit);
        }
    }

    private void OnDrawGizmos()
    {
        if (_transformsList != null)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < _transformsList.Count - 1; i++)
            {
                Transform currentTransform = _transformsList[i];
                Transform nextTransform = _transformsList[i + 1];
                Gizmos.DrawLine(currentTransform.position, nextTransform.position);
            }
        }
    }




}
