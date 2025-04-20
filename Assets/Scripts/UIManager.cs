using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Dialogue")]
    [SerializeField] private GameObject dialogueWindow;
    public TMP_Text dialogueText;
    public TMP_Text indexText;
    private GameObject enterButton;

    private void Start()
    {
        if (muteButton != null) { 
            muteButton.onClick.AddListener(ToggleMute);

            muteIcon.SetActive(false);
            unmuteIcon.SetActive(true);
        }
    }
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
            indexText = dialogueWindow.transform.GetChild(2).GetComponent<TMP_Text>();
        }

        if (quizWindow != null) {
            quizQuestionText = quizWindow.transform.GetChild(0).GetComponent<TMP_Text>();
            quizWindow.SetActive(false);
            answerButtons = quizWindow.GetComponentsInChildren<Button>();

        }

        if (objectiveWindow != null)
        {
            ObjectiveText = objectiveWindow.GetComponentInChildren<TMP_Text>();
            objectiveWindow.SetActive(false);
        }

        if (EndGameWindow != null) {
            
            EndGameWindow.SetActive(false);
        }

        if (PointsWindow != null) {
            LevelText = PointsWindow.transform.GetChild(0).GetComponent<TMP_Text>();
            PointsText = PointsWindow.transform.GetChild(1).GetComponent<TMP_Text>();
            PointsWindow.SetActive(false);
        }

        if (alertPanel != null) {
            alertText = alertPanel.GetComponentInChildren<TMP_Text>();
            alertPanel.SetActive(false);
        }

        if (menuPanel != null) {
            menuPanel.SetActive(true);
            menuPanelImage = menuPanel.GetComponent<Image>();
            menuButtonImage = menuPanel.transform.GetChild(0).GetComponent<Image>();
        }

        if(leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(false);
            leaderboardNameText = leaderboardPanel.transform.GetChild(0).GetComponent<TMP_Text>();
            leaderboardScoreText = leaderboardPanel.transform.GetChild(1).GetComponent<TMP_Text>();
        }

        if (leaderboardButton != null) {
           
            leaderboardButton.gameObject.SetActive(false);
            leaderboardButton.onClick.AddListener(ToggleLeaderboard);
            
        }

        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }

        if (helpButton != null) { 
            helpButton.gameObject.SetActive(false); 
            helpButton.onClick.AddListener(toggleHelpPanel);
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
            alertPanel.SetActive(false);

        }
        
    }
    public void updateIndexText(int current, int index) {
        if (indexText != null)
        {
            indexText.text = current + "/" + index;
        }

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





    #region Quiz
    [Header("Quiz")]
    [SerializeField] private GameObject quizWindow;

    [Header("Quiz Elements")]
    private TMP_Text quizQuestionText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private TMP_Text quizTimerText;

    public void setQuizTimerText(string text) {
        if (quizTimerText != null)
        {
            quizTimerText.text = text;
        }   
    }
    
    public GameObject QuizWindow
    {
        get { return quizWindow; }
        set { quizWindow = value; }
    }
    [SerializeField] private GameObject correctFeedbackImage;
    [SerializeField] private GameObject incorrectFeedbackImage;

    public void ShowCorrectFeedback(float duration)
    {
        if (correctFeedbackImage != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.correct);
            correctFeedbackImage.SetActive(true);
            StartCoroutine(HideFeedbackAfterDelay(correctFeedbackImage, duration));
        }
    }

    public void ShowIncorrectFeedback(float duration)
    {
        if (incorrectFeedbackImage != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.wrong);
            incorrectFeedbackImage.SetActive(true);
            StartCoroutine(HideFeedbackAfterDelay(incorrectFeedbackImage, duration));
        }
    }

    private IEnumerator HideFeedbackAfterDelay(GameObject feedbackObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        feedbackObj.SetActive(false);
    }
    public void ShowQuizWindow()
    {
        if (quizWindow != null)
        {
            quizWindow.SetActive(true);
            objectiveWindow.SetActive(false);
            PointsWindow.SetActive(false);
            correctFeedbackImage.SetActive(false);
            incorrectFeedbackImage.SetActive(false);
            alertPanel.SetActive(false);
        }
    }

    public void HideQuizWindow()
    {
        if (quizWindow != null)
        {
            quizWindow.SetActive(false);
            objectiveWindow.SetActive(true);
            PointsWindow.SetActive(true);
            correctFeedbackImage.SetActive(true);
            incorrectFeedbackImage.SetActive(true);
            alertPanel.SetActive(true);
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


    #region Objectives
    [Header("Objectives")]
    [SerializeField] private GameObject objectiveWindow;
    public TMP_Text ObjectiveText;
    public void SetObjectiveText(string message)
    {
        if (ObjectiveText != null)
            ObjectiveText.text = message;
        else
            Debug.LogError("Objective text component is not assigned in UIManager!");
    }

    public string GetObjectiveText()
    {
        return ObjectiveText != null ? ObjectiveText.text : string.Empty;
    }

    public void ClearObjectiveText()
    {
        if (ObjectiveText != null)
            ObjectiveText.text = string.Empty;
    }

    public void ShowObjectiveWindow()
    {
        if (objectiveWindow != null)
            objectiveWindow.SetActive(true);
    }

    public void HideObjectiveWindow()
    {
        if (objectiveWindow != null)
            objectiveWindow.SetActive(false);
    }

    public bool IsObjectiveWindowVisible()
    {
        return objectiveWindow != null && objectiveWindow.activeSelf;
    }
    #endregion


    #region EndGamePanel
    [Header("EndGame")]
    [SerializeField] private GameObject EndGameWindow;
    
    

    public void ShowEndGameWindow()
    {
        if (EndGameWindow != null)
            EndGameWindow.SetActive(true);
            PointsWindow.SetActive(false);
            objectiveWindow.SetActive(false);
            leaderboardButton.gameObject.SetActive(false);
            helpButton.gameObject.SetActive(false);
    }

    public void HideEndGameWindow()
    {
        if (EndGameWindow != null)
            EndGameWindow.SetActive(false);
    }

    public bool IsEndGameWindowVisible()
    {
        return EndGameWindow != null && EndGameWindow.activeSelf;
    }
    #endregion


    #region PointsPanel
    [Header("Points")]
    [SerializeField] private GameObject PointsWindow;
    public TMPro.TMP_Text LevelText;
    public TMP_Text PointsText;



    public void ShowPointsWindow()
    {
        if (PointsWindow != null)
            PointsWindow.SetActive(true);
    }

    public void HidePointsWindow()
    {
        if (PointsWindow != null)
            PointsWindow.SetActive(false);
    }

    public bool IsPointsWindowVisible()
    {
        return PointsWindow != null && PointsWindow.activeSelf;
    }

    public void SetPointsText(string message)
    {
        if (PointsText != null) {
            PointsText.text = message;
        }
            
        
    }

    public void SetLevelText(string message) {
        if (LevelText != null)
        {
            LevelText.text = message;
        }

    }
    #endregion


    #region Alert
    [SerializeField] private GameObject alertPanel;
    public TMP_Text alertText;
    private Coroutine alertCoroutine;
    public void ShowAlert(string message, float duration)
    {
        if (alertPanel == null || alertText == null)
        {
            Debug.LogError("Alert panel or text is not assigned in UIManager!");
            return;
        }
        // Stop any previous alert routine.
        if (alertCoroutine != null)
            StopCoroutine(alertCoroutine);

        alertText.text = message;
        alertPanel.SetActive(true);
        alertCoroutine = StartCoroutine(HideAlertAfterDelay(duration));
    }

    private IEnumerator HideAlertAfterDelay(float delay)
    {
        float elapsed = 0f;
        while (elapsed < delay)
        {
            if (IsDialogueWindowVisible())
            {
                alertPanel.SetActive(false);
                alertCoroutine = null;
                yield break; // Exit the coroutine early
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        alertPanel.SetActive(false);
        alertCoroutine = null;
    }
    #endregion
    #region Mute
    [Header("Mute")]
    [SerializeField] private Button muteButton;
    [SerializeField] private GameObject muteIcon;
    [SerializeField] private GameObject unmuteIcon;

    private bool isMuted = false;

    public void ToggleMute()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.toggle);
        isMuted = !isMuted;
        if (AudioManager.instance != null)
        {
            if (isMuted)
            {
                AudioManager.instance.MuteMusic();
            }
            else
            {
                AudioManager.instance.UnmuteMusic();
            }
        }
       
        UpdateMuteButtonImage();
    }

    private void UpdateMuteButtonImage()
    {
        if (muteButton != null)
        {
            if (isMuted)
            {
                muteIcon.SetActive(true);
                unmuteIcon.SetActive(false);
            }

            else
            {
                muteIcon.SetActive(false);
                unmuteIcon.SetActive(true);
            }
        }
    }
    #endregion
    #region Menu 
    [Header("Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private float menuFadeDuration = 0.5f;
    [SerializeField] private string welcomeMessage;
    private Image menuPanelImage;
    private Image menuButtonImage; 
    public void OnStartButtonClicked()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.click);
        if (menuPanel != null && menuPanelImage != null && menuButtonImage != null)
        {
            StartCoroutine(FadeOutMenuPanel());
        }
        else
        {
            Debug.LogError("Menu panel or CanvasGroup is not assigned in UIManager!");
        }
    }

    private IEnumerator FadeOutMenuPanel()
    {
        float elapsed = 0f;
        Color panelColor = menuPanelImage.color;
        Color buttonColor = menuButtonImage.color;

        while (elapsed < menuFadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / menuFadeDuration);
            panelColor.a = alpha;
            buttonColor.a = alpha;
            menuPanelImage.color = panelColor;
            menuButtonImage.color = buttonColor;
            yield return null;
        }
        // Ensure alpha is set to zero.
        panelColor.a = 0f;
        buttonColor.a = 0f;
        menuPanelImage.color = panelColor;
        menuButtonImage.color = buttonColor;
        // Hide the menu panel.
        menuPanel.SetActive(false);

        GameManager.Instance.player.enableInput();
        PointsWindow.SetActive(true);
        objectiveWindow.SetActive(true);
        leaderboardButton.gameObject.SetActive(true);
        helpButton.gameObject.SetActive(true);
        ShowAlert(welcomeMessage,6f);
        
    }
    #endregion
    #region Help Button
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private Button helpButton;

    
    public void toggleHelpPanel()
    {
        if (helpPanel != null)
        {
            helpPanel.SetActive(!helpPanel.activeSelf);
            AudioManager.instance.PlaySFX(AudioManager.instance.toggle);
        }
    }


    #endregion

    #region Leaderboard
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private TMP_Text leaderboardNameText;
    [SerializeField] private TMP_Text leaderboardScoreText;
    [SerializeField] private Button leaderboardButton;

    // Show the leaderboard panel.
    public void ShowLeaderboard()
    {
        if (leaderboardPanel != null)
        {
            leaderboardPanel.SetActive(true);
        }
    }

    // Hide the leaderboard panel.
    public void HideLeaderboard()
    {
        if (leaderboardPanel != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.click);
            leaderboardPanel.SetActive(false);
        }
    }

    // Set the leaderboard score text.
    public void SetLeaderboardScoreText(string text)
    {
        if (leaderboardScoreText != null)
        {
            leaderboardScoreText.text = text;
        }
    }
    public void SetLeaderboardNameText(string text)
    {
        if (leaderboardNameText != null)
        {
            leaderboardNameText.text = text;
        }
    }

    // Get the leaderboard score text.
    public string GetLeaderboardScoreText()
    {
        return leaderboardScoreText != null ? leaderboardScoreText.text : string.Empty;
    }

    public bool IsLeaderboardVisible()
    {
        return leaderboardPanel != null && leaderboardPanel.activeSelf;
    }

    public void ToggleLeaderboard()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.toggle);
        if (IsLeaderboardVisible())
        {
            HideLeaderboard();
        }
        else
        {
            LeaderboardManager.Instance.UpdateLeaderboardDisplay();
            ShowLeaderboard();
        }
    }
    #endregion
}



