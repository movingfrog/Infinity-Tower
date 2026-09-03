using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HealthBarImage;

    RectTransform rectTransform;
    float offsetY; // 최종 y축 오프셋 (콜라이더 절반높이 + 체력바 절반높이 + 여백)

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <param name="colliderExtentY">적 콜라이더의 세로 절반 길이 (bounds.extents.y)</param>
    /// <param name="padding">추가로 띄울 여백 (선택)</param>
    public void Init(Vector3 position, float colliderExtentY, float padding = 0.1f)
    {
        // 체력바 자신의 World 기준 절반 높이 계산
        // (World Space Canvas라 lossyScale 반영 필요)
        float barHalfHeight = (rectTransform.rect.height * rectTransform.lossyScale.y) * 0.5f;

        offsetY = colliderExtentY + barHalfHeight + padding;

        transform.position = position - Vector3.up * offsetY;
    }

    public void showHealth(float maxHP, float HP)
    {
        HealthBarImage.fillAmount = HP / maxHP;
    }

    public void MovePosition(Vector3 position)
    {
        transform.position = position - Vector3.up * offsetY;
    }
}
