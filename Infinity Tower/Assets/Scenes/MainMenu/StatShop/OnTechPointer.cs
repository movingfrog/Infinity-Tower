using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnTechPointer : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image image;

    [field: SerializeField]
    public SO_TechData Data { get; private set; }

    [field: SerializeField, Tooltip("최소값을 1을 넘도록 해주세요")]
    public int Level;

    private void Awake()
    {
        if (image.sprite != Data.TechIcon)
            image.sprite = Data.TechIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShopFuncControllCenter.Instance.ChnageData(Data, Level);
    }
}
