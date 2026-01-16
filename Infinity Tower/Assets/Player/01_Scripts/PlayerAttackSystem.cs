using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponType { Sword = 0, Gun = 1, Bow = 2, Pole = 3, None = 4 }
public class PlayerAttackSystem : MonoBehaviour
{
    Animator ani;
    Coroutine attackCoroutine;

    public float AttackCoolTime;
    public float attackDirection;
    public bool endAttack = true;
    public GameObject WeaponDirection;

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
        Debug.Log(ani);
        WeaponProperty = WeaponType.None;
        Debug.Log(ani.GetBool("isGet"));
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
        if (GetComponent<Animator>().GetBool("isUsingSkill")) return;
        if (ani.GetBool("isGet") && endAttack)
        {
            endAttack = false;
            ani.SetTrigger("Attack");
            attackCoroutine = StartCoroutine(AttackCool());
        }
        else //테스트 용
        {
            WeaponProperty = WeaponType.Sword;
            ani.GetBool("isGet");
        }
    }
    IEnumerator AttackCool()
    {
        yield return new WaitForSeconds(AttackCoolTime);
        endAttack = true;
    }
}