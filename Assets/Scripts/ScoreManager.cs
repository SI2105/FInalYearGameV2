using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }


    private Dictionary<string, QuizScore> quizScores = new Dictionary<string, QuizScore>();
    public int overallScore { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            overallScore = 0;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void addOverallScore(int points) {
        overallScore += points;
        GameManager.Instance.updatePointText();
        LeaderboardManager.Instance.UpdatePlayerScore(GameManager.Instance.PlayerName, overallScore);

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

    public void ResetScores()
    {
        quizScores.Clear();
        overallScore = 0;
    }

 
  
}
