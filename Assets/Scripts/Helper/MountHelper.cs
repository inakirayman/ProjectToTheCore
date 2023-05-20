using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MountHelper : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private TurretController _turretController;


    public void MountAndDismount(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            if (_playerController.enabled && _playerController.CanMountUp())
            {



                _playerController.enabled = false;
                _turretController.enabled = true;
                _turretController.MountUp();


            }
            else if (_turretController.enabled)
            {
                _turretController.Dismount();
                _turretController.enabled = false;
                _playerController.enabled = true;

            }
        }
     



    }


    

}
