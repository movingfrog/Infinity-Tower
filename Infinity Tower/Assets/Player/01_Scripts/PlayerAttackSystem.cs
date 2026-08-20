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
            InventoryManager.Instance.EquipEvent += AddEquipWeapon;
            InventoryManager.Instance.UnEquipEvent += MinusEquipWeapon;
            InventoryManager.Instance.ChangeEvent += ChangeEquipWeapon;
        }
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Attack.started -= StartAttack;
        InputManager.Instance.inputActions.Player.Attack.canceled -= EndAttack;
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.EquipEvent -= AddEquipWeapon;
            InventoryManager.Instance.UnEquipEvent -= MinusEquipWeapon;
            InventoryManager.Instance.ChangeEvent -= ChangeEquipWeapon;
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

    /// <summary>
    /// 무기 추가 함수
    /// </summary>
    /// <param name="item">추가할 무기의 Item 및 Guid</param>
    private void AddEquipWeapon(InvenItem item)
    {
        if (item == null || item.item.Equips == null)
            return;

        if (GameManager.Instance.allWeaponGuid.Contains(item.weaponGuid))
        {
            Debug.Log("이미 생성한 무기입니다.");
            return;
        }

        foreach (var w in PrefabData)
        {
            if (w.Type == item.item.Equips.Type)
            {
                GameObject weaponObject = Instantiate(
                    w.GetPrefabByLevel(item.item.level),
                    WeaponDirection.transform
                );
                item.weaponGuid = System.Guid.NewGuid();
                weaponObject.GetComponent<Weapon>().Data.guid = item.weaponGuid;
                GameManager.Instance.allWeaponGuid.Add(item.weaponGuid);
                if (weapon == null)
                {
                    weaponObject.SetActive(true);
                    GetWeaponType();
                }
            }
        }
    }

    /// <summary>
    /// 무기 장착 해제 함수
    /// </summary>
    /// <param name="targetItem">장착 해제할 무기</param>
    /// <param name="OtherEquipItem">다른 장착된 무기</param>
    /// <returns>장착 해제 성공 여부</returns>
    private bool MinusEquipWeapon(InvenItem targetItem, InvenItem OtherEquipItem)
    {
        if (
            targetItem.item == null
            || targetItem.item.Equips == null
            || OtherEquipItem.item == null
            || OtherEquipItem.item.Equips == null
        )
        {
            Debug.Log("실행 안됨");
            return false;
        }
        var w = WeaponDirection.GetComponentInChildren<Weapon>();
        if (w.Data.guid == targetItem.weaponGuid) //인챈트 확인도 필요
            return ChangeEquipWeapon(OtherEquipItem.item, OtherEquipItem.weaponGuid);
        return false;
    }

    /// <summary>
    /// 무기 교체 함수
    /// </summary>
    /// <param name="item">교체 될 무기</param>
    /// <param name="guid">교체 될 무기의 GUID</param>
    /// <returns>교체 성공 여부</returns>
    private bool ChangeEquipWeapon(Item item, System.Guid guid)
    {
        if (item == null || item.Equips == null)
        {
            Debug.Log("실행 안됨");
            return false;
        }
        if (WeaponDirection.transform.childCount > 1)
        {
            GameObject targetObject = null;
            foreach (var w in WeaponDirection.GetComponentsInChildren<Weapon>(true))
            {
                w.gameObject.SetActive(false);
                if (w.Type == item.Equips.Type && w.Level == item.level)
                {
                    targetObject = w.gameObject;
                }
            }
            if (targetObject != null)
            {
                targetObject.SetActive(true);
                GetWeaponType();
                return true;
            }
        }
        return false;
    }
}
