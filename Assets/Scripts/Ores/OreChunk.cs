using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreChunk : MonoBehaviour
{
    public BaseOre Ore;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        if (Ore == null)
        {
            Debug.Log("Ore is not set");
        }

        rb = GetComponent<Rigidbody>();
    }

    public void Collect()
    {

        Destroy(gameObject);
    }
}
