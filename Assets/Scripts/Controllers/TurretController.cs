using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private Transform _exitPoint;
    [SerializeField]
    private GameObject _bulletPrefab;

    [SerializeField]
    private Transform _turret;
    private Vector2 _aimInput;
 

    [SerializeField]
    private float _rotationSpeed =10;


    [SerializeField]
    private Transform _barrel;



    public PlayerController _player;

    public float FireRate = 5;
    private float _timeSinceLastShot = 0f;
    private bool _isShooting;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateTurret();
        _player.transform.rotation = _turret.rotation;
        ShootLogic();
    }

    private void ShootLogic()
    {
        _timeSinceLastShot += Time.deltaTime;


        if (_timeSinceLastShot >= 1 / FireRate && _isShooting)
        {
            Destroy(Instantiate(_bulletPrefab, _barrel.position, _turret.rotation), 4);

            _timeSinceLastShot = 0;
        }
    }

    private void RotateTurret()
    {
        if (_aimInput.magnitude == 0f)
            return; // No input, no rotation

        // Get the camera forward vector
        Vector3 cameraForward = _cameraTransform.forward;
        cameraForward.y = 0f; // Ignore vertical rotation

        // Calculate the rotation angle based on the input and camera forward vector
        float angle = Mathf.Atan2(_aimInput.x, _aimInput.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward) * Quaternion.Euler(0f, angle, 0f);

        // Rotate the turret to the target rotation
        _turret.rotation = Quaternion.RotateTowards(_turret.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }



    public void MountUp()
    {
        _player.transform.position = _turret.position - new Vector3(0,0.5f,0);
        _player.transform.rotation = _turret.rotation;
        _player.transform.parent = _turret;

    }


    public void Dismount()
    {
        _player.transform.position = _exitPoint.position;
        _player.transform.parent = null;

    }

    public void Shoot(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            _isShooting = true;
        }
        else if (context.canceled)
        {
            _isShooting = false;

        }
    }

    public void Aim(InputAction.CallbackContext context)
    {
        _aimInput = context.ReadValue<Vector2>();
    }







}
