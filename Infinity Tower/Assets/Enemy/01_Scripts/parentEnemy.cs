using TMPro;
using UnityEngine;

[RequireComponent(typeof(DamageFlash))]
[RequireComponent(typeof(TimeBody))]
public abstract class parentEnemy : MonoBehaviour, IHealth
{
    [Header("체력 관련")]
    public bool isDie;
    public float defaultHP;
    public GameObject parentCanvas;
    public GameObject HealthBar;
    protected HealthBar healthBar;
    public GameObject hitText;

    [Header("공격 관련")]
    public float AttackDamage;
    public bool isAttack;

    protected Animator ani;
    private DamageFlash _damageFlash;

    public float HP { get; set; }
    public float MaxHP { get; set; }

    protected virtual void Awake()
    {
        ani = GetComponent<Animator>();
        _damageFlash = GetComponent<DamageFlash>();
        GameObject temp = Instantiate(HealthBar, parentCanvas.transform);
        healthBar = temp.GetComponent<HealthBar>();
        healthBar.Init(transform.position, GetComponent<SpriteRenderer>().bounds.extents.y);
        MaxHP = defaultHP; //이후에 레벨 공식 필요
        HP = MaxHP;
    }

    protected virtual void FixedUpdate()
    {
        if (TimeManager.Instance.isRewinding) return;

        Attack();
        Move();
    }

    protected abstract void Move();
    protected abstract void Attack();

    public void Hurt(float damage)
    {
        if(HP - damage > 0)
        {
            HP -= damage;
            ShowDamage(damage);
            healthBar.showHealth(MaxHP, HP);
            _damageFlash.CallDamageFlash();
        }
        else
        {
            Die();
        }
    }
    private void ShowDamage(float damage)
    {
        //데미지 텍스트 셋팅
        GameObject _hitText = Instantiate(hitText, parentCanvas.transform);
        Rigidbody2D rigid = _hitText.GetComponent<Rigidbody2D>();
        TextMeshPro text = _hitText.GetComponent<TextMeshPro>();

        //랜덤 값 생성
        float randX = Random.Range(0.5f, -.5f);

        _hitText.transform.position = transform.position;
        rigid.AddForce(new Vector2(randX, 5), ForceMode2D.Impulse);

        Destroy(_hitText, 1f);
    }

    public virtual void Die()
    {
        isDie = true;
        Destroy(healthBar.gameObject);
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
