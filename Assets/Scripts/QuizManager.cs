using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    // Reference to UI elements.
    public Text questionText;
    public Button[] answerButtons;

    private Quiz currentQuiz;
    private int questionIndex;

    // Call this method to start a quiz with specific quiz data.
    public void StartQuiz(Quiz quiz)
    {
        currentQuiz = quiz;
        questionIndex = 0;
        gameObject.SetActive(true); // Enable the quiz UI
        DisplayQuestion();
    }

    // Update UI with the current question and answer choices.
    void DisplayQuestion()
    {
        if (currentQuiz != null && questionIndex < currentQuiz.questions.Count)
        {
            Question currentQuestion = currentQuiz.questions[questionIndex];
            questionText.text = currentQuestion.questionText;

            // Loop through the answer buttons.
            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (i < currentQuestion.answers.Count)
                {
                    answerButtons[i].gameObject.SetActive(true);
                    answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];

                    // Remove any previous listeners to avoid duplicate subscriptions.
                    answerButtons[i].onClick.RemoveAllListeners();
                    int closureIndex = i; // Prevent closure issues.
                    answerButtons[i].onClick.AddListener(() => OnAnswerSelected(closureIndex));
                }
                else
                {
                    answerButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            EndQuiz();
        }
    }

    // Called when the player selects an answer.
    void OnAnswerSelected(int answerIndex)
    {
        Question currentQuestion = currentQuiz.questions[questionIndex];

        if (answerIndex == currentQuestion.correctAnswerIndex)
        {
            Debug.Log("Correct answer!");
            // You can add scoring or visual feedback here.
        }
        else
        {
            Debug.Log("Incorrect answer!");
            // Handle wrong answer feedback.
        }

        questionIndex++;
        DisplayQuestion();
    }

    // Cleanup and close the quiz UI once the quiz is completed.
    void EndQuiz()
    {
        Debug.Log("Quiz completed!");
        // Optionally hide the quiz UI or trigger further game logic.
        gameObject.SetActive(false);
    }
}
