using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public Transform pathTilesParent;
    public Transform junctionTilesParent;
    public GameObject pathStartTile; // Treat as junction regarding removing tiles.
    private List<GameObject> pathTiles;
    private List<GameObject> junctionTiles;

    void Start()
    {
        pathTiles = new List<GameObject>();
        junctionTiles = new List<GameObject>();
        if(pathStartTile) junctionTiles.Add(pathStartTile);
    }

    public void StartNewPath()
    {
        foreach (Transform child in pathTilesParent)
        {
            Destroy(child.gameObject);
        }

        pathTiles.Clear();

        if (junctionTiles.Count > 1)
        {
            Destroy(junctionTiles[0]);
            junctionTiles.RemoveAt(0);
        }
    }

    public void AddToPath(GameObject obj)
    {
        pathTiles.Add(obj);
        obj.transform.SetParent(pathTilesParent);
    }

    public void AddJunction(GameObject obj)
    {
        junctionTiles.Add(obj);
        obj.transform.SetParent(junctionTilesParent);
    }
}
