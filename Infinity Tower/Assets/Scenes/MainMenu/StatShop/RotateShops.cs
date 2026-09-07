using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotateShops : MonoBehaviour
{
    [Header("UI 속성")]
    [SerializeField, Tooltip("상점 미리보기 아이콘")]
    private Image[] PreviewIcons; // 상점 미리보기 아이콘 배열

    [SerializeField, Tooltip("상점 미리보기 이미지")]
    private Image[] PreviewOption; // 상점 미리보기 옵션 배열

    [SerializeField, Tooltip("상점 미리보기 아이콘 이미지")]
    private Image[] PreviewIconImages;

    [SerializeField, Tooltip("상점 옵션 이름 텍스트")]
    private TextMeshProUGUI OptionName; // 상점 옵션 이름 텍스트

    [SerializeField, Tooltip("상점 옵션 설명 텍스트")]
    private TextMeshProUGUI OptionExplane; // 상점 옵션 설명 텍스트

    [Header("UI 상세 속성")]
    [SerializeField, Tooltip("선택된 상점 색상")]
    private Color SelectColor = Color.white;

    [SerializeField, Tooltip("선택되지 않은 상점 색상")]
    private Color UnSelectColor = Color.white;
}
