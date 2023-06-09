using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  
    public float MovementSpeed = 3f;

    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private LayerMask _oreLayer;
    [SerializeField]
    private LayerMask _orePickupLayer;
    [SerializeField]
    private LayerMask _minecartLayer;
    [SerializeField]
    private Animator _animator;



    [SerializeField]
    private Transform _hands;
    [SerializeField]
    private GameObject _objectInHands = null;
    public GameObject ObjectInHands
    {
        get { return _objectInHands; }
        set
        {
            _objectInHands = value;
            _isHoldingObject = (_objectInHands != null);
        }
    }

    [SerializeField]
    private float _interactRadius = 1;

    private Vector2 _movementInput;
    private Rigidbody _rb;


    private bool _isHoldingObject = false;

    
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
        if (_isHoldingObject)
        {
            _objectInHands.GetComponent<OreChunk>().Rb.useGravity = false;
            _objectInHands.GetComponent<OreChunk>().Rb.MovePosition(_hands.position);
        }




        // get input from the joystick axes
        float horizontalInput = _movementInput.x;
        float verticalInput = _movementInput.y;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            _animator.SetBool("IsMoving", true);
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
        else
        {
            _animator.SetBool("IsMoving", false);
        }

    }
    public void Move(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_isHoldingObject)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, _interactRadius, _orePickupLayer);
                if (colliders.Length > 0)
                {

                    ObjectInHands = colliders[0].transform.parent.gameObject;
                    return;
                }
               


                colliders = Physics.OverlapSphere(transform.position, _interactRadius, _oreLayer);
                if (colliders.Length > 0)
                {
                    _animator.SetTrigger("IsMining");
                    colliders[0].GetComponent<OreVein>().Mine(1);
                    return;
                }
                


            }
            else if(_isHoldingObject)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, _interactRadius, _minecartLayer);
                if (colliders.Length > 0)
                {
                    _objectInHands.GetComponent<OreChunk>().Collect();
                    ObjectInHands = null;
                    return;
                }
                else
                {
                    _objectInHands.GetComponent<OreChunk>().Rb.useGravity = true;
                    ObjectInHands = null;
                    return;
                }


             
            }
           






        }
    }

    public bool CanMountUp()
    {
        if (_isHoldingObject)
        {
            return false;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, _interactRadius, _minecartLayer);
        if (colliders.Length > 0)
        {
            _animator.SetBool("IsMoving", false);   
            return true;
           

        }
        return false;
    }



    private void OnDrawGizmosSelected()
    {
        // Display the detection radius in the Unity Editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactRadius);
    }
}



