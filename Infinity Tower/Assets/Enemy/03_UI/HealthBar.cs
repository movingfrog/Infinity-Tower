using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HealthBarImage;
    float BarPositionY;
    bool Fly;

    public void Init(Vector3 position, float barposition, bool fly)
    {
        Fly = fly;
        BarPositionY = barposition / (fly ? 1f : 1.5f);
        transform.position = position - Vector3.up * barposition;
    }

    public void showHealth(float maxHP, float HP/*, float Damage*/)
    {
        HealthBarImage.fillAmount = HP / maxHP;
    }

    public void MovePosition(Vector3 position)
    {
        transform.position = position - Vector3.up * BarPositionY / (Fly ? 1f : 1.5f);
    }
}
