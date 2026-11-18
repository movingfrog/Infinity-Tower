using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInfoIcon : MonoBehaviour
{
    [Serializable]
    private struct ConditionVisual
    {
        public AbilityCondition condition;
        public Sprite ringSprite; // 띠 형태
        public Color ringColor; // 띠 색
    }

    [Serializable]
    private struct TypeVisual
    {
        public AbilityType type;
        public string tagName; // 이름 앞에 붙을 태그 (예: 전투)
        public Color typeColor; // 종류 색 (이미지 + 태그 공통)
    }

    [Header("아이콘 이미지 참조")]
    [SerializeField]
    private Image centerImage;

    [SerializeField]
    private Image ringImage;

    [SerializeField]
    private Image typeImage;

    [Header("툴팁 텍스트 참조")]
    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text levelText;

    [SerializeField]
    private TMP_Text effectText;

    [Header("레벨 텍스트 색")]
    [SerializeField]
    private Color maxLevelColor = Color.yellow;

    [Header("비주얼 테이블")]
    [SerializeField]
    private ConditionVisual[] conditionVisuals;

    [SerializeField]
    private TypeVisual[] typeVisuals;

    private const string MaxLevelText = "Lv.Max";

    private bool isInitialized;
    private Color defaultLevelColor;

    public SO_TechData Data { get; private set; }
    public int Level { get; private set; }

    /// <summary>
    /// 생성 직후 단 한 번만 호출. 이후 호출은 무시됩니다.
    /// </summary>
    public void Initialize(SO_TechData data, int level)
    {
        if (isInitialized)
        {
            Debug.LogWarning($"{name}: 이미 초기화된 아이콘입니다.", this);
            return;
        }

        if (data == null)
        {
            Debug.LogError($"{name}: SO_TechData가 null입니다.", this);
            return;
        }

        if (level == 0)
        {
            Debug.LogWarning(
                $"{name}: 레벨이 0입니다. 능력은 1 이상의 레벨로 초기화되어야 합니다.",
                this
            );
            return;
        }

        Data = data;
        Level = level;

        bool hasTypeVisual = TryGetTypeVisual(data.Type, out var typeVisual);

        // 툴팁 텍스트 3종
        nameText.text = BuildName(data.Name, hasTypeVisual, typeVisual);
        ApplyLevel(level, data.MaxLevel);
        effectText.text = data.GetExplane(level);

        // 아이콘 비주얼 3종
        centerImage.sprite = data.TechIcon;
        ApplyRing(data.Condition);
        ApplyType(hasTypeVisual, typeVisual, data.Type);

        isInitialized = true;
    }

    private string BuildName(string abilityName, bool hasTypeVisual, TypeVisual typeVisual)
    {
        if (!hasTypeVisual || string.IsNullOrEmpty(typeVisual.tagName))
            return abilityName;

        string hex = ColorUtility.ToHtmlStringRGB(typeVisual.typeColor);
        return $"<color=#{hex}>[{typeVisual.tagName}]</color> {abilityName}";
    }

    private void ApplyLevel(int level, int maxLevel)
    {
        // 인스펙터에서 설정한 기본 색을 보존 (Initialize는 한 번만 호출되므로 안전)
        defaultLevelColor = levelText.color;

        bool isMax = level >= maxLevel;

        levelText.text = isMax ? MaxLevelText : $"Lv. {level}";
        levelText.color = isMax ? maxLevelColor : defaultLevelColor;
    }

    private bool TryGetTypeVisual(AbilityType type, out TypeVisual result)
    {
        foreach (var v in typeVisuals)
        {
            if (v.type != type)
                continue;
            result = v;
            return true;
        }
        result = default;
        return false;
    }

    private void ApplyRing(AbilityCondition condition)
    {
        foreach (var v in conditionVisuals)
        {
            if (v.condition != condition)
                continue;
            ringImage.sprite = v.ringSprite;
            ringImage.color = v.ringColor;
            return;
        }
        Debug.LogWarning($"조건 {condition}에 해당하는 비주얼이 없습니다.", this);
    }

    private void ApplyType(bool hasTypeVisual, TypeVisual typeVisual, AbilityType type)
    {
        if (!hasTypeVisual)
        {
            Debug.LogWarning($"종류 {type}에 해당하는 비주얼이 없습니다.", this);
            return;
        }

        typeImage.color = typeVisual.typeColor;
    }
}
