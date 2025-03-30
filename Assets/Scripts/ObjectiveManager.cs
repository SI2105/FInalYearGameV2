using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [Header("Objective UI")]
    [SerializeField] private TMP_Text objectiveText;

    [System.Serializable]
    public class Section
    {
        public string dialogueObjective;  
        public string quizObjective;        
    }

    [Header("Sections (In Order)")]
    [SerializeField] private List<Section> sections = new List<Section>();

    private int currentSection = 0;
    private bool dialogueCompleted = false;
    private bool quizCompleted = false;

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
        UpdateObjectiveText();
    }

   
    public void CompleteDialogueObjective()
    {
        dialogueCompleted = true;
        UpdateObjectiveText("Dialogue complete! Now complete the quiz for this section.");
    }

    
    public void CompleteQuizObjective()
    {
        quizCompleted = true;
        UpdateObjectiveText("Quiz complete! Proceed to the next NPC.");

   
        currentSection++;
        dialogueCompleted = false;
        quizCompleted = false;
    }

    
    public void UpdateObjectiveText(string message = "")
    {
        if (objectiveText == null)
            return;

        if (!string.IsNullOrEmpty(message))
        {
            objectiveText.text = message;
        }
        else if (currentSection < sections.Count)
        {
            objectiveText.text = "Current Objectives:\n" +
                "Talk to NPC: " + sections[currentSection].dialogueObjective + "\n" +
                "Quiz: " + sections[currentSection].quizObjective;
        }
        else
        {
            objectiveText.text = "All objectives complete!";
        }
    }
}
