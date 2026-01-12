using UnityEngine;

public abstract class parentEnemy : MonoBehaviour, IHealth
{
    public bool isDie;
    public bool isAttack;
    public GameObject hitEffect;

    protected Animator ani;
    
    public float HP { get; set; }
    public float MaxHP { get; set; }

    protected virtual void Awake()
    {
        ani = GetComponent<Animator>();
        HP = MaxHP;
    }

    protected abstract void Move();
    protected abstract void Attack();

    public void Hurt(float damage)
    {
        if(HP - damage > 0)
        {
            HP -= damage; 
            GameObject _hitEffect = Instantiate(hitEffect);
            _hitEffect.transform.position = transform.position;
            Destroy(_hitEffect, .5f);
        }
        else
        {
            Die();
        }
    }

    public virtual void Die()
    {
        isDie = true;
        ani.SetTrigger("isDie");
    }

    public void Heal(float amount, GameObject healObject)
    {
        if (!isDie)
        {
            HP += amount;
            GameObject healEffect = Instantiate(healObject);
            healEffect.transform.position = transform.position;
            Destroy(healEffect, .5f);
        }
    }
}
