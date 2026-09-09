using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct TechInfo
{
    public string Name;

    [TextArea]
    public string Explane;
}

[RequireComponent(typeof(RotateShops))]
public class ShopUIControllCenter : MonoBehaviour
{
    [SerializeField]
    private SO_AcientStone Goods;

    [Header("타이틀 UI")]
    [SerializeField, Tooltip("타이틀 TMP할당")]
    private TextMeshProUGUI Title;

    [Header("메인 UI")]
    [SerializeField, Tooltip("PreviewMenu")]
    private GameObject PreviewMenu;

    [SerializeField, Tooltip("상점 관리자")]
    private GameObject[] Shops;

    [Header("도구 UI")]
    [SerializeField, Tooltip("확인 도구")]
    private GameObject CheckTool;

    [SerializeField, Tooltip("구매 도구")]
    private GameObject BuyTool;

    [Header("정보 UI")]
    [SerializeField, Tooltip("재화 정보")]
    private TextMeshProUGUI GoodsInfo;

    [Header("정보 상자")]
    [SerializeField, Tooltip("정보 상자 객체")]
    private GameObject InfoBox;

    [SerializeField, Tooltip("기술 이미지")]
    private Image TechImage;

    [SerializeField, Tooltip("기술 이름")]
    private TextMeshProUGUI TechName;

    [SerializeField, Tooltip("기술 단계")]
    private Image[] LevelViewer;

    [SerializeField, Tooltip("기술 설명")]
    private TextMeshProUGUI TechLevelExplane;

    [SerializeField, Tooltip("기술 효과")]
    private TextMeshProUGUI TechLevelEffect;

    [SerializeField, Tooltip("재화 사용량")]
    private TextMeshProUGUI GoodsUseAmount;

    [Header("고정 변경 값")]
    [SerializeField, Tooltip("기술 현재 레벨 색")]
    private Color ActiveColor = Color.white;

    [SerializeField, Tooltip("기술 다음 레벨 색")]
    private Color UnActiveColor = Color.white;

    public void ResetAll()
    {
        Title.text = "마석 상점";
        PreviewMenu.SetActive(true);
        for (int i = 0; i < Shops.Length; i++)
            Shops[i].SetActive(false);
        BuyTool.SetActive(false);
        CheckTool.SetActive(true);
        InfoBox.SetActive(false);
        GoodsInfo.text = Goods.Get.ToString("0");
    }

    public void GoShop()
    {
        PreviewMenu.SetActive(false);
        for (int i = 0; i < Shops.Length; i++)
            Shops[i].SetActive(ShopFuncControllCenter.Instance.CurrentIndex == i);
        CheckTool.SetActive(false);
        BuyTool.SetActive(true);
        InfoBox.SetActive(false);
        ChangeTiitle();
    }

    private void ChangeTiitle()
    {
        Title.text +=
            " - "
            + ShopFuncControllCenter
                .Instance
                ._TechInfo[ShopFuncControllCenter.Instance.CurrentIndex]
                .Name;
    }

    public void BackShop()
    {
        ResetAll();
    }

    public void SetBox(SO_TechData data, int level)
    {
        TechImage.sprite = data.TechIcon;
        TechName.text = data.Name;
        TechLevelExplane.text = data.GetExplane(level);
        TechLevelEffect.text = data.GetEffect(level);
        GoodsUseAmount.text = $"<sprite=0>{data.UseAmount}";

        for (int i = 0; i < LevelViewer.Length; i++)
            LevelViewer[i].color = i < level ? ActiveColor : UnActiveColor;

        InfoBox.SetActive(true);
    }
}
