using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

   
    private Dictionary<string, QuizScore> quizScores = new Dictionary<string, QuizScore>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Store or update a quiz score
    public void StoreScore(string quizId, int correct, int incorrect)
    {
        if (quizScores.ContainsKey(quizId))
        {
            quizScores[quizId].correctCount = correct;
            quizScores[quizId].incorrectCount = incorrect;
        }
        else
        {
            quizScores.Add(quizId, new QuizScore(correct, incorrect));
        }
    }

    public QuizScore GetScore(string quizId)
    {
        if (quizScores.ContainsKey(quizId))
        {
            return quizScores[quizId];
        }
        return null;
    }
  //Dev
    public void PrintScores()
    {
        foreach (var pair in quizScores)
        {
            Debug.Log($"Quiz: {pair.Key} | Correct: {pair.Value.correctCount} | Incorrect: {pair.Value.incorrectCount}");
        }
    }
}
