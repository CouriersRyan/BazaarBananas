using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Array
{
    public List<bool> cells = new List<bool>();
    public bool this[int index] => cells[index];
}

[Serializable]
public class Matrix
{
    public List<Array> arrays = new List<Array>();
    public bool this[int x, int y] => arrays[x][y];
}
