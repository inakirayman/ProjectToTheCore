using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PathGenerator : MonoBehaviour
{
    public List<PathTile> LeftTiles;
    public List<PathTile> StraightTiles;
    public List<PathTile> RightTiles;
    public List<PathTile> JunctionTiles;

    public Transform PathStart;
    public int seed;

    //private int pathLengthRemaining = 0;
    //private int rotationBias = 0;
    private Transform lastPos;

    void Start()
    {
        Random.InitState(seed);
        lastPos = PathStart;
        //PathTile spawned = Instantiate(LeftTiles[0], nextPos.position, nextPos.rotation);
        //nextPos = spawned.EndTransform;
    }

    public void GenerateToJunction(int length)
    {
        GenerateToJunctionRecursive(length, lastPos, 0);
    }

    public void GenerateToJunctionRecursive(int lengthRemaining, Transform attachPoint, int rotationBias)
    {
        if (lengthRemaining > 0)
        {
            StartCoroutine(GenerationToJunctionCoroutine(lengthRemaining, attachPoint, rotationBias));
        }
        else
        {
            GenerateJunction();
        }
    }

    IEnumerator GenerationToJunctionCoroutine(int lengthRemaining, Transform attachPoint, int rotationBias)
    {
        yield return new WaitForSeconds(0.05f); // Wait for half a second
        float maxBias = 90f;
        float leftProbability = 1f + rotationBias / maxBias;
        float rightProbability = 1f - rotationBias / maxBias;
        float straightProbability = 1f;

        float remainingRotationLeft = 0f;
        float remainingRotationRight = 0f;

        remainingRotationLeft = -90f - rotationBias;
        remainingRotationRight = 90f - rotationBias;

        List<PathTile> filteredLeftTiles = LeftTiles.Where(pt => pt.Length <= lengthRemaining && pt.PathRotation >= remainingRotationLeft).ToList();
        if (filteredLeftTiles.Count == 0) leftProbability = 0f;
        List<PathTile> filteredStraightTiles = StraightTiles.Where(pt => pt.Length <= lengthRemaining).ToList();
        if (filteredStraightTiles.Count == 0) straightProbability = 0f;
        List<PathTile> filteredRightTiles = RightTiles.Where(pt => pt.Length <= lengthRemaining && pt.PathRotation <= remainingRotationRight).ToList();
        if (filteredRightTiles.Count == 0) rightProbability = 0f;

        float totalProbability = leftProbability + straightProbability + rightProbability;
        float rand = Random.Range(0f, totalProbability);

        PathTile selectedItem = null;

        if (rand < leftProbability)
        {
            selectedItem = filteredLeftTiles[Random.Range(0, filteredLeftTiles.Count)];
        }
        else if (rand < leftProbability + straightProbability)
        {
            selectedItem = filteredStraightTiles[Random.Range(0, filteredStraightTiles.Count)];
        }
        else
        {
            selectedItem = filteredRightTiles[Random.Range(0, filteredRightTiles.Count)];
        }

        PathTile spawnedTile = Instantiate(selectedItem, attachPoint.position, attachPoint.rotation);
        lastPos = spawnedTile.EndTransform;
        if ((lengthRemaining - spawnedTile.Length) > 0)
        {
            GenerateToJunctionRecursive(lengthRemaining - spawnedTile.Length, spawnedTile.EndTransform, rotationBias + spawnedTile.PathRotation);
        }
        else GenerateJunction();
        //GenerateToJunctionRecursive(lengthRemaining, );
    }

    public void GenerateJunction()
    {

    }

    [ContextMenu("Generate 1")]
    void GenSingle()
    {
        GenerateToJunction(1);
    }

    [ContextMenu("Generate 5")]
    void GenFive()
    {
        GenerateToJunction(5);
    }


    [ContextMenu("Generate 100")]
    void GenLots()
    {
        GenerateToJunction(100);
    }

    [ContextMenu("Generate 1000")]
    void GenVeryLots()
    {
        GenerateToJunction(1000);
    }
}
