using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : BaseEnemy
{
    [SerializeField]
    private GameObject _explosionEffect;
    [SerializeField]
    private float _explosionRadius;
    [SerializeField]
    private int _explosionDamage;
    [SerializeField]
    private float _explosionDelay;

    private bool _hasExploded;
    private bool _explosionDelayStarted;

    

    private void Update()
    {
        if (!_hasExploded)
        {
            // Custom logic for the Exploder enemy
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator StartExplosionDelay()
    {
        yield return new WaitForSeconds(_explosionDelay);
        Explode();

    }



    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider collider in colliders)
        {
            Minecart minecart = collider.GetComponent<Minecart>();
            if (minecart != null)
            {
                minecart.Hit(_explosionDamage);
                break;
            }
        }
        Instantiate(_explosionEffect, transform.position, transform.rotation);
        _hasExploded = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Minecart"))
        {
            if (!_explosionDelayStarted)
            {
                _explosionDelayStarted = true;
                StopMovement(); // Stop the movement of the BaseEnemy
                Animator.SetBool("Jump", true);
                StartCoroutine(StartExplosionDelay());
            }
        }
    }
}
  






