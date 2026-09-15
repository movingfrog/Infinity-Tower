using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_DiscountAbility",
    menuName = "Scriptable Objects/SO_DiscountAbility"
)]
public class SO_DiscountAbility : SO_AbilityTechData, IOnAcquire
{
    private int DiscountCacul(int level) => level * 20 - 10;

    public override string GetEffect(int level)
    {
        return string.Format(_effect, DiscountCacul(level - 1), DiscountCacul(level));
    }

    public override string GetExplane(int level)
    {
        return string.Format(_explane, DiscountCacul(level));
    }

    public void OnAcquire(int level)
    {
        GameManager.Instance.PriceDiscountAmount -= DiscountCacul(level);
    }
}
