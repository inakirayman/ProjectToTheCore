using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{



    public static GameManager Instance { get; private set; }
    private Dictionary<OreType, int> _collectedOres;




    //Settings
    public bool HasGameStarted = false;
    


    [SerializeField]
    private float _fuelValue = 10;

    [SerializeField,Range(0f,100f)]
    private float _minecartFuel;
    public float MinecartFuel
    {
        get 
        { 
            return _minecartFuel; 
        }
        set
        {
            _minecartFuel = Mathf.Max(0, value);
            UIManager.Instance.FuelBar.Current = _minecartFuel; // Update the UI element
        }
    }

    [SerializeField, Range(0f, 100f)]
    private float _lightEnergy;
    public float LightEnergy
    {
        get 
        { 
            return _lightEnergy; 
        }
        set
        {
            _lightEnergy = Mathf.Max(0, value);
            UIManager.Instance.LightBar.Current = _lightEnergy;
        }
    }
    private float _energyIncreaseRate = 100f / 30f;
    private float _energyDecreaseRate = 100f / 45f;
    private bool _isRecharging = false;

    [Range(0f, 10f)]
    public float MinecartHealth = 10f;

    public List<Transform> MinecartWaypoints;
    public bool IsMinecartDriving = false;

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


    private void Start()
    {
        MinecartFuel = _minecartFuel;
        LightEnergy = _lightEnergy;
    }


    private void Update()
    {
        //Check if there is place to add fuel.
        if (_collectedOres.ContainsKey(OreType.Coal) && MinecartFuel + _fuelValue <= 100 && _collectedOres[OreType.Coal] >= 1)
        {
            RemoveCollectedOre(OreType.Coal);
            MinecartFuel += _fuelValue;

        }






        LightLogic();
    }

    private void LightLogic()
    {
        if (!HasGameStarted)
        {
            return;
        }


        if (_isRecharging)
        {
            if (LightEnergy < 100f)
            {
                float energyIncrease = _energyIncreaseRate * Time.deltaTime;
                LightEnergy = Mathf.Min(100f, LightEnergy + energyIncrease);
            }
            else
            {
                _isRecharging = false;
            }
        }
        else
        {
            if (LightEnergy > 0f)
            {
                float energyDecrease = _energyDecreaseRate * Time.deltaTime;
                LightEnergy = Mathf.Max(0f, LightEnergy - energyDecrease);
            }
            else
            {
                IsMinecartDriving = false;
                _isRecharging = true;
            }
        }



    }

    




    #region OreLogic
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
    #endregion


    #region Inputs
    public void ToggleMinecartDriving(InputAction.CallbackContext context)
    {
        if (context.performed && !_isRecharging)
        {
            IsMinecartDriving = !IsMinecartDriving;
            HasGameStarted = true;
        }
    }
    #endregion


}