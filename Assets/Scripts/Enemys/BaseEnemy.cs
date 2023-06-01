using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    private float _health;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _minDistanceThreshold;


    public Transform Target;
    private Vector3 _desiredPosition;
    private Rigidbody _rb;
    [SerializeField]
    public Animator Animator;
    private bool _isAllowedToMove = true;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_isAllowedToMove)
        {
            MoveTowardsTarget();
            RotateTowardsMovement();
        }
       
    }

    private void MoveTowardsTarget()
    {
        Vector3 desiredPosition = Target.position - transform.position;
        desiredPosition.y = 0f; // Ignore Y component
        float distance = desiredPosition.magnitude; // Distance between enemy and target

        if (distance > _minDistanceThreshold)
        {
            desiredPosition.Normalize();
            _rb.velocity = desiredPosition * _moveSpeed;
            Animator.SetBool("Walk Forward", true);
        }
        else
        {
            _rb.velocity = Vector3.zero;
            Animator.SetBool("Walk Forward", false);
        }
    }

    private void RotateTowardsMovement()
    {
        if (_rb.velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_rb.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    protected void StopMovement()
    {
        _isAllowedToMove = false;
        _rb.velocity = Vector3.zero; // Stop the movement of the BaseEnemy
        Animator.SetBool("Walk Forward", false);
    }

    public void Hit(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }


    protected void Die()
    {
        // Destroy the enemy or perform any other actions required for death
        Destroy(gameObject);
    }
}
