using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PathGenerator : MonoBehaviour
{
    public PathManager Manager;
    
    public List<PathTilePath> LeftTiles;
    public List<PathTilePath> StraightTiles;
    public List<PathTilePath> RightTiles;
    public List<PathTileJunction> JunctionTiles;
    public List<PathTileExit> ExitTiles;
    public Transform PathStart;
    [Tooltip("0 = random seed")]
    public int seed;

    private Transform[] lastPos = {null, null};

    void Start()
    {
        if (seed != 0)
        {
            Random.InitState(seed);
        }
        lastPos[0] = PathStart;
    }

    public void GenerateToJunction(int length, int dir)
    {
        Manager.StartNewPath();
        Transform attachPoint;
        if (lastPos[1])
        {
            attachPoint = GeneratePathRecursively(length, lastPos[dir], 0);
        }
        else
        {
            attachPoint = GeneratePathRecursively(length, lastPos[0], 0);
        }
        GenerateJunction(attachPoint);
    }

    public void GenerateToExit(int length, int dir)
    {
        Manager.StartNewPath();
        Transform attachPoint;
        if (lastPos[1])
        {
            attachPoint = GeneratePathRecursively(length, lastPos[dir], 0);
        }
        else
        {
            attachPoint = GeneratePathRecursively(length, lastPos[0], 0);
        }
        GenerateExit(attachPoint);
    }

    public Transform GeneratePathRecursively(int lengthRemaining, Transform attachPoint, int rotationBias)
    {
        if (lengthRemaining > 0)
        {
            float maxBias = 90f;
            float leftProbability = 1f + rotationBias / maxBias;
            float rightProbability = 1f - rotationBias / maxBias;
            float straightProbability = 1f;

            float remainingRotationLeft = 0f;
            float remainingRotationRight = 0f;

            remainingRotationLeft = -90f - rotationBias;
            remainingRotationRight = 90f - rotationBias;

            List<PathTilePath> filteredLeftTiles = LeftTiles.Where(pt => pt.Length <= lengthRemaining && pt.PathRotation >= remainingRotationLeft).ToList();
            if (filteredLeftTiles.Count == 0) leftProbability = 0f;
            List<PathTilePath> filteredStraightTiles = StraightTiles.Where(pt => pt.Length <= lengthRemaining).ToList();
            if (filteredStraightTiles.Count == 0) straightProbability = 0f;
            List<PathTilePath> filteredRightTiles = RightTiles.Where(pt => pt.Length <= lengthRemaining && pt.PathRotation <= remainingRotationRight).ToList();
            if (filteredRightTiles.Count == 0) rightProbability = 0f;

            float totalProbability = leftProbability + straightProbability + rightProbability;

            if (totalProbability == 0f)
            {
                return attachPoint;
            }

            float rand = Random.Range(0f, totalProbability);

            PathTilePath selectedItem = null;

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

            PathTilePath spawnedTile = Instantiate(selectedItem, attachPoint.position, attachPoint.rotation);
            Manager.AddToPath(spawnedTile.gameObject);
            lastPos[0] = spawnedTile.EndTransform;
            lastPos[1] = null;
            return GeneratePathRecursively(lengthRemaining - spawnedTile.Length, spawnedTile.EndTransform, rotationBias + spawnedTile.PathRotation);
        }
        else
        {
            return attachPoint;
        }
    }

    public void GenerateJunction(Transform attachPoint)
    {
        PathTileJunction selectedItem = JunctionTiles[Random.Range(0, JunctionTiles.Count)];
        PathTileJunction spawnedTile = Instantiate(selectedItem, attachPoint.position, attachPoint.rotation);
        Manager.AddJunction(spawnedTile.gameObject);
        lastPos[0] = spawnedTile.EndTransformLeft;
        lastPos[1] = spawnedTile.EndTransformRight;
    }

    public void GenerateExit(Transform attachPoint)
    {
        PathTileExit selectedItem = ExitTiles[Random.Range(0, JunctionTiles.Count)];
        PathTileExit spawnedTile = Instantiate(selectedItem, attachPoint.position, attachPoint.rotation);
        Manager.AddJunction(spawnedTile.gameObject);
        lastPos[0] = spawnedTile.EndTransform;
        lastPos[1] = null;
    }

    [ContextMenu("Generate 50 to Junction")]
    void GenSingleToJunction()
    {
        GenerateToJunction(50, 0);
    }

    [ContextMenu("Generate 100 to Junction")]
    void GenFiveToJunction()
    {
        GenerateToJunction(100, 0);
    }

    [ContextMenu("Generate 500 to Junction")]
    void GenLotsToJunction()
    {
        GenerateToJunction(500, 0);
    }

    [ContextMenu("Generate 1000 to Junction")]
    void GenVeryLotsToJunction()
    {
        GenerateToJunction(1000, 0);
    }


    [ContextMenu("Generate 50 to Exit")]
    void GenSingleToExit()
    {
        GenerateToExit(50, 0);
    }

    [ContextMenu("Generate 100 to Exit")]
    void GenFiveToExit()
    {
        GenerateToExit(100, 0);
    }

    [ContextMenu("Generate 500 to Exit")]
    void GenLotsToExit()
    {
        GenerateToExit(500, 0);
    }

    [ContextMenu("Generate 1000 to Exit")]
    void GenVeryLotsToExit()
    {
        GenerateToExit(1000, 0);
    }
}
