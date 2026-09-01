using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject TooltipObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipObject.SetActive(false);
    }
}
