using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image HealthBarImage;
    float BarPositionY;

    public void Init(Vector3 position, float barposition)
    {
        BarPositionY = barposition;
        transform.position = position - Vector3.up * barposition / 2;
    }

    public void showHealth(float maxHP, float HP/*, float Damage*/)
    {
        HealthBarImage.fillAmount = HP / maxHP;
    }

    public void MovePosition(Vector3 position)
    {
        transform.position = position - Vector3.up * BarPositionY / 2;
    }
}
