using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopRestock : MonoBehaviour
{
    [Header("플레이어 판정 관련")]
    [SerializeField]
    private LayerMask Player;

    [SerializeField]
    private float interactSize;

    [Header("UI오브젝트")]
    private GameObject ItemInfoObject;
    private TextMeshProUGUI ItemInfoText;

    [Header("재입고 기능 관련")]
    [SerializeField]
    private ShopBox[] allShopBox;

    [SerializeField]
    private int restockCount;

    [SerializeField, Tooltip("재입고 이미지")]
    private Transform RestockImage;

    [SerializeField]
    private float spinSpeedAmount;

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Interact.started += Restock;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Interact.started -= Restock;
    }

    private void Start()
    {
        ItemInfoObject = SpaceUIManager.Instance.CreateItemUI(gameObject);
        ItemInfoText = ItemInfoObject.GetComponentInChildren<TextMeshProUGUI>();
        RefreshUI();
        ItemInfoObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Collider2D outPlayer = Physics2D.OverlapCircle(
            transform.position,
            interactSize * .8f,
            Player
        );
        bool playerState = outPlayer != null;

        ItemInfoObject.SetActive(playerState);
    }

    private void Restock(InputAction.CallbackContext context)
    {
        if (restockCount > 0 && ItemInfoObject != null && ItemInfoObject.activeSelf)
        {
            foreach (var shop in allShopBox)
            {
                shop.GetNewItem();
            }
            StartCoroutine(SpinRestockImage(spinSpeedAmount));
            restockCount--;
            RefreshUI();
        }
    }

    IEnumerator SpinRestockImage(float amount)
    {
        for (float i = 0; i < 1; i += Time.deltaTime * spinSpeedAmount)
        {
            RestockImage.rotation = Quaternion.Lerp(
                Quaternion.identity,
                Quaternion.Euler(new Vector3(0, 0, -360)),
                i
            );
            yield return null;
        }
    }

    private void RefreshUI()
    {
        ItemInfoText.text = restockCount.ToString("0");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.lightCyan * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireSphere(transform.position, interactSize);
    }
}
