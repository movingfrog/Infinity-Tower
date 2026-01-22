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
        currentHP += amount;

        HealthBar.fillAmount = currentHP / MaxHP;
        HealthText.text = currentHP.ToString("00") + "/" + MaxHP.ToString("00");
    }
    public void UseAmmo(int amount)
    {
        Ammo -= amount;
    }
}
