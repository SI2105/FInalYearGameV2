using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

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

        // Ensure dialogue window is initially hidden
        if (dialogueWindow != null)
        {
            dialogueText = dialogueWindow.GetComponentInChildren<TMP_Text>();
            dialogueWindow.SetActive(false);
        }

        enterButton = dialogueWindow.transform.GetChild(1).gameObject;
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
}
