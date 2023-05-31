using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Dictionary<OreType, int> _collectedOres;




    //Settings
    [SerializeField]
    private float _fuelValue = 10;

    [SerializeField,Range(0f,100f)]
    private float _minecartFuel;
    public float MinecartFuel
    {
        get { return _minecartFuel; }
        set
        {
            _minecartFuel = Mathf.Max(0, value); // Ensure the fuel value doesn't go below zero
            UIManager.Instance.FuelBar.Current = _minecartFuel; // Update the UI element
        }
    }

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

        _collectedOres = new Dictionary<OreType, int>();


    }


    private void Update()
    {
        //Check if there is place to add fuel.
        if (_collectedOres.ContainsKey(OreType.Coal) && MinecartFuel + _fuelValue <= 100 && _collectedOres[OreType.Coal] >= 1)
        {
            RemoveCollectedOre(OreType.Coal);
            MinecartFuel += _fuelValue;

        }






        UpdateUI();
    }

    private void UpdateUI()
    {
        
    }

    public void AddCollectedOre(OreType ore)
    {
        

        if (_collectedOres.ContainsKey(ore))
        {
            _collectedOres[ore]++;
        }
        else
        {
            _collectedOres.Add(ore, 1);
        }
    }

    public void RemoveCollectedOre(OreType ore, int amount = 1)
    {

        if (_collectedOres.ContainsKey(ore))
        {
            if (_collectedOres[ore] > 1)
            {
                _collectedOres[ore]--;
            }
            else
            {
                _collectedOres.Remove(ore);
            }
        }
    }


    public int GetCollectedOreCount(OreType ore)
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