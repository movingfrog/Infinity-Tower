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
    public WeaponData[] PrefabData;
    private Weapon weapon;

    private void Awake()
    {
        TryGetComponent(out PlayerAni);
        GetWeaponType();
    }

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Attack.started += StartAttack;
        InputManager.Instance.inputActions.Player.Attack.canceled += EndAttack;
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.equipEvent += AddEquipWeapon;
        }
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Attack.started -= StartAttack;
        InputManager.Instance.inputActions.Player.Attack.canceled -= EndAttack;
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.equipEvent -= AddEquipWeapon;
        }
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
            weapon.isPushing = true;
            if (
                PlayerAni.GetBool("isUsingSkill")
                || PlayerAni.GetBool("isDash")
                || PlayerStatManager.instance.currentState != PlayerState.Idle
            )
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
        weapon.isPushing = false;
        weapon.EndAttack();
    }

    private void GetWeaponType()
    {
        if (WeaponDirection.transform.childCount > 0)
        {
            weapon = WeaponDirection.GetComponentInChildren<Weapon>();
        }
    }

    private void AddEquipWeapon(Item item)
    {
        foreach (var w in PrefabData)
        {
            if (w.Type == item.Equips.Type)
            {
                GameObject weaponObject = Instantiate(
                    w.GetPrefabByLevel(item.level),
                    WeaponDirection.transform
                );
                if (weapon == null)
                {
                    weaponObject.SetActive(true);
                    GetWeaponType();
                }
            }
        }
    }
}
