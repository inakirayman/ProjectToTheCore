using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OreVein : MonoBehaviour
{
    public BaseOre Ore;



    void Start()
    {
        if(Ore == null)
        {
            Debug.Log("Ore is not set");
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        Ore.Hit();
        
    }
}
