using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "soPrefabInstatiate", menuName = "Scriptable Objects/Startup")]
public class SOPrefabInstatiate : ScriptableObject
{
    [SerializeField]
    public List<PrefabInstatiateStruct> prefabInstatiateList;
}
