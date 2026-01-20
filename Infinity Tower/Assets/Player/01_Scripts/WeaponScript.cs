using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    Coroutine ShotingCoroutine;
    Animator ani;
    float Scale;

    public WeaponType type;
    public bool endAttack;
    public float damage;
    public float AttackCoolTime;
    public LayerMask EnemyLayer;
    [Foldout("근접 공격")]
    public Vector2 attackSize;
    [Foldout("근접 공격")]
    public Vector2 attackRange;
    [Foldout("총기 공격 관련")]
    public bool hasAuto;
    [Foldout("총기 공격 관련")]
    public int MaxAmmo;
    public int currentAmmo { get; private set; }

    private void Awake()
    {
        TryGetComponent(out ani);
        Scale = Mathf.Abs(transform.localScale.x);
    }
    private void Start()
    {
        getAmmo(true);
    }

    private void OnEnable()
    {
        ani.SetBool("isGet", true);
    }
    private void OnDisable()
    {
        ani.SetBool("isGet", false);
    }

    public bool Shot()
    {
        if(currentAmmo > 0)
        {
            currentAmmo -= 1;
            Debug.Log(currentAmmo);
            return true;
        }
        else
        {
            if (PlayerStatManager.instance.Ammo > 0)
            {
                ani.SetTrigger("Reload");
                getAmmo();
                return true;
            }
            else return false;
        }
    }
    void getAmmo(bool isFirst = false)
    {
        int ammo = PlayerStatManager.instance.Ammo - MaxAmmo >= 0 || isFirst ? MaxAmmo : PlayerStatManager.instance.Ammo;
        if(!isFirst)PlayerStatManager.instance.UseAmmo(ammo);
        currentAmmo = ammo;
    }
    public void PositionMove(Vector2 value, float AttackRange)
    {
        float ValueX = 1 - Mathf.Abs(Sign(value.y));
        transform.parent.localPosition = new Vector2(AttackRange * ValueX, AttackRange * Sign(value.y));
        if(type == WeaponType.Sword)
        {
            if (value.y < 0) transform.localScale = Scale * new Vector2(1, -1);
            else transform.localScale = Scale * new Vector2(ValueX != 0 ? 1 : -1, 1);
        }
        else
        {
            if (value.y == 0) transform.rotation = Quaternion.identity;
            else transform.rotation = Quaternion.Euler(new Vector3(0, 0,90 * Sign(value.y)));
        }
    }
    bool checkGunAttack() => currentAmmo == 0 &&PlayerStatManager.instance.Ammo == 0;
    public void Attack()
    {
        if (type == WeaponType.Gun && checkGunAttack()) return;
        float finalDamage = PlayerStatManager.instance.Damage + damage;
        ani.SetTrigger("Attack");
        endAttack = true;
        switch (type)
        {
            case WeaponType.Sword:
                Collider2D[] EnemyColl = Physics2D.OverlapBoxAll(transform.position + computeAttackRange(), attackSize, Sign(transform.localPosition.y) == 0 ? 0 : 90, EnemyLayer);
                foreach (var enemy in EnemyColl) enemy.GetComponent<IHealth>().Hurt(finalDamage);
                break;
            case WeaponType.Gun:
                ShotingCoroutine = StartCoroutine(Shoting());
                break;
        }
    }
    IEnumerator Shoting()
    {
        ani.SetBool("isAuto", hasAuto);
        while (Shot())
        {
            yield return new WaitForSeconds(AttackCoolTime);
        }
        ani.SetBool("isAuto", false);
        Endattack();
    }
    public void Endattack()
    {
        if (!endAttack) return;
        if (ShotingCoroutine != null)
        {
            StopCoroutine(ShotingCoroutine);
            ani.SetBool("isAuto", false);
        }
        StartCoroutine(AttackCool());
    }
    IEnumerator AttackCool()
    {
        yield return new WaitForSeconds(AttackCoolTime);
        endAttack = false;
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
        Gizmos.DrawCube(transform.parent.parent.position + computeAttackRange(), vec);
    }
}
