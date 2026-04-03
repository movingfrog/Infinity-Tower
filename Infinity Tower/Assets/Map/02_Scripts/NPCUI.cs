using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPCUI : MonoBehaviour
{
    public static NPCUI instance;

    [SerializeField]
    private Button button;
    private int selectNum;

    [Header("UI속성")]
    public TextMeshProUGUI NPCName;
    public TextMeshProUGUI NPCDialogue;
    public TextMeshProUGUI selectAText;
    public TextMeshProUGUI selectBText;

    [Header("UI패널")]
    public GameObject TextBoxUI;
    public GameObject selectObject;

    [Header("선택 UI위치")]
    public Image selectArrow;
    public RectTransform[] selectPos;

    [Header("대사 세팅")]
    public float typingSpeed = .05f;

    private Tween typingTween;
    private bool isTyping;
    private string[] currentLine;
    private int lineIndex;

    private Action onSelectA;
    private Action onSelectB;

    private void Awake()
    {
        instance = this;
        button.onClick.AddListener(OnInteract);
        selectObject.SetActive(false);
    }

    private void OnEnable()
    {
        InputManager.Instance.inputActions.Player.Interact.started += inter;
    }

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.Interact.started -= inter;
    }

    private void OnDestroy()
    {
        if (instance != null)
            instance = null;
    }

    public void SettingUI(
        string npcName,
        string[] line,
        string selectA,
        string selectB,
        Action actionA,
        Action actionB
    )
    {
        TextBoxUI.SetActive(true);
        selectObject.SetActive(false);

        NPCName.text = npcName;
        currentLine = line;
        lineIndex = 0;

        selectAText.text = selectA;
        selectBText.text = selectB;
        onSelectA = actionA;
        onSelectB = actionB;

        ShowNextLine();
    }

    private void ShowNextLine()
    {
        if (lineIndex < currentLine.Length)
        {
            string line = currentLine[lineIndex].Replace("\\n", "\n");

            NPCDialogue.text = line;
            NPCDialogue.maxVisibleCharacters = 0;
            isTyping = true;

            typingTween?.Kill();
            typingTween = DOTween
                .To(
                    () => NPCDialogue.maxVisibleCharacters,
                    x => NPCDialogue.maxVisibleCharacters = x,
                    line.Length,
                    line.Length * typingSpeed
                )
                .SetEase(Ease.Linear)
                .OnComplete(() => isTyping = false);

            lineIndex++;
        }
        else
        {
            ShowSelect();
        }
    }

    private void ShowSelect()
    {
        isTyping = false;
        if (onSelectA != null && onSelectB != null)
            selectObject.SetActive(true);
        else
            CloseUI();
    }

    private void ExecuteSelect()
    {
        CloseUI();
        if (selectNum == 0)
            onSelectA.Invoke();
        else
            onSelectB.Invoke();
    }

    private void CloseUI()
    {
        typingTween.Kill();
        selectObject.SetActive(false);
        TextBoxUI.SetActive(false);
        PlayerStatManager.instance.resetState();
    }

    private void UpdateSelectUI()
    {
        Vector3 movePos = selectArrow.rectTransform.anchoredPosition;
        movePos.y = selectPos[selectNum].anchoredPosition.y;
        selectArrow.rectTransform.anchoredPosition = movePos;
    }

    public void OnSelect(InputValue value)
    {
        if (!selectObject.activeSelf)
            return;
        Vector2 input = value.Get<Vector2>();
        if (input.y != 0)
        {
            selectNum = (selectNum + ((int)input.y) + 2) % 2;
            UpdateSelectUI();
        }
    }

    void inter(InputAction.CallbackContext callback) => OnInteract();

    public void OnInteract()
    {
        if (!PlayerStatManager.instance.getState(PlayerState.Interacting))
            return;
        if (isTyping)
        {
            typingTween.Complete();
        }
        else
        {
            if (!selectObject.activeSelf)
            {
                ShowNextLine();
            }
            else
            {
                ExecuteSelect();
            }
        }
    }
}
