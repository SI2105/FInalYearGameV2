using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quiz : MonoBehaviour
{
    // Interactable indicator for the quiz (e.g., an icon)
    public GameObject indicator;

    // List of questions for this quiz
    public List<Question> questions;

    private int currentQuestionIndex;
    private bool started;

    private int correctCount = 0;
    private int incorrectCount = 0;
    private void Awake()
    {
        ToggleIndicator(false);
    }

    public void ToggleIndicator(bool show)
    {
        if (indicator != null)
            indicator.SetActive(show);
    }

    public void StartQuiz()
    {
        if (started)
            return;

        started = true;

        // Show the quiz UI
        UIManager.Instance.ShowQuizWindow();

        // Hide the indicator when the quiz starts
        ToggleIndicator(false);

        currentQuestionIndex = 0;
        DisplayQuestion();
    }

    private void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            Question currentQuestion = questions[currentQuestionIndex];
            // Update the UI with the current question text
            UIManager.Instance.SetQuizQuestionText(currentQuestion.questionText);

            // Clear previous button listeners
            UIManager.Instance.ClearAnswerButtonListeners();

            // Setup each answer button
            for (int i = 0; i < currentQuestion.answers.Count; i++)
            {
                int answerIndex = i; // local copy for closure
                UIManager.Instance.SetAnswerButton(i, currentQuestion.answers[i], () => OnAnswerSelected(answerIndex));
            }
        }
        else
        {
            EndQuiz();
        }
    }

    private void OnAnswerSelected(int answerIndex)
    {
        Question currentQuestion = questions[currentQuestionIndex];

        if (answerIndex == currentQuestion.correctAnswerIndex)
        {
            correctCount++;
        }
        else
        {
            incorrectCount++;
        }

        currentQuestionIndex++;
        DisplayQuestion();
    }

    public void EndQuiz()
    {
        Debug.Log("Quiz completed!");
        started = false;
        UIManager.Instance.HideQuizWindow();
        ScoreManager.Instance.StoreScore(gameObject.name, correctCount, incorrectCount);
        ScoreManager.Instance.PrintScores();
    }
}
