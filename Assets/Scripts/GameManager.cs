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
    [SerializeField] public string PlayerName;
    public bool gameCompleted = false;



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
        LeaderboardManager.Instance.UpdatePlayerScore(PlayerName, 0);
    }

    public void updatePointText()
    {
        string lvl = "Lvl " + (objectiveManager.currentSection+1);
        string points =   "Points: " + scoreManager.overallScore;
        UIManager.Instance.SetLevelText(lvl);   
        UIManager.Instance.SetPointsText(points);
       

    }

       
     public void GameCompleted()
    {
        gameCompleted = true;
        UIManager.Instance.ShowEndGameWindow();
        player.disableInput();


    }
}