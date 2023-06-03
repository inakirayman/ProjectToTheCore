using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{



    public static GameManager Instance { get; private set; }
    private Dictionary<OreType, int> _collectedOres;




    //Settings
    public bool HasGameStarted = false;


    #region UI Settings
    [SerializeField]
    private EventSystem _eventSystem;

    #region GameOver
    [SerializeField]
    private GameObject _gameOverScreen;


    #endregion

    #region Win
    [SerializeField]
    private GameObject _winScreen;
    public bool LevelFinished = false;
    #endregion

    #region Gate
    [SerializeField]
    private GameObject _gateProgressBarObject;
    private ProgressBar _gateProgressBar;
    private float _gateProgress = 0;
    private float _gateIncreaseRate = 100f / 60f;
    public bool IsGateCharging = false; 
    public float GateProgress
    {
        get
        {
            return _gateProgress;
        }
        set
        {
            _gateProgress = value;
            _gateProgressBar.Current = _gateProgress;
        }
    }
    #endregion 

    #region Fuel
    [SerializeField]
    private float _fuelValue = 15;
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
    #endregion

    #region Light
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
    private bool _isLightRecharging = false;
    #endregion

    #region Health
    [SerializeField, Range(0f, 10f)]
    private float _minecartHealth = 10f;
    public float MinecartHealth
    {
        get
        {
            return _minecartHealth;
        }
        set
        {
            _minecartHealth = Mathf.Max(0, value);
            UIManager.Instance.HealthBar.Current = _minecartHealth;
        } 
        
    }
    #endregion

    #endregion

    public List<Transform> MinecartWaypoints;
    public bool IsMinecartDriving = false;
    public GameObject Minecart;

    public EnemySpawner EnemySpawner;
    public float SpawnDelay = 6;
    private float _spawnTimer =0;
    public Vector2 EnemySpawnAmountRange = new Vector2(2, 5);


    private AudioSource _audioSource;
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
        _audioSource = GetComponent<AudioSource>();
        _gateProgressBar = _gateProgressBarObject.GetComponent<ProgressBar>();



    }


    private void Start()
    {
        MinecartHealth = _minecartHealth;
        MinecartFuel = _minecartFuel;
        LightEnergy = _lightEnergy;

    }


    private void Update()
    {

        if(MinecartHealth <= 0)
        {
            _gameOverScreen.SetActive(true);
            _eventSystem.SetSelectedGameObject(_gameOverScreen.transform.Find("Menu").Find("Restart Button").gameObject);
            Time.timeScale = 0f;
        }



        //Check if there is place to add fuel.
        if (_collectedOres.ContainsKey(OreType.Coal) && MinecartFuel + _fuelValue <= 100 && _collectedOres[OreType.Coal] >= 1)
        {
            RemoveCollectedOre(OreType.Coal);
            MinecartFuel += _fuelValue;

        }

        if (_isLightRecharging)
        {
            SpawnEnemys();
        }
    

        if (!IsGateCharging)
        {
            LightLogic();
        }
        else
        {
            GateLogic();
        }


       
    }

    private void GateLogic()
    {
       

        if(GateProgress >= 100)
        {
            Win();
        }
        else
        {
            
            SpawnEnemys();
            _gateProgressBarObject.SetActive(true);
            LightEnergy = 0;
            GateProgress += _gateIncreaseRate * Time.deltaTime;
        }


    }


   private void Win()
   {
        _winScreen.SetActive(true);
        _eventSystem.SetSelectedGameObject(_winScreen.transform.Find("Menu").Find("Continue Button ").gameObject);
        Time.timeScale = 0f;
        _gateProgressBarObject.SetActive(false);
        LevelFinished = true;
   }

    private void OreUpdateUI()
    {
        
        if (_collectedOres.ContainsKey(OreType.Coal))
        {
            int value = _collectedOres[OreType.Coal];
            UIManager.Instance.CoalOreAmount.text = value.ToString();
        }
        else
        {
            UIManager.Instance.CoalOreAmount.text = "0";
        }

        if (_collectedOres.ContainsKey(OreType.Iron))
        {
            int value = _collectedOres[OreType.Iron];
            UIManager.Instance.IronOreAmount.text = value.ToString();
        }
        else
        {
            UIManager.Instance.IronOreAmount.text = "0";
        }

        if (_collectedOres.ContainsKey(OreType.Gold))
        {
            int value = _collectedOres[OreType.Gold];
            UIManager.Instance.GoldOreAmount.text = value.ToString();
        }
        else
        {
            UIManager.Instance.GoldOreAmount.text = "0";
        }

    }

    private void LightLogic()
    {
        if (!HasGameStarted)
        {
            return;
        }


        if (_isLightRecharging)
        {
            if (LightEnergy < 100f)
            {
                float energyIncrease = _energyIncreaseRate * Time.deltaTime;
                LightEnergy = Mathf.Min(100f, LightEnergy + energyIncrease);
            }
            else
            {
                _isLightRecharging = false;
            }
        }
        else
        {
            if (LightEnergy > 0f)
            {
                float energyDecrease = _energyDecreaseRate * Time.deltaTime;
                LightEnergy = Mathf.Max(0f, LightEnergy - energyDecrease);
                _spawnTimer = 0;
              
            }
            else
            {
                _audioSource.Play();
                IsMinecartDriving = false;
                _isLightRecharging = true;
            }
        }



    }

    private void SpawnEnemys()
    {
        // Increment the spawn timer
        _spawnTimer += Time.deltaTime;

        // Check if it's time to spawn enemies
        if (_spawnTimer >= SpawnDelay)
        {
            // Reset the spawn timer
            _spawnTimer = 0f;

            // Determine the number of enemies to spawn within the specified range
            int numberOfEnemies = UnityEngine.Random.Range((int)EnemySpawnAmountRange.x, (int)EnemySpawnAmountRange.y + 1);

            // Call the SpawnEnemiesInRandomArea method from the EnemySpawner
            for (int i = 0; i < numberOfEnemies; i++)
            {
                EnemySpawner.SpawnEnemiesInRandomArea();
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

        OreUpdateUI();
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

        OreUpdateUI();
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
        if (context.performed && !_isLightRecharging)
        {
            IsMinecartDriving = !IsMinecartDriving;
            HasGameStarted = true;
        }
    }
    #endregion


}