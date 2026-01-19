using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public WeaponType type;
    public float damage;
    public float AttackCoolTime;
    public bool hasAuto;
    public int currentAmmo { get; private set; }
    public int MaxAmmo;

    public bool Shot()
    {
        if(currentAmmo > 0)
        {
            if (PlayerStatManager.instance.Ammo > 0)
            {

                return true;
            }
            else return false;
        }
        else
        {
            return true;
        }
    }
}
