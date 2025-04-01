using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerController player;
    private ObjectiveManager objectiveManager;
    private ScoreManager scoreManager;

    //[Header("Leaderboard Panel")]
    //[SerializeField] private GameObject leaderboardPanel;
    //[SerializeField] private TMP_Text leaderboardScoreText;

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

        player = FindObjectOfType<PlayerController>();
        scoreManager = ScoreManager.Instance;
        objectiveManager = ObjectiveManager.Instance;
        updatePointText();
    }

    public void updatePointText()
    {
        string lvl = "Lvl " + (objectiveManager.currentSection+1);
        string points =   "Points: " + scoreManager.overallScore;
        UIManager.Instance.SetLevelText(lvl);   
        UIManager.Instance.SetPointsText(points);
       

    }

        // Call this method when the final dialogue is completed.
        public void GameCompleted()
    {
        UIManager.Instance.ShowEndGameWindow();
        player.disableInput();


    }
}