using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UIElements;

public class WeaponScript : MonoBehaviour
{
    Coroutine ChargingCoroutine;
    Coroutine ShottingCoroutine;
    Coroutine CooltimeCoroutine;
    GameObject arrow;
    Animator ani;
    float Scale;

    public WeaponType type;
    public bool endAttack;
    public bool isPusing;
    public float damage;
    public float attackRate;
    public LayerMask EnemyLayer;
    [Foldout("근접 공격")]
    public Vector2 attackSize;
    [Foldout("근접 공격")]
    public Vector2 attackRange;
    [Foldout("원거리 공격 관련")]
    public Transform shotPosition;
    [Foldout("원거리 공격 관련")]
    public GameObject bulletPrefeb;
    [Foldout("총기 공격 관련")]
    public bool isReload;
    [Foldout("총기 공격 관련")]
    public bool hasAuto;
    private bool _hasAuto;
    [Foldout("총기 공격 관련")]
    public int MaxAmmo;
    [Foldout("총기 공격 관련")]
    public float fireRate;
    [Foldout("활 공격 관련")]
    public bool isCrossBow;
    public int currentAmmo { get; private set; }
    private Vector2 FValue;

    private void Awake()
    {
        TryGetComponent(out ani);
        Scale = Mathf.Abs(transform.localScale.x);
        _hasAuto = hasAuto;
    }
    private void Start()
    {
        currentAmmo = MaxAmmo;
        FValue = Vector2.right;
    }

    private void OnEnable()
    {
        ani.SetBool("isGet", true);
    }
    private void OnDisable()
    {
        ani.SetBool("isGet", false);
    }

