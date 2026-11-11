using TMPro;
using UnityEngine;

enum StatInfoType
{
    Int,
    Float,
    Percent,
}

public class StatInfoUI : MonoBehaviour
{
    private float MaxHP;

    [Header("스탯 UI")]
    [SerializeField]
    private StatInfoType[] Type;

    [SerializeField]
    private TextMeshProUGUI[] StatText;

    [field: SerializeField]
    public int StatIndex { get; private set; }

    private void Start()
    {
        MaxHP = PlayerStatManager.instance.MaxHP;
    }

    public void ShowStat()
    {
        StatText[0].text = StatValueDivision(
            Type[0],
            PlayerStatManager.instance.f_Damage(0),
            0,
            PlayerStatManager.instance.f_Damage(0)
        );
        StatText[1].text = StatValueDivision(
            Type[1],
            PlayerStatManager.instance.MaxHP,
            MaxHP,
            PlayerStatManager.instance.MaxHP - MaxHP
        );
        StatText[2].text = StatValueDivision(
            Type[2],
            PlayerStatManager.instance.f_Speed,
            1,
            PlayerStatManager.instance.f_Speed - 1
        );
        StatText[3].text = StatValueDivision(
            Type[3],
            PlayerStatManager.instance.f_Crit_Rate,
            .3f,
            PlayerStatManager.instance.f_Crit_Rate - .3f
        );
        StatText[4].text = StatValueDivision(
            Type[4],
            PlayerStatManager.instance.f_Crit_Dmg,
            1.5f,
            PlayerStatManager.instance.f_Crit_Dmg - 1.5f
        );
        StatText[5].text = StatValueDivision(
            Type[5],
            PlayerStatManager.instance.f_GoldBoost,
            1,
            PlayerStatManager.instance.f_GoldBoost - 1
        );
        StatText[6].text = StatValueDivision(
            Type[6],
            PlayerStatManager.instance.f_HealBoost,
            1,
            PlayerStatManager.instance.f_HealBoost - 1
        );
        StatText[7].text = StatValueDivision(Type[7], PlayerStatManager.instance.Satiety, 0);
    }

    private string StatValueDivision(StatInfoType type, float value, float dValue, float pValue = 0)
    {
        switch (type)
        {
            case StatInfoType.Int:
                Debug.Log($"value: {value}, dValue: {dValue}, pValue: {pValue}");
                return $"{(int)value}(<color=yellow>{(int)dValue}</color>+<color=green>{(int)pValue}</color>)";
            case StatInfoType.Float:
                Debug.Log($"value: {value}, dValue: {dValue}, pValue: {pValue}");
                return $"{(int)(value * 100)}%(<color=yellow>{(int)(dValue * 100)}%</color>+<color=green>{(int)(pValue * 100)}%</color>)";
            case StatInfoType.Percent:
                return (value).ToString("0") + "%";
            default:
                return null;
        }
    }
}
