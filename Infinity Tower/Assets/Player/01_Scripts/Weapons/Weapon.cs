using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class WeaponObjectData
{
    [Header("GUID")]
    public System.Guid guid;

    [Header("각인 설정")]
    public WeaponEnchant[] enchants = new WeaponEnchant[2];

    public bool CanEnchant()
    {
        for (int i = 0; i < enchants.Length; i++)
        {
            if (enchants[i] == null)
                return true;
        }
        return false;
    }

    public void EquipEnchant(WeaponEnchant enchant)
    {
        for (int i = 0; i < enchants.Length; i++)
        {
            if (enchants[i] == null)
            {
                enchants[i] = enchant;
                return;
            }
        }
    }
}

public abstract class Weapon : MonoBehaviour
{
    public WeaponObjectData Data;

    protected Animator ani;
    protected float baseScale;
    public Coroutine cooltimeCoroutine { get; protected set; }

    [Header("무기 특성 설정")]
    public WeaponType Type;
    public ItemLevel Level;

    [Header("무기 설정")]
    public float damage;
    public float attackRate;

    public bool endAttack { get; protected set; }
    public bool isPushing { get; set; }

    Transform WeaponPos;
    Transform Player;

    public void InitializeWeapon(WeaponObjectData data)
    {
        Data = data;
    }

    protected virtual void Awake()
    {
        TryGetComponent<Animator>(out ani);
        baseScale = Mathf.Abs(transform.localScale.x);
        WeaponPos = transform.parent;
        Player = transform.parent.parent;
    }

    protected virtual void Start()
    {
        Debug.Log(Data.guid != System.Guid.Empty);
        if (Data.guid != System.Guid.Empty)
            GameManager.Instance.allWeaponData.Add(Data.guid, Data);
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

    public virtual void MoveWeapon()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(Player.position).z;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        float dx = mouseWorldPos.x - Player.position.x;
        float dy = mouseWorldPos.y - Player.position.y;
        float theta = Mathf.Atan2(dy, dx);
        float thetaDeg = theta * Mathf.Rad2Deg;

        float playerSign = Mathf.Sign(Player.localScale.x);

        // 위치
        float ox = Player.position.x + .45f * Mathf.Cos(theta);
        float oy = Player.position.y + .45f * Mathf.Sin(theta);
        WeaponPos.position = new Vector3(ox, oy, WeaponPos.position.z);

        float localTargetDeg = (playerSign < 0) ? (180f - thetaDeg) : thetaDeg;

        WeaponPos.localRotation = Quaternion.Euler(0f, 0f, localTargetDeg);
        if (WeaponPos.localEulerAngles.z >= 90f && WeaponPos.localEulerAngles.z <= 270f)
        {
            Debug.Log("slfkdjsfsl");
            WeaponPos.localScale = new Vector3(1, -1, 1f);
        }
        else
        {
            Debug.Log("jjjjjjjjjj");
            WeaponPos.localScale = new Vector3(1, 1, 1f);
        }
    }

    public void UnEquipEnchant(int slotNum)
    {
        if (slotNum < 0 || slotNum >= Data.enchants.Length)
            return;
        Data.enchants[slotNum] = null;
    }

    protected IEnumerator StartCooltime()
    {
        yield return new WaitForSeconds(attackRate);
        cooltimeCoroutine = null;
    }

    protected float GetSign(float value) =>
        value > 0 ? 1f
        : value < 0 ? -1f
        : 0f;

    protected virtual float AttackDamageCaculator(float finalDamage)
    {
        if (UnityEngine.Random.value <= PlayerStatManager.instance.Crit_Rate)
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
        for (int i = 0; i < Data.enchants.Length; i++)
        {
            if (Data.enchants[i] != null && Data.enchants[i].Type == targetType)
            {
                Data.enchants[i].WeaponUpgrade(this, enemy);
            }
        }
    }
}
