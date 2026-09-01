using UnityEngine;
using UnityEngine.InputSystem;

public abstract class DroppedEnchant : MonoBehaviour
{
    /// <summary>
    ///  드롭된 각인 오브젝트의 각인 정보
    /// </summary>
    public WeaponEnchant enchant;

    [Header("플레이어 접근 시 등장하는 오브젝트들")]
    [SerializeField]
    protected EnchantInfo InfoObject; //GameObject를 InfoObject에 할당 된 스크립트로 변환 필요

    [SerializeField]
    protected SpriteRenderer BackImage;

    [Header("플레이어 상호작용 값")]
    [SerializeField]
    protected float InteractDistance = 1f;

    [SerializeField]
    protected Transform EnchantPosition;

    [SerializeField]
    protected LayerMask Player;

    protected void Start()
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
            InputManager.Instance.inputActions.Player.Interact.started += Interact;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.inputActions.Player.Interact.started -= Interact;
        }
    }

    private void Interact(InputAction.CallbackContext callback)
    {
        Collider2D PlayerColl = Physics2D.OverlapCircle(
            EnchantPosition.position,
            InteractDistance,
            Player
        );
        if (PlayerColl != null)
        {
            OnInteract();
        }
    }

    protected abstract void OnInteract();

    protected void FixedUpdate()
    {
        Collider2D PlayerColl = Physics2D.OverlapCircle(
            EnchantPosition.position,
            InteractDistance,
            Player
        );
        InfoObject.gameObject.SetActive(PlayerColl != null);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireSphere(EnchantPosition.position, InteractDistance);
    }
}
