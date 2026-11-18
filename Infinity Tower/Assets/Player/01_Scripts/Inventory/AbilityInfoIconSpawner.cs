using UnityEngine;

public class AbilityInfoIconSpawner : MonoBehaviour
{
    [field: SerializeField]
    public AbilityInfoIcon IconPrefab { get; private set; }

    [field: SerializeField]
    public SO_TechData[] AllTechData { get; private set; }

    private void Start()
    {
        SpawnIcons();
    }

    private void SpawnIcons()
    {
        foreach (var data in AllTechData)
        {
            if (TechUpgradeManager.instance.GetCurrentLevel(data) == 0)
            {
                continue;
            }
            var icon = Instantiate(IconPrefab, transform);
            icon.Initialize(data, TechUpgradeManager.instance.GetCurrentLevel(data));
        }
    }
}
