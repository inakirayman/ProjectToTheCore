using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTileJunction : PathTile
{
    [Range(-90, 90)]
    public int SplitRotationLeft;
    [Range(-90, 90)]
    public int SplitRotationRight;
    public Transform EndTransformLeft;
    public Transform EndTransformRight;
}
