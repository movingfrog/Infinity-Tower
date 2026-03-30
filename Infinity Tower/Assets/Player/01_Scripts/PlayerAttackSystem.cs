using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponType
{
    Sword = 0,
    Gun = 1,
    Bow = 2,
    Spear = 3,
    None = 4,
}

public class PlayerAttackSystem : MonoBehaviour
{
    Animator PlayerAni;
    bool isPusing;

    [Header("공격 판정")]
    public float attackDirection;
    public GameObject WeaponDirection;

    [Header("공격 형태")]
    public GameObject[] weaponPrefeb;
    private WeaponScript weapon;

    private void Awake()
    {
        TryGetComponent(out PlayerAni);
        GetWeaponType();
    }

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Attack.started += StartAttack;
        InputManager.Instance.inputActions.Player.Attack.canceled += EndAttack;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Attack.started -= StartAttack;
        InputManager.Instance.inputActions.Player.Attack.canceled -= EndAttack;
    }

    public void OnMove(InputValue value)
    {
        Vector2 movement = value.Get<Vector2>();
        weapon.PositionMove(movement, attackDirection);
    }

    public void OnFireMode()
    {
        if (isPusing)
            return;
        if (weapon != null)
            weapon.FireSelect();
        isPusing = true;
        StartCoroutine(waitPusing());
    }

    IEnumerator waitPusing()
    {
        yield return new WaitForSeconds(.5f);
        isPusing = false;
    }

    private void StartAttack(InputAction.CallbackContext callback)
    {
        if (weapon != null)
        {
            weapon.isPusing = true;
            if (PlayerAni.GetBool("isUsingSkill") || PlayerAni.GetBool("isDash"))
                return;
            if (!weapon.endAttack)
            {
                weapon.Attack();
            }
        }
    }

    private void EndAttack(InputAction.CallbackContext callback)
    {
        if (weapon == null)
            return;
        weapon.isPusing = false;
        weapon.Endattack();
    }

    private void GetWeaponType()
    {
        if (WeaponDirection.transform.childCount > 0)
        {
            weapon = WeaponDirection.GetComponentInChildren<WeaponScript>();
        }
    }
}
