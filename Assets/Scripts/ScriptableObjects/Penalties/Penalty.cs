using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CreateAssetMenu(fileName = "Penalty", menuName = "ScriptableObjects/Penalty", order = 3)]
public abstract class Penalty : ScriptableObject
{
     public abstract void RunPenalty();
}