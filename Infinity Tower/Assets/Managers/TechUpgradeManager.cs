using System.Collections.Generic;
using UnityEngine;

public class TechUpgradeManager : MonoBehaviour
{
    public static TechUpgradeManager instance { get; private set; }

    private readonly Dictionary<SO_TechData, int> purchasedLevelByTech = new();

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
    /// 구매 가능한지 확인.
    /// 클릭 시 미리보기(ChangeData)에서 호출.
    /// </summary>
    public bool CanPurchase(SO_TechData data, int level)
    {
        int existingLevel = purchasedLevelByTech.TryGetValue(data, out int found) ? found : 0;
        return level == existingLevel + 1;
    }

    public bool Purchase(SO_TechData data, int level, SO_AcientStone goods)
    {
        if (!(CanPurchase(data, level) && goods.Decrease(data.UseAmount)))
            return false;

        purchasedLevelByTech[data] = level;
        return true;
    }

    /// <summary>
    /// 플레이어가 스테이지 등장하는 시점에 호출
    /// 지금까지 구매된 모든 기술/능력을 실제로 적용한다
    /// </summary>
    public void ApplyAllPurchaseTechs()
    {
        foreach (var pair in purchasedLevelByTech)
        {
            pair.Key.Apply(pair.Value);
        }
    }

    public int GetCurrentLevel(SO_TechData data)
    {
        return purchasedLevelByTech.TryGetValue(data, out int lv) ? lv : 0;
    }
}
