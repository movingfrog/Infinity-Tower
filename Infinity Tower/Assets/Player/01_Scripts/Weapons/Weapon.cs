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

    [Header("각인 설정")]
    public WeaponEnchant[] enchants = new WeaponEnchant[2];

    protected virtual void Awake()
    {
        TryGetComponent<Animator>(out ani);
        baseScale = Mathf.Abs(transform.localScale.x);
    }

    protected virtual void OnEnable()
    {
        OnEnableWeapon();

        TriggerEnchants(EnchantType.Stat);
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

    protected virtual float AttackDamageCaculator(float finalDamage)
    {
        if (Random.value <= PlayerStatManager.instance.Crit_Rate)
            finalDamage = finalDamage * PlayerStatManager.instance.Crit_Dmg;
        return finalDamage;
    }

    protected void TriggerAttackEnchant(GameObject enemy)
    {
        TriggerEnchants(EnchantType.Attack, enemy);
    }

    protected void TriggerHitEnchants()
    {
        TriggerEnchants(EnchantType.Ability);
    }

    protected void TriggerEnchants(EnchantType targetType, GameObject enemy = null)
    {
        for (int i = 0; i < enchants.Length; i++)
        {
            if (enchants[i] != null && enchants[i].Type == targetType)
            {
                enchants[i].WeaponUpgrade(this, enemy);
            }
        }
    }
}
