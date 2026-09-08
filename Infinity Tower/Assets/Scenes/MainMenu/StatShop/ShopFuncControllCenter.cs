using UnityEngine;

public class ShopFuncControllCenter : MonoBehaviour
{
    public static ShopFuncControllCenter Instance { get; private set; }

    [SerializeField, Tooltip("기술 정보")]
    private TechInfo[] TechInfoes; // 기능 수행 싱글톤 클래스로 옮길 예정
    public TechInfo[] _TechInfo => TechInfoes;

    public int CurrentIndex = 0; // 현재 선택된 상점 인덱스 - 기능 수행 싱글톤 클래스로 옮길 예정

    private void Awake()
    {
        Instance = this;
    }
}
