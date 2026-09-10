using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OwnedAbility
{
    public SO_AbilityTechData Data;
    public int Level;

    public OwnedAbility(SO_AbilityTechData _data, int _level)
    {
        Data = _data;
        Level = _level;
    }
}

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance { get; private set; }

    private readonly List<OwnedAbility> ownedAbilities = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 능력 획드. SO_AbilityTechData.Apply(level)에서 호출됨.
    /// </summary>
    public void Acquire(SO_AbilityTechData data, int level)
    {
        var owned = new OwnedAbility(data, level);
        ownedAbilities.Add(owned);
    }

    public void NotifyAttack(ref AttackContext ctx)
    {
        foreach (var owned in ownedAbilities)
            if (owned.Data is IOnAttack r)
                r.OnAttack(owned.Level, ref ctx);
    }

    public void NotifyHit(ref HitContext ctx)
    {
        foreach (var owned in ownedAbilities)
            if (owned.Data is IOnHit r)
                r.OnHit(owned.Level, ref ctx);
    }

    public bool HasAbility(SO_AbilityTechData data) => ownedAbilities.Exists(o => o.Data == data);

    public int GetLevel(SO_AbilityTechData data)
    {
        var found = ownedAbilities.Find(o => o.Data == data);
        return found?.Level ?? 0;
    }
}
