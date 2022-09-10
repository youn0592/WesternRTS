using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIObjectScriptable", menuName = "ScriptableObjects/AIObject")]
public class AIObjectScriptable : ScriptableObject
{
    public string AIName;
    public GameObject AIPrefab;
}
