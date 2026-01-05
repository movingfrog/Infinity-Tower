using UnityEngine;

public class parentEnemy : MonoBehaviour, IHealth
{
    public bool isDie;

    Animator ani;
    
    public float HP { get; set; }
    public float MaxHP { get; set; }

    protected void Awake()
    {
        ani = GetComponent<Animator>();
        HP = MaxHP;
    }

    public void Hurt(float damage)
    {
        if(HP - damage > 0)
        {
            HP -= damage;
            //이펙트 추가 필요
        }
        else
        {
            isDie = true;
            ani.SetTrigger("isDie");
        }
    }

    public void Heal(float amount)
    {
        if (!isDie)
        {
            HP += amount;
            //이펙트 추가 필요(체력 회복 이펙트와 체력 회복 텍스트)
        }
    }
}
