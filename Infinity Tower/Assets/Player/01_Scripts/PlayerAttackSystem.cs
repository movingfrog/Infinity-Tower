using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponType { Sword = 0, Gun = 1, Bow = 2, Pole = 3, None = 4 }
public class PlayerAttackSystem : MonoBehaviour
{
    Animator PlayerAni;
    Animator ani;

    [Header("공격 범위")]
    public Vector2[] attackSize;
    public Vector2[] attackRange;
    public LayerMask EnemyLayer;
    [Header("공격 판정")]
    public float[] attackDamage;
    public float AttackCoolTime;
    public float attackDirection;
    public bool endAttack = true;
    public GameObject WeaponDirection;
    [Header("공격 형태")]
    public GameObject[] weaponPrefeb;
    private WeaponScript weapon;
    public WeaponType type; //값 저장용
    public WeaponType WeaponProperty //값 활용 용
    {
        get => type;
        set
        {
            type = value;
            ani.SetBool("isGet", type != WeaponType.None);
            ani.SetInteger("WeaponCount", (int)type);
        }
    }

    private void Awake()
    {
        WeaponDirection.TryGetComponent(out ani);
        TryGetComponent(out PlayerAni);
        GetWeaponType();
    }

    public void OnMove(InputValue value)
    {
        Vector2 movement = value.Get<Vector2>();
        if (movement.y != 0)
        {
            if (movement.y > 0)
            {
                WeaponDirection.transform.localScale = new Vector2(-1, 1);
                WeaponDirection.transform.localPosition = new Vector2(0, attackDirection);
            }
            else
            {
                WeaponDirection.transform.localScale = new Vector2(1, -1);
                WeaponDirection.transform.localPosition = new Vector2(0, -attackDirection);
            }
        }
        else
        {
            WeaponDirection.transform.localScale = Vector2.one;
            WeaponDirection.transform.localPosition = new Vector2(attackDirection, 0);
        }
    }

    public void OnAttack()
    {
        if (PlayerAni.GetBool("isUsingSkill") || PlayerAni.GetBool("isDash")) return;
        if (ani.GetBool("isGet") && endAttack)
        {
            endAttack = false;
            ani.SetTrigger("Attack");
            StartCoroutine(AttackCool());
            InsertDamage(WeaponProperty);
        }
    }
    private void InsertDamage(WeaponType weaponType)
    {
        float finalDamage = PlayerStatManager.instance.Damage + weapon.damage;
        switch (weaponType)
        {
            case WeaponType.Sword:
                Collider2D[] EnemyColl = Physics2D.OverlapBoxAll(transform.position + computeAttackRange(WeaponType.Sword), attackSize[(int)WeaponType.Sword], Sign(transform.localPosition.y) == 0 ? 0 : 90, EnemyLayer);
                foreach (var enemy in EnemyColl) enemy.GetComponent<IHealth>().Hurt(finalDamage);
                break;
        }
    }
    IEnumerator AttackCool()
    {
        yield return new WaitForSeconds(AttackCoolTime);
        endAttack = true;
    }

    private void GetWeaponType()
    {
        if(WeaponDirection.transform.childCount > 0)
        {
            weapon = WeaponDirection.GetComponentInChildren<WeaponScript>();
            WeaponProperty = weapon.type;
        }
        else
        {
            weapon = Instantiate(weaponPrefeb[(int)WeaponProperty], WeaponDirection.transform).GetComponent<WeaponScript>();
        }
    }

    private Vector3 computeAttackRange(WeaponType weaponCode)
    {
        Vector3 weaponPosition = WeaponDirection.transform.localPosition;
        float x = Sign(weaponPosition.x) * attackRange[(int)weaponCode].x * 2;
        float y = Sign(weaponPosition.y) * attackRange[(int)weaponCode].y * 2;
        Vector3 position = new Vector3((weaponPosition.x + x) * transform.localScale.x,weaponPosition.y + y, 0);
        return position;
    }
    private float Sign(float value) => value > 0 ? 1f : value < 0 ? -1f : 0;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue * new Color(1, 1, 1, .1f);
        Vector2 vec = Sign(transform.localPosition.y) == 0 ? attackSize[(int)WeaponProperty] : new Vector2(attackSize[(int)WeaponProperty].y, attackSize[(int)WeaponProperty].x);
        Gizmos.DrawCube(transform.position + computeAttackRange(WeaponType.Sword), vec);
    }
}