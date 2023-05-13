using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Ore", menuName = "Ore/BaseOre")]
public class BaseOre : ScriptableObject
{
    public int HitPoint = 3;
    public Ores OreType = Ores.Coal;
    public GameObject OreVeinModel;
    public GameObject OreChunkPrefab;

    public int Hit(int currentHitpoints ,float value = 0)
    {
        return currentHitpoints -= (int)value;
    }

    public void Break(Vector3 oreChunkSpawnPosition)
    {
        Instantiate(OreChunkPrefab, oreChunkSpawnPosition, Quaternion.identity);

        Debug.Log($"{OreType.ToString()} breaks");
    }
}
