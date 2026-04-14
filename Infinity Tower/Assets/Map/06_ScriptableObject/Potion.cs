using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Scriptable Objects/Potion", order = 1)]
public class Potion : ScriptableObject
{
    [Header("UI속성")]
    public Sprite PotionIcon;
    public string potionName;

    [TextArea]
    public string potionInfo;
    public uint price;

    [Header("물약 효과")]
    [Range(1, 10)]
    public int healAmount;

    [Range(1, 3)]
    public int healthAmount;

    [Range(1, 100)]
    public int satietyAmount;
}
