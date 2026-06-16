using System.Collections.Generic;

public static class PriceTable
{
    private static Dictionary<ItemLevel, uint> PriceCoefficient = new Dictionary<ItemLevel, uint>
    {
        { ItemLevel.Common, 1 },
        { ItemLevel.Rare, 3 },
        { ItemLevel.Legend, 5 },
    };

    private const uint healthValue = 1;
    private const uint goldValue = 100;

    private const uint lootValue = 5;

    private const float SellEfficiency = .7f;

    private const float SmithyMultiplier = 0.5f;

    private static uint GetCoefficient(ItemLevel grade) =>
        PriceCoefficient.TryGetValue(grade, out uint value) ? value : 0;

    public static uint GetPrice(ItemLevel grade, bool isHealth) =>
        GetCoefficient(grade) * (isHealth ? healthValue : goldValue);

    public static uint GetSellPrice(ItemLevel grade, bool isEquip) =>
        isEquip
            ? (uint)(GetPrice(grade, false) * SellEfficiency)
            : GetCoefficient(grade) * lootValue;

    public static Dictionary<GoodsType, uint> UpgradePrice(ItemLevel currentGrade)
    {
        uint goldPrice = (uint)(GetPrice(currentGrade, false) * (1 + SmithyMultiplier));
        uint stonePrice = 0;
        return new Dictionary<GoodsType, uint>
        {
            { GoodsType.Gold, goldPrice },
            { GoodsType.Stone, stonePrice },
        };
    }
}
