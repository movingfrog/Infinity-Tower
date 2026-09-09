using UnityEngine;

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
    public SO_TechData SelectData { get; private set; }

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

    public void ChnageData(SO_TechData _data, int _level)
    {
        SelectData = _data;
        SUICC.SetBox(SelectData, _level);
    }
}
