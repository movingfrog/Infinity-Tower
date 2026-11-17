using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StoneInfoIconToggleTooltip : MonoBehaviour, IPointerClickHandler
{
    [Header("클릭 시 켜지고 꺼질 자식 오브젝트")]
    [SerializeField]
    private GameObject childObject;

    private PointerEventData pointerData;
    private readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

    private void OnEnable()
    {
        if (childObject != null)
            childObject.SetActive(false);

        InputManager.Instance.inputActions.UI.PressAction.performed += OnPressPerformed;
    }

    private void OnDisable()
    {
        // 오브젝트가 꺼질 때 상태를 정리
        Close();
    }

    // 자기 자신(또는 하위 UI)을 클릭했을 때: 토글
    public void OnPointerClick(PointerEventData eventData)
    {
        if (childObject.activeSelf)
            Close();
        else
            Open();
    }

    private void Open()
    {
        childObject.SetActive(true);
    }

    private void Close()
    {
        if (childObject != null)
            childObject.SetActive(false);

        if (InputManager.Instance != null)
            InputManager.Instance.inputActions.UI.PressAction.performed -= OnPressPerformed;
    }

    // 열려 있는 동안 어디든 눌렀을 때 호출됨
    private void OnPressPerformed(InputAction.CallbackContext ctx)
    {
        var pointer = Pointer.current;
        if (pointer == null)
            return;

        // 자기 자신을 누른 경우는 OnPointerClick이 처리하므로 무시
        if (IsPointerOnSelf(pointer.position.ReadValue()))
            return;

        // 그 외 다른 곳을 누르면 닫기
        Close();
    }

    private bool IsPointerOnSelf(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
            return false;

        if (pointerData == null)
            pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = screenPosition;

        raycastResults.Clear();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.transform.IsChildOf(transform))
                return true;
        }
        return false;
    }
}
