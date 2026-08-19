using UnityEngine;
using UnityEngine.InputSystem;

public class DroppedEnchant : MonoBehaviour
{
    /// <summary>
    /// 드롭된 각인 오브젝트의 각인 정보
    /// </summary>
    public WeaponEnchant enchant { get; set; }

    [Header("플레이어 접근 시 등장하는 오브젝트들")]
    [SerializeField]
    private EnchantInfo InfoObject; //GameObject를 InfoObject에 할당 된 스크립트로 변환 필요

    [SerializeField]
    private SpriteRenderer BackImage;

    [Header("플레이어 상호작용 값")]
    [SerializeField]
    private float InteractDistance = 1.5f;

    [SerializeField]
    private GameObject EnchantCanvas;

    [SerializeField]
    private LayerMask Player;

    private void Start()
    {
        if (enchant != null && BackImage != null)
        {
            BackImage.sprite = enchant.EnchantImage;
        }
        InfoObject.gameObject.SetActive(false);
        InfoObject.SetInfo(enchant);
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.inputActions.Player.Interact.started += OnInteract;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.inputActions.Player.Interact.started -= OnInteract;
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (InfoObject.gameObject.activeSelf)
        {
            EnchantCanvas.SetActive(true);
            Time.timeScale = 0f; // 게임 일시정지
        }
    }

    private void FixedUpdate()
    {
        Collider2D PlayerColl = Physics2D.OverlapCircle(
            transform.position,
            InteractDistance,
            Player
        );
        InfoObject.gameObject.SetActive(PlayerColl != null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireSphere(transform.position, InteractDistance);
    }
}
