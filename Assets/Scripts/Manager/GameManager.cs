using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Dictionary<BaseOre, int> collectedOres;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        collectedOres = new Dictionary<BaseOre, int>();
    }

    public void AddCollectedOre(BaseOre ore)
    {
        if (ore.OreType == Ores.Coal)
        {
            IncreaseFuel();
            return;
        }


        if (collectedOres.ContainsKey(ore))
        {
            collectedOres[ore]++;
        }
        else
        {
            collectedOres.Add(ore, 1);
        }
    }

    private void IncreaseFuel()
    {
        Debug.Log("Fuel added");
    }

    public int GetCollectedOreCount(BaseOre ore)
    {
        if (collectedOres.ContainsKey(ore))
        {
            return collectedOres[ore];
        }
        else
        {
            return 0;
        }
    }
}