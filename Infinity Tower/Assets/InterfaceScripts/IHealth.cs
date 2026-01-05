using UnityEngine;

public interface IHealth
{
    // 체력 관련 변수
    public float HP { get; set; }
    public float MaxHP { get; set; }

    // 체력 관련 함수
    public void Hurt(float damage);
    public void Heal(float amount);
}
