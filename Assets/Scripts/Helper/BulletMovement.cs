using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float bulletSpeed;
    private void FixedUpdate()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }




    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
