using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTilePath : PathTile
{
    [Range(-90, 90)]
    public int PathRotation;
    public Transform EndTransform;
    public int Length;
}
