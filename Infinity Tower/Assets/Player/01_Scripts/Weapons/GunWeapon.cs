using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunWeapon : Weapon
{
    private Coroutine shootingCoroutine;
    private Vector2 fireDirection;
    private bool _hasAuto;

    [Header("총기 설정")]
    public Transform shotPosition;
    public GameObject bulletPrefab;
    public int maxAmmo;
    public float fireRate;
    public bool hasAuto;

    public int currentAmmo { get; private set; }
    public bool isReload { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _hasAuto = hasAuto;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InputManager.Instance.inputActions.Player.FireMode.started += ToggleAutoFire;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InputManager.Instance.inputActions.Player.FireMode.started -= ToggleAutoFire;
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
        fireDirection = Vector2.right;
    }

    public void ToggleAutoFire(InputAction.CallbackContext callback)
    {
        if (_hasAuto)
            hasAuto = !hasAuto;
    }

    public override void Attack()
    {
        if (shootingCoroutine != null || isReload || CheckNoAmmo())
            return;

        ani.SetTrigger("Attack");
        endAttack = true;
        shootingCoroutine = StartCoroutine(ShootingLoop());
    }

    private IEnumerator ShootingLoop()
    {
        ani.SetBool("isAuto", hasAuto);
        while (ExecuteShot() && hasAuto && isPushing)
        {
            yield return new WaitForSeconds(fireRate);
        }
        ani.SetBool("isAuto", false);
        shootingCoroutine = null;
    }

    private bool ExecuteShot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            float finalDamage = damage + PlayerStatManager.instance.Damage * 0.15f;
            Quaternion rotation = Quaternion.Euler(0, 0, fireDirection.y * 90);

            Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            bullet.Init(shotPosition.position, rotation, fireDirection, finalDamage);
            return true;
        }

        TriggerReload();
        return false;
    }

    private void TriggerReload()
    {
        if (PlayerStatManager.instance.Ammo > 0 && !isReload)
        {
            isReload = true;
            ani.SetTrigger("Reload");
        }
    }

    // 애니메이션 이벤트 리스너 등으로 호출될 재장전 완료 메서드
    public void CompleteReload()
    {
        int ammoToFill =
            PlayerStatManager.instance.Ammo - maxAmmo >= 0
                ? maxAmmo
                : PlayerStatManager.instance.Ammo;
        PlayerStatManager.instance.UseAmmo(ammoToFill);
        currentAmmo = ammoToFill;
        isReload = false;

        if (isPushing && hasAuto)
            Attack();
    }

    public override void EndAttack()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
            ani.SetBool("isAuto", false);
        }

        if (cooltimeCoroutine == null)
        {
            cooltimeCoroutine = StartCoroutine(StartCooltime());
        }
    }

    public override void PositionMove(Vector2 value, float range)
    {
        float valueX = 1 - Mathf.Abs(GetSign(value.y));
        transform.parent.localPosition = new Vector2(range * valueX, range * GetSign(value.y));

        if (value.y == 0)
            transform.rotation = Quaternion.identity;
        else
            transform.rotation = Quaternion.Euler(
                0,
                0,
                90 * GetSign(value.y) * transform.parent.parent.localScale.x
            );

        fireDirection = new Vector2(
            valueX * transform.parent.parent.localScale.x,
            GetSign(value.y)
        );
    }

    private bool CheckNoAmmo() => currentAmmo == 0 && PlayerStatManager.instance.Ammo == 0;
}
