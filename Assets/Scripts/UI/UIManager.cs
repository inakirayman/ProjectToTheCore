using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ProgressBar FuelBar;
    public ProgressBar HealthBar;
    public ProgressBar LightBar;
    
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
