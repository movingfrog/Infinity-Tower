using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public float MaxHP { get; set; }
    public float HP { get; set; }

    private void Start()
    {
        MaxHP = PlayerStatManager.instance.MaxHP;
        HP = MaxHP;
    }

    public void Die()
    {

    }

    public void Heal(float amount, GameObject healObject)
    {

    }

    public void Hurt(float damage)
    {
        HP -= damage;

        PlayerStatManager.instance.ChangeHealth(damage);
    }
}
