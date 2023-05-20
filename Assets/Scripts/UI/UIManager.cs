using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }



    public ProgressBar FuelBar;
    public ProgressBar HealthBar;
    public ProgressBar LightBar;


  
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

    }
    public void SetFuelBarStartStats(float max, float current)
    {
        FuelBar.Maximum = max;
        FuelBar.Current = current;

    }

    public void SetHealthBarStartStats(float max,float current)
    {
        HealthBar.Maximum = max;
        HealthBar.Current = current;
    }

    public void SetLightBarStartStats(float max, float current)
    {
        LightBar.Maximum = max;
        HealthBar.Current = current;

    }

   
}
