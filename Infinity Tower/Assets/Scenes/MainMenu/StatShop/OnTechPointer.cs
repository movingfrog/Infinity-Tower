using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnTechPointer : MonoBehaviour, IPointerClickHandler
{
    private Image BGImage;
    private bool isLock = true;

    [SerializeField, Tooltip("사용될 아이콘")]
    private Image MainIcon;

    [SerializeField, Tooltip("아이콘 색상")]
    private Color IconColor;

    [SerializeField, Tooltip("다음 단계 객체")]
    private OnTechPointer NextPurchaseObject;

    [SerializeField, Tooltip("색이 변경될 라인")]
    private Image LinkedLine;

    [SerializeField, Tooltip("구매 가능 색 값")]
    private Color canPurchaseColor;

    [SerializeField, Tooltip("구매 상태 색 값")]
    private Color purchaseColor;

    [field: SerializeField]
    public SO_TechData Data { get; private set; }

    [field: SerializeField, Tooltip("최소값을 1을 넘도록 해주세요")]
    public int Level;

    private void Awake()
    {
        BGImage = GetComponent<Image>();
        MainIcon.color = IconColor;
        if (MainIcon.sprite != Data.TechIcon)
            MainIcon.sprite = Data.TechIcon;
        if (TechUpgradeManager.instance.CanPurchase(Data, Level))
        {
            UnlockLevel();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLock)
            return;
        ShopFuncControllCenter.Instance.ChnageData(this);
    }

    private void UnlockLevel()
    {
        BGImage.color = canPurchaseColor;
        LinkedLine.color = canPurchaseColor;
        isLock = false;
    }

    public void Purchase()
    {
        NextPurchaseObject?.UnlockLevel();
        BGImage.color = purchaseColor;
        LinkedLine.color = purchaseColor;
    }
}
