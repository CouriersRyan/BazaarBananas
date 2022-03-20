using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;

    public int value = -1;

    [Header("Min/Max values inclusive")]
    public Vector2Int gold;
    public Vector2Int protection;
    public Vector2Int tools;
    public Vector2Int food;
}
