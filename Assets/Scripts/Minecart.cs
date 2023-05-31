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
    private float _moveSpeed;
    private float _rotationSpeed =4;
    #endregion


    #region Waypoints
    private int _currentWaypointIndex;
    private List<Transform> _waypoints;
    #endregion






    private void Start()
    {
        _moveSpeed = BaseMoveSpeed;


        if (AreaLight == null)
        {
            // If the target light is not assigned, use the light component attached to this GameObject
            AreaLight = GetComponent<Light>();
        }

        // Store the initial intensity of the light
       

        // Set the initial target intensity to the minimum intensity
        _targetIntensity = IntensityRange.y;
        AreaLight.intensity = _targetIntensity;



        _waypoints = GameManager.Instance.MinecartWaypoints;

        if (_waypoints.Count > 0)
        {
            transform.position = _waypoints[0].position;
        }



    }

    private void Update()
    {
        LightLogic();


       
    }


    private void FixedUpdate()
    {
        //Check if ther minecart has fuel.
        if(GameManager.Instance.MinecartFuel > 0 && GameManager.Instance.IsMinecartDriving)
        {
            if (_currentWaypointIndex < _waypoints.Count)
            {
                Transform currentWaypoint = _waypoints[_currentWaypointIndex];
                MoveTowardsWaypoint(currentWaypoint);

                GameManager.Instance.MinecartFuel -= _moveSpeed * Time.deltaTime;

            }
        }


       
    }



    public void MinecartHit(float value = 1)
    {
        GameManager.Instance.MinecartHealth -= value;
    }


    #region Minecart Movement

    private void MoveTowardsWaypoint(Transform waypoint)
    {
        // Calculate the direction and distance to the waypoint
        Vector3 direction = waypoint.position - transform.position;
        direction.y = 0; // Set the y component of the direction to zero to restrict movement to the x and z axes
        float distance = direction.magnitude;

        // Move the minecart towards the waypoint at the current move speed
        transform.position += direction.normalized * _moveSpeed * Time.deltaTime;

        // Rotate the minecart towards the waypoint over time


        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
        

        // Check if the minecart has reached the waypoint
        if (distance <= _moveSpeed * Time.deltaTime)
        {
            // Set the minecart position to the exact waypoint position
            transform.position = new Vector3(waypoint.position.x, transform.position.y, waypoint.position.z);

            // Move to the next waypoint
            _currentWaypointIndex++;

            // Check if the minecart has reached the final waypoint
            if (_currentWaypointIndex >= _waypoints.Count)
            {
                // Perform actions when the minecart reaches the end
                OnMinecartReachEnd();
            }
        }
    }



    private void OnMinecartReachEnd()
    {
        // Custom logic when the minecart reaches the end (e.g., end game, reset, etc.)
    }
    #endregion



    #region Light Logic

    private void LightLogic()
    {
        if (GameManager.Instance.LightEnergy == 100)
        {
            BrightenLight();

            foreach (GameObject gameObject in SideLights)
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

        if (_targetIntensity != AreaLight.intensity)
            AreaLight.intensity = Mathf.Lerp(AreaLight.intensity, _targetIntensity, LerpSpeed * Time.deltaTime);
    }

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

