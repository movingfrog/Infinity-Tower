using UnityEngine;
using UnityEngine.UI;

public class ShopFuncControllCenter : MonoBehaviour
{
    public static ShopFuncControllCenter Instance { get; private set; }

    [Header("내부 기능 수행 객체")]
    private ShopUIControllCenter SUICC;
    private RotateShops RS;

    [SerializeField, Tooltip("기술 정보")]
    private TechInfo[] TechInfoes; // 기능 수행 싱글톤 클래스로 옮길 예정
    public TechInfo[] _TechInfo => TechInfoes;

    public int CurrentIndex = 0; // 현재 선택된 상점 인덱스 - 기능 수행 싱글톤 클래스로 옮길 예정

    /// <summary>
    /// 구매를 위한 선택 기술 데이터
    /// </summary>
    public OnTechPointer SelectData { get; private set; }

    private void Awake()
    {
        Instance = this;
        RS = GetComponent<RotateShops>();
        SUICC = GetComponent<ShopUIControllCenter>();
    }

    private void OnEnable()
    {
        SUICC.ResetAll();
        RS.ResetShopPos();
    }

    public void ChnageData(OnTechPointer _data)
    {
        SelectData = _data;
        SUICC.SetBox(_data.Data, _data.Level);
        SUICC.ChangeToolBar(TechUpgradeManager.instance.CanPurchase(_data.Data, _data.Level));
    }

    public void Purchase()
    {
        if (SelectData != null)
        {
            if (
                TechUpgradeManager.instance.Purchase(SelectData.Data, SelectData.Level, SUICC.Goods)
            )
            {
                SelectData.Purchase();
                SUICC.ChangeToolBar(false);
            }
        }
    }
}
