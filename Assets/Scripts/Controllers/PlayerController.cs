using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float MovementSpeed = 3f;

    [SerializeField]
    private Transform _cameraTransform;

    private Vector2 _movementInput;
    private Rigidbody _rb;
    
    void Start()
    {
        if (_cameraTransform == null)
        {
            _cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }


        _rb = GetComponent<Rigidbody>();


      

    }

    private void FixedUpdate()
    {
        // get input from the joystick axes
        float horizontalInput = _movementInput.x;
        float verticalInput = _movementInput.y;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            
            Vector3 cameraForward = _cameraTransform.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = _cameraTransform.right;
            cameraRight.y = 0f;

           
            Vector3 targetPosition = transform.position +
                cameraForward.normalized * movementDirection.z * MovementSpeed * Time.fixedDeltaTime +
                cameraRight.normalized * movementDirection.x * MovementSpeed * Time.fixedDeltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

            
            _rb.MovePosition(targetPosition);
            _rb.MoveRotation(targetRotation);
        }

    }
    public void Move(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }


}
