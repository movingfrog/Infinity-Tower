using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponType { Sword = 0, Gun = 1, Bow = 2, Pole = 3, None = 4 }
public class PlayerAttackSystem : MonoBehaviour
{
    InputSystem_Actions input;
    Animator PlayerAni;

    [Header("공격 판정")]
    public float attackDirection;
    public GameObject WeaponDirection;
    [Header("공격 형태")]
    public GameObject[] weaponPrefeb;
    private WeaponScript weapon;

    private void Awake()
    {
        input = new InputSystem_Actions();
        TryGetComponent(out PlayerAni);
        GetWeaponType();
    }

    private void OnEnable()
    {
        input.Player.Enable();

        input.Player.Attack.started += StartAttack;
        input.Player.Attack.canceled += EndAttack;
    }
    private void OnDisable()
    {
        input.Player.Attack.started -= StartAttack;
        input.Player.Attack.canceled -= EndAttack;

        input.Player.Disable();
    }

    public void OnMove(InputValue value)
    {
        Vector2 movement = value.Get<Vector2>();
        weapon.PositionMove(movement, attackDirection);
    }

    private void StartAttack(InputAction.CallbackContext callback)
    {
        if (PlayerAni.GetBool("isUsingSkill") || PlayerAni.GetBool("isDash")) EndAttack(callback);
        if (weapon != null && !weapon.endAttack)
        {
            weapon.Attack();
        }
    }
    private void EndAttack(InputAction.CallbackContext callback)
    {
        weapon.Endattack();
    }

    private void GetWeaponType()
    {
        if(WeaponDirection.transform.childCount > 0)
        {
            weapon = WeaponDirection.GetComponentInChildren<WeaponScript>();
        }
    }
}