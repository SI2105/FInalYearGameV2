using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

   
    

    [System.Serializable]
    public class Section
    {
        public string dialogueObjective;  
        public string quizObjective;
        public bool isFinalSection;
    }

    [Header("Sections (In Order)")]
    [SerializeField] private List<Section> sections = new List<Section>();

    public int currentSection { get; private set; } = 0;
    public bool dialogueCompleted { get; private set; }  = false;
    public bool quizCompleted { get; private set; }  = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
      
    }

    private void Start()
    {
        UpdateObjectiveText();
       
    }


    public void CompleteDialogueObjective()
    {
        ScoreManager.Instance.addOverallScore(5);
        dialogueCompleted = true;
        if (sections[currentSection].isFinalSection)
        {
          
            GameManager.Instance.GameCompleted();
        }

        else {
            UIManager.Instance.ShowAlert(sections[currentSection].dialogueObjective + " Objective Completed! Find and upload your knowledge to the terminal", 4f);
            UIManager.Instance.ObjectiveText.text = "Current Objectives:\n" +
                "Quiz: " + sections[currentSection].quizObjective;
        }
        
    }

    
    public void CompleteQuizObjective()
    {
        quizCompleted = true;


        ScoreManager.Instance.addOverallScore(10);
        currentSection++;
        dialogueCompleted = false;
        quizCompleted = false;
        UpdateObjectiveText();
        GameManager.Instance.updatePointText();
    }

    
    public void UpdateObjectiveText(string message = "")
    {
        print(UIManager.Instance);
        if (UIManager.Instance.ObjectiveText == null)
            return;

        if (!string.IsNullOrEmpty(message))
        {
            UIManager.Instance.ObjectiveText.text = message;
        }
        else if (currentSection < sections.Count)
        {

            UIManager.Instance.ObjectiveText.text = "Current Objectives:\n";
            if (!dialogueCompleted)
            {
                UIManager.Instance.ObjectiveText.text += "Talk to NPC: " + sections[currentSection].dialogueObjective + "\n";
            }

            if (!quizCompleted && !sections[currentSection].isFinalSection)
            {
                UIManager.Instance.ObjectiveText.text += "Quiz: " + sections[currentSection].quizObjective;
            }
            
        }
        else
        {
            UIManager.Instance.ObjectiveText.text = "All objectives complete!";
        }
    }
}
