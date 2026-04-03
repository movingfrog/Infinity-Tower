using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Idle,
    Interacting,
    InvenOpen,
    ShopOpen,
    Pause,
}

public class PlayerStatManager : MonoBehaviour
{
    private static PlayerStatManager Instance;
    public static PlayerStatManager instance
    {
        get
        {
            if (Instance == null)
                return null;
            return Instance;
        }
    }

    public PlayerState currentState { get; private set; }

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
    public float Speed = 1;
    public float Atk = 1;
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

    public void resetStat()
    {
        Crit_Rate = .3f;
        Crit_Dmg = 1.5f;
        Speed = 1;
        Atk = 0;
        GoldBoost = 1;
        HealBoost = 1;
    }

    public void statUp(StatType stat, float Increase)
    {
        switch (stat)
        {
            case StatType.ATK:
                Atk += Increase;
                break;
            case StatType.CRIT_RATE:
                Crit_Rate += Increase / 100;
                break;
            case StatType.CRIT_DMG:
                Crit_Dmg += Increase / 100;
                break;
            case StatType.SPEED:
                Speed += Increase / 100;
                break;
            case StatType.GOLDBOOST:
                GoldBoost += Increase / 100;
                break;
            case StatType.HEALBOOST:
                HealBoost += Increase / 100;
                break;
        }
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

    public void ChangeState(PlayerState State) => currentState = State;

    public bool getState(PlayerState compareState) => currentState == compareState;

    public void resetState() => currentState = PlayerState.Idle;
}
