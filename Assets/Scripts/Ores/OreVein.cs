using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OreVein : MonoBehaviour
{
    public BaseOre Ore;

    private int _hitPoints;



    void Start()
    {
        if(Ore == null)
        {
            Debug.Log("Ore is not set");
        }
        if(Ore.OreVeinModel == null)
        {
            Debug.Log($"OreVein model is not set in {Ore.name}");
        }
        _hitPoints = Ore.HitPoint;
        Instantiate(Ore.OreVeinModel, transform.position, Quaternion.identity, transform);
    }

    public void Mine(float value = 0)
    {
        _hitPoints = Ore.Hit(_hitPoints,value);

        if (_hitPoints <= 0)
        {
            Ore.Break(transform.position + new Vector3(0,0.5f,0));

            gameObject.SetActive(false);
        }

    }
}
