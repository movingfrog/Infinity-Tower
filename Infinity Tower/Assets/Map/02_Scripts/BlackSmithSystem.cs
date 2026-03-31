using TMPro;
using UnityEngine;

public class BlackSmithSystem : MonoBehaviour
{
    public static BlackSmithSystem Instance;

    [Header("UI ¼Ó¼º")]
    public TextMeshProUGUI[] gettingGoods;
    public TextMeshProUGUI[] usingGoods;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