    public void FireSelect()
    {
        if (_hasAuto)
        {
            hasAuto = !hasAuto;
        }
    }
    private bool Shot()
    {
        if(currentAmmo > 0)
        {
            currentAmmo -= 1;
            float finalDamage = damage + PlayerStatManager.instance.Damage * .15f; //총기 데미지 공식은 15% 낮춤
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, FValue.y * 90));
            Bullet bullet = Instantiate(bulletPrefeb).GetComponent<Bullet>();
            bullet.Init(shotPosition.position, rotation, FValue, finalDamage);
            return true;
        }
        else
        {
            if (PlayerStatManager.instance.Ammo > 0 && !isReload)
            {
                isReload = true;
                ani.SetTrigger("Reload");
            }
            return false;
        }
    }
    private void ShotArrow(float Percent)
    {
        ani.SetTrigger("Shot");
        arrow.transform.SetParent(null, true);
        arrow.transform.localScale = Vector3.one;
        float finalDamage = (damage + PlayerStatManager.instance.Damage) * (.3f + Percent * .7f);
        Arrow _arrow = arrow.GetComponent<Arrow>();
        _arrow.Shot(FValue, Percent, finalDamage);
    }
    public void getAmmo()
    {
        int ammo = PlayerStatManager.instance.Ammo - MaxAmmo >= 0 ? MaxAmmo : PlayerStatManager.instance.Ammo;
        PlayerStatManager.instance.UseAmmo(ammo);
        currentAmmo = ammo;
        isReload = false;
        if(isPusing && hasAuto) Attack();
    }
    public void PositionMove(Vector2 value, float AttackRange)
    {
        float ValueX = 1 - Mathf.Abs(Sign(value.y));
        transform.parent.localPosition = new Vector2(AttackRange * ValueX, AttackRange * Sign(value.y));
        if(type == WeaponType.Sword || type == WeaponType.Spear)
        {
            if(type == WeaponType.Spear)
            {
                ani.SetInteger("X", (int)(ValueX * transform.parent.parent.localScale.x));
                ani.SetInteger("Y", (int)Sign(value.y));
            }
            if (value.y < 0) transform.localScale = Scale * new Vector2(1, -1);
            else transform.localScale = Scale * new Vector2(ValueX != 0 ? 1 : -1, 1);
        }
        else
        {
            if (value.y == 0) transform.rotation = Quaternion.identity;
            else transform.rotation = Quaternion.Euler(new Vector3(0, 0,90 * Sign(value.y) * transform.parent.parent.localScale.x));
            FValue = new Vector2(ValueX * transform.parent.parent.localScale.x, Sign(value.y));
        }
    }
    bool checkGunAttack() => currentAmmo == 0 &&PlayerStatManager.instance.Ammo == 0;
    public void Attack()
    {
        if (ShottingCoroutine != null || !isPusing || isReload) return;
        if (type == WeaponType.Gun && checkGunAttack()) return;
        float finalDamage = PlayerStatManager.instance.Damage + damage;
        ani.SetTrigger("Attack");
        endAttack = true;
        switch (type)
        {
            case WeaponType.Sword:
                Vector3 currentPosition = new Vector3(transform.parent.parent.position.x, transform.position.y, 0);
                Collider2D[] EnemyColl = Physics2D.OverlapBoxAll(currentPosition + computeAttackRange(), attackSize, Sign(transform.localPosition.y) == 0 ? 0 : 90, EnemyLayer);
                foreach (var enemy in EnemyColl) enemy.GetComponent<IHealth>().Hurt(finalDamage);
                break;
            case WeaponType.Gun:
                ShottingCoroutine = StartCoroutine(Shoting());
                break;
            case WeaponType.Spear:
                Vector3 SpearPosition = new Vector3(transform.parent.parent.position.x, transform.position.y, 0);
                Collider2D[] _EnemeyColl = Physics2D.OverlapBoxAll(SpearPosition + computeAttackRange(), attackSize, ani.GetInteger("Y") == 0 ? 0 : 90, EnemyLayer);
                foreach (var enemy in _EnemeyColl) enemy.GetComponent<IHealth>().Hurt(finalDamage);
                break;
            case WeaponType.Bow:
                ChargingCoroutine = StartCoroutine(Charging());
                break;
        }
    }
    IEnumerator Shoting()
    {
        ani.SetBool("isAuto", hasAuto);
        while (Shot() && hasAuto)
        {
            yield return new WaitForSeconds(fireRate);
        }
        ani.SetBool("isAuto", false);
        ShottingCoroutine = null;
    }
    IEnumerator Charging()
    {
        float temp = 0;
        arrow = Instantiate(bulletPrefeb, shotPosition);
        arrow.transform.localPosition = Vector2.zero;

        Rigidbody2D arrowRB = arrow.GetComponent<Rigidbody2D>();
        if(arrowRB != null) arrowRB.simulated = false;

        while (isPusing)
        {
            if (isCrossBow)
            {
                temp = 1;
                break;
            }
            temp = Mathf.Min(temp + Time.deltaTime, 1f);
            yield return null;
        }

        if (arrowRB != null) arrowRB.simulated = true;
        ShotArrow(temp);
    }

    public void Endattack()
    {
        if (ShottingCoroutine != null)
        {
            StopCoroutine(ShottingCoroutine);
            ShottingCoroutine = null;
            ani.SetBool("isAuto", false);
        }
        if (type == WeaponType.Bow) isPusing = false;
        if(CooltimeCoroutine != null) return;
        CooltimeCoroutine = StartCoroutine(AttackCool());
    }
    IEnumerator AttackCool()
    {
        yield return new WaitForSeconds(attackRate);
        endAttack = false;
        CooltimeCoroutine = null;
    }

    private Vector3 computeAttackRange() //무기 이미지에 따른 무기 공격 위치의 중심 점 반환
    {
        Vector3 weaponPosition = transform.parent.localPosition;
        float x = Sign(weaponPosition.x) * attackRange.x * 2;
        float y = Sign(weaponPosition.y) * attackRange.y * 2;
        Vector3 position = new Vector3((weaponPosition.x + x) * transform.parent.parent.localScale.x, weaponPosition.y + y, 0);
        return position;
    }
    private float Sign(float value) => value > 0 ? 1f : value < 0 ? -1f : 0; //음수,양수,0 반환
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green * new Color(1, 1, 1, .1f);
        Vector2 vec = Sign(transform.parent.localPosition.y) == 0 ? attackSize : new Vector2(attackSize.y, attackSize.x);
        Vector3 currentPosition = new Vector3(transform.parent.parent.position.x, transform.position.y, 0);
        Gizmos.DrawCube(currentPosition + computeAttackRange(), vec);
    }
}
