using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class StatisticData
{
    public Text UIText;
    public string StatisticName;
    public int StatisticValue;

    public void UpdateText(float value)
    {
        if (UIText != null)
        {
            int amount = Mathf.RoundToInt(Mathf.Lerp(0, StatisticValue, value));
            UIText.text = $"{StatisticName}: {amount}";
        }
    }
}

public class StatisticManager : MonoBehaviour
{
    public static StatisticManager Instance { get; private set; }

    private Coroutine statisticCoroutine;

    [Header("UI 속성들")]
    [field: SerializeField]
    public GameObject StatisticPanel { get; private set; }

    [field: SerializeField]
    public StatisticData[] StatisticDataArray { get; private set; }

    [field: SerializeField]
    public Image[] currentWeapon;

    [field: SerializeField]
    public Image[] currentAccessory;

    public void Kill() => StatisticDataArray[0].StatisticValue++;

    public void GetHealth(int amount) => StatisticDataArray[2].StatisticValue += amount;

    public void UseHealth(int amount) => StatisticDataArray[3].StatisticValue += amount;

    public void GetHurt(int amount) => StatisticDataArray[4].StatisticValue += amount;

    public void Damage(int amount) => StatisticDataArray[5].StatisticValue += amount;

    public void Heal(int amount) => StatisticDataArray[6].StatisticValue += amount;

    public void GetAcienetStone(int amount) => StatisticDataArray[7].StatisticValue += amount;

    public void GetGold(int amount) => StatisticDataArray[8].StatisticValue += amount;

    public void GetStone(int amount) => StatisticDataArray[9].StatisticValue += amount;

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
            return;
        }
    }

    public void OnStatistic()
    {
        StatisticPanel.SetActive(true);
        StatisticDataArray[1].StatisticValue = NextStageMovePortal.currentStage;
        statisticCoroutine = StartCoroutine(ShowStatistic());
    }

    IEnumerator ShowStatistic()
    {
        float t = 0;
        while (t >= 1)
        {
            t += Time.unscaledDeltaTime;
            for (int i = 0; i < StatisticDataArray.Length; i++)
            {
                StatisticDataArray[i].UpdateText(t);
            }
            yield return null;
        }
    }
}
