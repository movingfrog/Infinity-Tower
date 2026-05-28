using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [field: SerializeField]
    public WeaponType Type { get; private set; }

    [SerializeField]
    private GameObject[] UpgradePrefab;

    public GameObject GetPrefabByLevel(ItemLevel level) => UpgradePrefab[(int)level];
}
