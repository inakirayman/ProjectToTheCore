using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }



    public ProgressBar FuelBar;
    public ProgressBar HealthBar;
    public ProgressBar LightBar;
    

    public TextMeshProUGUI CoalOreAmount;
    public TextMeshProUGUI IronOreAmount;
    public TextMeshProUGUI GoldOreAmount;


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
   
}
