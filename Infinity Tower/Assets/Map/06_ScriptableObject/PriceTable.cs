using System.Collections.Generic;

public static class PriceTable
{
    private static Dictionary<ItemLevel, int> PriceCoefficient = new Dictionary<ItemLevel, int>
    {
        { ItemLevel.Common, 1 },
        { ItemLevel.Rare, 3 },
        { ItemLevel.Legend, 5 },
    };

    private const int healthValue = 5;
    private const int goldValue = 100;

    private const float SellEfficiency = .3f;

    private const float SmithyMultiplier = 0.5f;

    private static int GetCoefficient(ItemLevel grade) =>
        PriceCoefficient.TryGetValue(grade, out int value) ? value : 0;

    public static int GetPrice(ItemLevel grade, bool isHealth) =>
        GetCoefficient(grade) * (isHealth ? healthValue : goldValue);

    //public static int GetSellPrice(ItemLevel grade, bool isEquip) => isEquip ? (int)(GetPrice(grade, false) * SellEfficiency) : GetCoefficient(grade)
}
