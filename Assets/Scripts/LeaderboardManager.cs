using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int highScore;
}

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    [Header("Leaderboard Entries (Populate via Inspector)")]
    [SerializeField] private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

    private string currentPlayerName;

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


    private void Start()
    {
        currentPlayerName = GameManager.Instance.PlayerName;
    }
    public void UpdatePlayerScore(string playerName, int score)
    {
        bool found = false;
        //looks for player
        foreach (var entry in leaderboardEntries)
        {
            if (entry.playerName == playerName)
            {
                entry.highScore = score;
                found = true;
                break;
            }
        }
        if (!found)
        {
            
            LeaderboardEntry newEntry = new LeaderboardEntry { playerName = playerName, highScore = score };
            leaderboardEntries.Add(newEntry);
        }

        // Update the current player's name.
        currentPlayerName = playerName;

        SortLeaderboard();
        UpdateLeaderboardDisplay();
    }


    private void SortLeaderboard()
    {
        leaderboardEntries = leaderboardEntries.OrderByDescending(e => e.highScore).ToList();
    }


    public void UpdateLeaderboardDisplay()
    {
        string namesDisplay = "Name\n";
        string scoresDisplay = "Score\n";

        foreach (var entry in leaderboardEntries)
        {
            string nameLine;
            string scoreLine;
            if (entry.playerName == currentPlayerName)
            {
                
                nameLine = "<color=green><b>" + entry.playerName + "</b></color>";
                scoreLine = "<color=green><b>" + entry.highScore + "</b></color>";
            }
            else
            {
                nameLine = entry.playerName;
                scoreLine = entry.highScore.ToString();
            }
            namesDisplay += nameLine + "\n";
            scoresDisplay += scoreLine + "\n";
        }

        // Update the UI using the UIManager instance.
        UIManager.Instance.SetLeaderboardNameText(namesDisplay);
        UIManager.Instance.SetLeaderboardScoreText(scoresDisplay);
    }

}
