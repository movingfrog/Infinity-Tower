using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageMapList", menuName = "Scriptable Objects/StageMapList")]
public class StageMapList : ScriptableObject
{
    [field: SerializeField]
    public List<GameObject> MapList { get; private set; } = new();

    [field: SerializeField]
    public List<GameObject> EventList { get; private set; } = new();
}
