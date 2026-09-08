using UnityEngine;

public class SO_TechData : ScriptableObject
{
    [field: SerializeField]
    public Sprite TechIcon;

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField, TextArea]
    public string Explane { get; private set; }

    [field: SerializeField]
    public uint UseAmount { get; private set; }
}
