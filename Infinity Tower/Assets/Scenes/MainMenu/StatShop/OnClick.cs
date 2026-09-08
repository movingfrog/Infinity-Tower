using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnClick : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnClickAction;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickAction?.Invoke();
    }
}
