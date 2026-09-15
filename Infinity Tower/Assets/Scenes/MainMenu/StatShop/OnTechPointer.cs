using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnTechPointer : MonoBehaviour, IPointerClickHandler
{
    protected Image BGImage;
    protected bool isLock = true;

    [SerializeField, Tooltip("사용될 아이콘")]
    protected Image MainIcon;

    [SerializeField, Tooltip("아이콘 색상")]
    protected Color IconColor;

    [SerializeField, Tooltip("다음 단계 객체")]
    protected OnTechPointer NextPurchaseObject;

    [SerializeField, Tooltip("색이 변경될 라인")]
    protected Image LinkedLine;

    [SerializeField, Tooltip("구매 가능 색 값")]
    protected Color canPurchaseColor;

    [SerializeField, Tooltip("구매 상태 색 값")]
    protected Color purchaseColor;

    [field: SerializeField]
    public SO_TechData Data { get; protected set; }

    [field: SerializeField, Tooltip("최소값을 1을 넘도록 해주세요")]
    public int Level;

    protected virtual void Awake()
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

    protected virtual void UnlockLevel()
    {
        BGImage.color = canPurchaseColor;
        LinkedLine.color = canPurchaseColor;
        isLock = false;
    }

    public virtual void Purchase()
    {
        NextPurchaseObject?.UnlockLevel();
        BGImage.color = purchaseColor;
        LinkedLine.color = purchaseColor;
    }
}
