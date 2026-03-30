using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour
{
    [Header("NPC 속성")]
    public string NPCName;
    public string[] dialogueLines;
    public string[] addDialogueLine;
    public string selectAText;
    public string selectBText;

    [Header("플레이어 탐지 관련 변수")]
    public GameObject interactionIcon;
    public LayerMask PlayerLayer;
    public float radius;
    public bool isIn;

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Interact.started += OnInteract;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Interact.started -= OnInteract;
    }

    protected void FixedUpdate()
    {
        Collider2D Player = Physics2D.OverlapCircle(transform.position, radius, PlayerLayer);

        interactionIcon.SetActive(Player != null);
        isIn = Player != null;
        if (Player != null)
        {
            interactionIcon.transform.position = Player.transform.position;
        }
    }

    public void OnInteract(InputAction.CallbackContext callback)
    {
        Debug.Log("dslkfj");
        if (isIn && PlayerStatManager.instance.getState(PlayerState.Idle))
        {
            PlayerStatManager.instance.ChangeState(PlayerState.Interacting);
            NPCUI.instance.SettingUI(
                NPCName,
                dialogueLines,
                selectAText,
                selectBText,
                OnConfirmAction,
                OnCancelAction
            );
        }
    }

    public abstract void OnConfirmAction();

    public virtual void OnCancelAction()
    {
        NPCUI.instance.SettingUI(NPCName, addDialogueLine, "noting", "noting", null, null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue * new Color(1, 1, 1, .3f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
