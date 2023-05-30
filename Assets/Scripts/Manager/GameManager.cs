using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Dictionary<BaseOre, int> _collectedOres;




    //Settings
    [SerializeField]
    private float _fuelValue = 10;
    [Range(0f, 100f)]
    public float LightEnergy = 100f;
    [Range(0f, 10f)]
    public float MinecartHealth = 10f;

    public List<Transform> MinecartWaypoints;

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

        _collectedOres = new Dictionary<BaseOre, int>();


    }



    public void AddCollectedOre(BaseOre ore)
    {
        if (ore.OreType == Ores.Coal)
        {
            IncreaseFuel();
            return;
        }


        if (_collectedOres.ContainsKey(ore))
        {
            _collectedOres[ore]++;
        }
        else
        {
            _collectedOres.Add(ore, 1);
        }
    }

    private void IncreaseFuel()
    {

        UIManager.Instance.FuelBar.Current += _fuelValue;



        Debug.Log("Fuel added");
    }

    public int GetCollectedOreCount(BaseOre ore)
    {
        if (_collectedOres.ContainsKey(ore))
        {
            return _collectedOres[ore];
        }
        else
        {
            return 0;
        }
    }
}