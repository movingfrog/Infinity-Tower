using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Animator ani;
    protected float baseScale;
    protected Coroutine cooltimeCoroutine;

    [Header("무기 특성 설정")]
    public WeaponType Type;
    public ItemLevel Level;

    [Header("무기 설정")]
    public float damage;
    public float attackRate;

    public bool endAttack { get; protected set; }
    public bool isPushing { get; set; }

    protected virtual void Awake()
    {
        TryGetComponent<Animator>(out ani);
        baseScale = Mathf.Abs(transform.localScale.x);
    }

    protected virtual void OnEnable()
    {
        OnEnableWeapon();
    }

    protected virtual void OnDisable()
    {
        OnDisableWeapon();
    }

    public virtual void OnEnableWeapon() => ani.SetBool("isGet", true);

    public virtual void OnDisableWeapon() => ani.SetBool("isGet", false);

    public abstract void Attack();
    public abstract void EndAttack();
    public abstract void PositionMove(Vector2 value, float attackRange);

    protected IEnumerator StartCooltime()
    {
        yield return new WaitForSeconds(attackRate);
        endAttack = false;
        cooltimeCoroutine = null;
    }

    protected float GetSign(float value) =>
        value > 0 ? 1f
        : value < 0 ? -1f
        : 0f;

    protected virtual float AttackDamageCapculator(float finalDamage)
    {
        if (Random.Range(0, 1.0f) <= PlayerStatManager.instance.Crit_Rate)
            finalDamage = finalDamage * PlayerStatManager.instance.Crit_Dmg;
        return finalDamage;
    }
}
