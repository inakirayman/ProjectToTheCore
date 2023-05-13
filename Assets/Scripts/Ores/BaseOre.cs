using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Ore", menuName = "Ore/BaseOre")]
public class BaseOre : ScriptableObject
{
    public int HitPoint = 3;
    public Ores OreType = Ores.Coal;

    public void Hit(float value = 0)
    {
        HitPoint -=(int)value;
        if (HitPoint <= 0)
            Break();


        Debug.Log($"{OreType.ToString()} Ore Hit For {value}");
    }

    public  void Break()
    {
        Debug.Log($"{OreType.ToString()} breaks");
    }
}
