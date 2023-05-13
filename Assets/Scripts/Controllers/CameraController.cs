using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public float Speed = 5f;
    public float rotateSpeed = 10f;

    private Vector3 _offset;
    private float _horizontalInput;

    private void Start()
    {
        if(Target == null)
        {
            Target =  GameObject.FindGameObjectWithTag("Player").transform;
        }

        
        _offset = transform.position - Target.position;
    }

    private void FixedUpdate()
    {
        // Calculate the desired position of the camera
        Vector3 desiredPosition = Target.position + _offset;

        // Interpolate between the current camera position and the desired position
        Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, Speed * Time.deltaTime);

        
        // Set the camera position to the interpolated position
        transform.position = newPosition;


        transform.Rotate(Vector3.up, _horizontalInput * rotateSpeed * Time.deltaTime);
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        _horizontalInput = context.ReadValue<Vector2>().x;
    }

}
