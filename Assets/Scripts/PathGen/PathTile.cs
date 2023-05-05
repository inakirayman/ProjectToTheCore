using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTile : MonoBehaviour
{
    [Range(-90, 90)]
    public int PathRotation;
    public Transform StartTransform;
    public Transform EndTransform;
    public int Length;
}
