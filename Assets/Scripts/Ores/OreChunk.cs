using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreChunk : MonoBehaviour
{
    public BaseOre Ore;
    public Rigidbody Rb;
    // Start is called before the first frame update
    void Start()
    {
        if (Ore == null)
        {
            Debug.Log("Ore is not set");
        }

        Rb = GetComponent<Rigidbody>();
    }

    public void Collect()
    {
        GameManager.Instance.AddCollectedOre(Ore.OreType);
        Destroy(gameObject);
    }
}
