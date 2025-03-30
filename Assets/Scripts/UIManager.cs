using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Dialogue")]
    [SerializeField] private GameObject dialogueWindow;
    public TMP_Text dialogueText;
    private GameObject enterButton;


    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Ensure quiz window is initially hidden
        if (dialogueWindow != null)
        {
            dialogueText = dialogueWindow.GetComponentInChildren<TMP_Text>();
            dialogueWindow.SetActive(false);
            enterButton = dialogueWindow.transform.GetChild(1).gameObject;
        }

        if (quizWindow != null) {
            quizQuestionText = quizWindow.transform.GetChild(0).GetComponent<TMP_Text>();
            quizWindow.SetActive(false);
            answerButtons = quizWindow.GetComponentsInChildren<Button>();

        }

        
    }

    public void SetDialogueText(string message)
    {
        if (dialogueText == null)
        {
            Debug.LogError("Dialogue text component is not assigned in UIManager!");
            return;
        }

        dialogueText.text = message;
    }

    public string GetDialogueText()
    {
        if (dialogueText != null)
        {
            return dialogueText.text;
        }
        return string.Empty;
    }


    public void ClearDialogueText()
    {
        if (dialogueText != null)
        {
            dialogueText.text = string.Empty;
        }
    }

    
    public void ShowDialogueWindow()
    {
        if (dialogueWindow != null)
        {
            dialogueWindow.SetActive(true);
        }
        dialogueText.text = "OPEN";
    }

    
    public void HideDialogueWindow()
    {
        if (dialogueWindow != null)
        {
            dialogueWindow.SetActive(false);
        }
    }

    public void ShowEnter() {
        enterButton.SetActive(true);
    }

    public void HideEnter() {
        enterButton.SetActive(false);
    }


    public bool IsDialogueWindowVisible()
    {
        return dialogueWindow != null && dialogueWindow.activeSelf;
    }


    [Header("Quiz")]
    [SerializeField] private GameObject quizWindow;

    [Header("Quiz Elements")]
    private TMP_Text quizQuestionText;
    [SerializeField] private Button[] answerButtons;
    public GameObject QuizWindow
    {
        get { return quizWindow; }
        set { quizWindow = value; }
    }

    
    #region Quiz Methods
    public void ShowQuizWindow()
    {
        if (quizWindow != null)
        {
            quizWindow.SetActive(true);
        }
    }

    public void HideQuizWindow()
    {
        if (quizWindow != null)
        {
            quizWindow.SetActive(false);
        }
    }

    public bool IsQuizWindowVisible()
    {
        return quizWindow != null && quizWindow.activeSelf;
    }

    // Set the quiz question text.
    public void SetQuizQuestionText(string text)
    {
        if (quizQuestionText != null)
        {
            quizQuestionText.text = text;
        }
    }

    // Set a specific answer button's text and listener.
    public void SetAnswerButton(int index, string text, UnityAction action)
    {
        if (answerButtons != null && index < answerButtons.Length)
        {
            answerButtons[index].gameObject.SetActive(true);
            TMP_Text btnText = answerButtons[index].GetComponentInChildren<TMP_Text>();
            if (btnText != null)
            {
                btnText.text = text;
            }
            answerButtons[index].onClick.RemoveAllListeners();
            answerButtons[index].onClick.AddListener(action);
        }
    }

    // Clear listeners from all answer buttons.
    public void ClearAnswerButtonListeners()
    {
        if (answerButtons != null)
        {
            foreach (var button in answerButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }
    #endregion

}
