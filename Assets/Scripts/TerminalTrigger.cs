using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalTrigger : MonoBehaviour
{
    public Quiz quiz;
    private bool playerDetected;
    public int objectiveIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ObjectiveManager.Instance.currentSection == objectiveIndex && ObjectiveManager.Instance.dialogueCompleted)
            {
                playerDetected = true;
                quiz.ToggleIndicator(true);
            }

            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            quiz.ToggleIndicator(false);
        }
    }

    private void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.E))
        {
            if (ObjectiveManager.Instance.currentSection == objectiveIndex && ObjectiveManager.Instance.dialogueCompleted)
            {
                quiz.StartQuiz();
            }
            else
            {
                UIManager.Instance.SetObjectiveText("You must talk to the NPC first!");
            }
        }
    }
}
