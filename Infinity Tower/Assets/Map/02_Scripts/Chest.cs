using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    private Animator ani;
    private bool isRegistered;

    [SerializeField]
    private RewardTable Reward;

    [Header("플레이어 탐지 관련")]
    [SerializeField]
    private GameObject InteractionObject;

    [SerializeField]
    private LayerMask PlayerLayer;

    [SerializeField]
    private float radius;
    private bool isIn;

    private void Awake()
    {
        TryGetComponent<Animator>(out ani);
    }

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.inputActions.Player.Interact.started += Open;
            isRegistered = true;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null && isRegistered)
        {
            InputManager.Instance.inputActions.Player.Interact.started -= Open;
            isRegistered = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isRegistered)
            return;
        Collider2D PlayerColl = Physics2D.OverlapCircle(transform.position, radius, PlayerLayer);
        InteractionObject.SetActive(PlayerColl != null);
        isIn = PlayerColl != null;
        if (isIn)
        {
            InteractionObject.transform.position = PlayerColl.transform.position;
        }
    }

    private void Open(InputAction.CallbackContext callback)
    {
        if (!isIn)
            return;
        ani.SetBool("IsOpened", true);
        Reward.Reward(transform);
        InputManager.Instance.inputActions.Player.Interact.started -= Open;
        isRegistered = false;
        InteractionObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.lightSeaGreen * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
