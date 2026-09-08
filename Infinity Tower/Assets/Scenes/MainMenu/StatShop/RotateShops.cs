using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RotateShops : MonoBehaviour
{
    public ShopControllCenter SCC { get; set; }
    private bool isRotate;

    public int CurrentIndex { get; private set; } = 0; // 현재 선택된 상점 인덱스 - 기능 수행 싱글톤 클래스로 옮길 예정

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

    [SerializeField, Tooltip("회전 Alphar값")]
    private float[] RotationAlphas;

    [SerializeField, Tooltip("회전 X 위치")]
    private float[] RotationXPositions;

    [SerializeField, Tooltip("회전 스케일")]
    private float[] RotationScale;

    [SerializeField, Tooltip("회전 레이어")]
    private Transform[] RotationParent;

    [Header("회전 속성")]
    [SerializeField, Tooltip("회전 시간")]
    private float RotationOffset;

    public void ResetShopPos()
    {
        CurrentIndex = 0;
        RotateShop(0);
    }

    public void RotateShop(int index)
    {
        if (isRotate)
            return;
        Sequence rotateSequence = DOTween.Sequence();
        isRotate = true;
        CurrentIndex += index;
        if (CurrentIndex < 0)
        {
            CurrentIndex = PreviewIcons.Length - 1;
        }
        else if (CurrentIndex >= PreviewIcons.Length)
        {
            CurrentIndex = 0;
        }

        for (int i = 0; i < PreviewIcons.Length; i++)
        {
            if (PreviewIcons[i] == null)
            {
                Debug.LogError("미리보기 아이콘이 할당되지 않았습니다");
                return;
            }

            PreviewIcons[i].color = (i == CurrentIndex) ? SelectColor : UnSelectColor;

            int n = i - CurrentIndex;
            if (n < 0)
                n += PreviewIcons.Length;
            rotateSequence.Join(
                PreviewOption[i]
                    .rectTransform.DOLocalMoveX(RotationXPositions[n], RotationOffset)
                    .SetEase(Ease.OutCirc)
            );
            PreviewOption[i].rectTransform.localScale = Vector3.one * RotationScale[n];
            PreviewOption[i].color *= new Color(
                1f,
                1f,
                1f,
                RotationAlphas[n] / PreviewOption[i].color.a
            );
            PreviewIconImages[i].color *= new Color(
                1f,
                1f,
                1f,
                RotationAlphas[n] / PreviewIconImages[i].color.a
            );
            PreviewOption[i].rectTransform.SetParent(RotationParent[n], false);
        }
        OptionName.text = SCC._TechInfo[CurrentIndex].Name;
        OptionExplane.text = SCC._TechInfo[CurrentIndex].Explane;
        rotateSequence.OnComplete(() =>
        {
            isRotate = false;
        });
    }
}
