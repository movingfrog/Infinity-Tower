using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunWeapon : Weapon
{
    private Coroutine shootingCoroutine;
    private Vector2 fireDirection;

    [Header("총기 설정")]
    public Transform shotPosition;
    public GameObject bulletPrefab;
    public int maxAmmo;

    public int currentAmmo { get; private set; }
    public bool isReload { get; private set; }

    protected override void Start()
    {
        base.Start();
        currentAmmo = maxAmmo;
        fireDirection = Vector2.right;
    }

    public override void Attack()
    {
        if (shootingCoroutine != null || isReload)
            return;

        TriggerHitEnchants();
        ani.SetBool("Attack", true);
        endAttack = true;
        shootingCoroutine = StartCoroutine(ShootingLoop());
    }

    private IEnumerator ShootingLoop()
    {
        ani.SetBool("isAuto", true);
        while (ExecuteShot() && isPushing)
        {
            yield return new WaitForSeconds(attackRate);
        }
        ani.SetBool("isAuto", false);
        shootingCoroutine = null;
    }

    private bool ExecuteShot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            float finalDamage = AttackDamageCaculator(
                damage + PlayerStatManager.instance.damage * 0.15f
            );
            fireDirection = (
                (Vector2)transform.parent.position - (Vector2)transform.parent.parent.position
            ).normalized;
            Quaternion rotation = Quaternion.Euler(0, 0, fireDirection.y * 90);

            Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            bullet.Init(
                shotPosition.position,
                rotation,
                fireDirection,
                finalDamage,
                TriggerAttackEnchant
            );
            return true;
        }

        TriggerReload();
        return false;
    }

    private void TriggerReload()
    {
        if (!isReload)
        {
            isReload = true;
            ani.SetTrigger("Reload");
        }
    }

    // 애니메이션 이벤트 리스너 등으로 호출될 재장전 완료 메서드
    public void CompleteReload()
    {
        currentAmmo = maxAmmo;
        isReload = false;

        if (isPushing)
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
        ani.SetBool("Attack", false);

        if (cooltimeCoroutine == null)
        {
            cooltimeCoroutine = StartCoroutine(StartCooltime());
        }
    }
}
