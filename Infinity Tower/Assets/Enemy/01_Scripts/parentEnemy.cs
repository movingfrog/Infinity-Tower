using System;
using System.Collections;
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
    public GameObject _hitText;

    [Header("공격 관련")]
    public float AttackDamage;
    public bool isAttack;

    protected Animator ani;
    private DamageFlash _damageFlash;

    public float HP { get; set; }
    public float MaxHP { get; set; }
    public GameObject hitText { get; set; }

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
            ShowDamage(damage, Color.white);
            healthBar.showHealth(MaxHP, HP);
            _damageFlash.CallDamageFlash();
        }
        else
        {
            Die();
        }
    }
    private void ShowDamage(float damage, Color color)
    {
        //데미지 텍스트 셋팅
        GameObject hitTextInstance = Instantiate(_hitText, parentCanvas.transform);
        Rigidbody2D rigid = hitTextInstance.GetComponent<Rigidbody2D>();
        TextMeshProUGUI text = hitTextInstance.GetComponent<TextMeshProUGUI>();

        //랜덤 값 생성
        float randX = UnityEngine.Random.Range(0.5f, -.5f);

        text.color = color;
        text.text = damage.ToString("0");
        hitTextInstance.transform.position = transform.position;
        rigid.AddForce(new Vector2(randX, 5), ForceMode2D.Impulse);

        Destroy(hitTextInstance, 1f);
    }

    public virtual void Die()
    {
        isDie = true;
        Destroy(healthBar.gameObject);
        ani.SetTrigger("isDie");
    }

    protected IEnumerator waitAttackCool(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public void Heal(float amount, GameObject healObject)
    {
        if (!isDie)
        {
            HP += amount;
            ShowDamage(amount, Color.yellow);
            GameObject healEffect = Instantiate(healObject);
            healEffect.transform.position = transform.position;
            Destroy(healEffect, .5f);
        }
    }
}
