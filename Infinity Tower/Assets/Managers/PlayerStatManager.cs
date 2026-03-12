using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatManager : MonoBehaviour
{
    private static PlayerStatManager Instance;
    public static PlayerStatManager instance
    {
        get
        {
            if (Instance == null) return null;
            return Instance;
        }
    }

    [Header("체력 관련")]
    public float MaxHP;
    public float currentHP { get; private set; }
    public Image HealthBar;
    public TextMeshProUGUI HealthText;
    [Header("공격 관련")]
    public float Damage;
    public int Ammo;
    [Header("추가 능력치")]
    [Range(-1f, 1f)]
    public float Crit_Rate = .3f;
    public float Crit_Dmg = 1.5f;
    public float Speed;
    public float Atk;
    public float GoldBoost = 1;
    public float HealBoost = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        currentHP = MaxHP;
        ChangeHealth(0);
    }

    public void ChangeHealth(float amount)
    {
        currentHP += amount * amount > 0 ? HealBoost : 1;

        HealthBar.fillAmount = currentHP / MaxHP;
        HealthText.text = currentHP.ToString("00") + "/" + MaxHP.ToString("00");
    }
    public void UseAmmo(int amount)
    {
        Ammo -= amount;
    }
}
