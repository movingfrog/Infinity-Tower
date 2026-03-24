using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCUI : MonoBehaviour
{
    public static NPCUI instance;

    private Button button;

    [Header("UI加己")]
    public TextMeshProUGUI NPCName;
    public TextMeshProUGUI NPCDialogue;
    public TextMeshProUGUI selectAText;
    public TextMeshProUGUI selectBText;

    [Header("UI菩澄")]
    public GameObject TextBoxUI;
    public GameObject selectObject;

    [Header("技泼")]
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
        button = GetComponent<Button>();
        button.onClick.AddListener(OnInteract);
        selectObject.SetActive(false);
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
        selectObject.SetActive(true);
    }

    private void CloseUI()
    {
        typingTween.Kill();
        selectObject.SetActive(false);
        TextBoxUI.SetActive(false);
        PlayerStatManager.instance.resetState();
    }

    public void OnInteract()
    {
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
                Debug.Log("急琶等 可记阑 角青");
            }
        }
    }
}
