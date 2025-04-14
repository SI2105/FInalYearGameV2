using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quiz : MonoBehaviour
{
    
    public GameObject indicator;

    // List of questions for this quiz
    public List<Question> questions;

    private int currentQuestionIndex;
    private bool started;

    private int correctCount = 0;
    private int incorrectCount = 0;

    private int failCount = 0;
    //Locked state for if quiz has been failed multiple times 
    private bool locked = false;
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
            UIManager.Instance.ShowCorrectFeedback(0.5f);
        }
        else
        {
            incorrectCount++;
            UIManager.Instance.ShowIncorrectFeedback(0.5f);
        }
        StartCoroutine(ProceedAfterFeedback(0.5f));
        
    }

    private IEnumerator ProceedAfterFeedback(float delay)
    {
        yield return new WaitForSeconds(delay);
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
        
        if (correctCount == questions.Count)
        {
            // Mark the quiz objective as complete.
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.CompleteQuizObjective();
                UIManager.Instance.ShowAlert("Quiz Passed Sucessfully! \n Please proceed to next objective", 4f);
            }
            // Disable access to this terminal.
            gameObject.SetActive(false);
        }
        else
        {
            failCount++;
            if (ObjectiveManager.Instance != null)
            {
               UIManager.Instance.ShowAlert("Quiz not passed. Please try again.\n Your Results: \n Correct: " + correctCount + "\n Incorrect: " + incorrectCount , 4f);
            }
        }

        correctCount = 0;
        incorrectCount = 0;
    }
}
