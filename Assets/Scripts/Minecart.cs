using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecart : MonoBehaviour
{

    #region Light Stats
    [Header("Light Stats")]
    public Light AreaLight;
    public List<GameObject> SideLights = new List<GameObject>();
    public Vector2 IntensityRange = new Vector2(0.2f, 1f);
    public float LerpSpeed = 1f;
    private float _targetIntensity;
    #endregion

    #region Minecart Stats
    [Header("Minecart Stats")]
    public float BaseMoveSpeed;




    #endregion





    private void Start()
    {
        if (AreaLight == null)
        {
            // If the target light is not assigned, use the light component attached to this GameObject
            AreaLight = GetComponent<Light>();
        }

        // Store the initial intensity of the light
       

        // Set the initial target intensity to the minimum intensity
        _targetIntensity = IntensityRange.y;
        AreaLight.intensity = _targetIntensity;

    }

    private void Update()
    {

        if (GameManager.Instance.LightEnergy == 100)
        {
            BrightenLight();

            foreach(GameObject gameObject in SideLights)
            {
                gameObject.SetActive(true);
            }


        }
        else if (GameManager.Instance.LightEnergy == 0)
        {
            DimLight();

            foreach (GameObject gameObject in SideLights)
            {
                gameObject.SetActive(false);
            }
        }

        if(_targetIntensity != AreaLight.intensity)
        AreaLight.intensity = Mathf.Lerp(AreaLight.intensity, _targetIntensity, LerpSpeed * Time.deltaTime);
    }

    public void MinecartHit(float value = 1)
    {
        GameManager.Instance.MinecartHealth -= value;
    }


    #region Light Logic
    public void DimLight()
    {
        // Set the target intensity to the minimum intensity
        _targetIntensity = IntensityRange.x;
    }

    public void BrightenLight()
    {
        // Set the target intensity to the maximum intensity
        _targetIntensity = IntensityRange.y;
    }
    #endregion
}

