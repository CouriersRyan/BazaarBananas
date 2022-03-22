using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public int Width
    {
        get { return size.arrays.Count; }
    }
    public int Height
    {
        get { return size.arrays[0].cells.Count; }
    }

    public Matrix size;

    public Sprite itemIcon;

    public int value = -1;

    [Header("Min/Max values inclusive")]
    public Vector2Int gold;
    public Vector2Int protection;
    public Vector2Int tools;
    public Vector2Int food;
}
