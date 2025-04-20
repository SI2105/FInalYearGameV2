using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quiz : MonoBehaviour
{
    public GameObject indicator;
    public GameObject lockIndicator;
    public GameObject completeIndicator;
    public List<Question> questions;

    [Header("Timer")]
    public float timePerQuestion = 10f;
    public int pointsPerSavedSecond = 1;

    int currentQuestionIndex;
    bool started;
    bool waiting;

    int correctCount;
    int incorrectCount;

    float questionTimeLeft;
    float totalTimeUsed;

    private float normalMusicSpeed = 1f;
    private float quizMusicSpeed = 1.25f;

    public bool Iscomplete;

    private void Awake()
    {
        toggleIndicator(false);
        toggleLockIndicator(false);
        toggleCompleteIndicator(false);

    }

    private void Start()
    {
        Iscomplete = false;
    }

    public void toggleIndicator(bool show)
    {
        if (indicator != null) indicator.SetActive(show);
    }

    public void toggleLockIndicator(bool show)
    {
        if (lockIndicator != null) lockIndicator.SetActive(show);
    }

    public void toggleCompleteIndicator(bool show) {
        if (completeIndicator != null) completeIndicator.SetActive(show);
    }
    public void StartQuiz()
    {
        if (started) return;

        GameManager.Instance.player.disableInput(); 
        started = true;
        correctCount = incorrectCount = 0;
        totalTimeUsed = 0f;
        currentQuestionIndex = 0;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.GraduallySpeedUpMusic(quizMusicSpeed, 2f); // Speed up over 2 seconds
        }
        UIManager.Instance.ShowQuizWindow();
        toggleIndicator(false);
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex >= questions.Count) { EndQuiz(); return; }

        Question q = questions[currentQuestionIndex];
        UIManager.Instance.SetQuizQuestionText(q.questionText);
        UIManager.Instance.ClearAnswerButtonListeners();

        for (int i = 0; i < q.answers.Count; i++)
        {
            int idx = i;
            UIManager.Instance.SetAnswerButton(idx, q.answers[i], () => OnAnswer(idx));
        }

        questionTimeLeft = timePerQuestion;
        waiting = true;
    }

    void OnAnswer(int idx)
    {
        if (!waiting) return;
        waiting = false;

        bool correct = idx == questions[currentQuestionIndex].correctAnswerIndex;
        if (correct)
        {
            correctCount++;
            UIManager.Instance.ShowCorrectFeedback(1f);
        }
        else
        {
            incorrectCount++;
            UIManager.Instance.ShowIncorrectFeedback(1f);
        }

        totalTimeUsed += timePerQuestion - questionTimeLeft;
        StartCoroutine(Next(0.35f));
    }

    IEnumerator Next(float n)
    {
        yield return new WaitForSeconds(n);
        currentQuestionIndex++;
        DisplayQuestion();
    }

    void Update()
    {
        if (!started || !waiting) {
            return;
        }
           

        // gets the time remaining in seconds
        int remainingSeconds = Mathf.FloorToInt(questionTimeLeft);

        //handles the time between updates
        questionTimeLeft -= Time.deltaTime;
        UIManager.Instance.setQuizTimerText(remainingSeconds.ToString());

    

    if (questionTimeLeft <= 0f)
    {
        waiting = false;
        incorrectCount++;
        UIManager.Instance.ShowIncorrectFeedback(1f);
        totalTimeUsed += timePerQuestion;
        StartCoroutine(Next(0.35f));
    }
    }

    void EndQuiz()
    {
        started = false;
        UIManager.Instance.HideQuizWindow();
        GameManager.Instance.player.enableInput();

        ScoreManager.Instance.StoreScore(gameObject.name, correctCount, incorrectCount);
        if (AudioManager.instance != null)
        {
            AudioManager.instance.GraduallySlowDownMusic(normalMusicSpeed, 2f); // Slow down over 2 seconds
        }

        if (correctCount == questions.Count)
        {
            float saved = Mathf.Max(0f, questions.Count * timePerQuestion - totalTimeUsed);
            int bonus = Mathf.RoundToInt(saved) * pointsPerSavedSecond;
            ScoreManager.Instance.addOverallScore(bonus);
            ObjectiveManager.Instance?.CompleteQuizObjective();
            UIManager.Instance.ShowAlert($"Quiz passed!\nBonus +{bonus} pts", 4f);
            Iscomplete = true;
            UIManager.Instance.HideQuizWindow();
        }
        else
        {
            UIManager.Instance.ShowAlert($"Quiz failed.\n<color=green>Correct: {correctCount}</color>\n<color=red>Incorrect: {incorrectCount}</color>", 4f);
        }

        correctCount = incorrectCount = 0;
    }
}
