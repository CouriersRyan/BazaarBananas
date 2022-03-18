using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;

    public int value;

    public int gold;
    public int protection;
    public int tools;
    public int food;
}
