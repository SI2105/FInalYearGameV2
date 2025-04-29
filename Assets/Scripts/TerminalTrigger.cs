using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalTrigger : MonoBehaviour
{
    //acts as the trigger between player and the terminal and manages the interactions
    public Quiz quiz;
    private bool playerDetected;
    public int objectiveIndex;
    
    //collider based trigger logic
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ObjectiveManager.Instance.currentSection == objectiveIndex && ObjectiveManager.Instance.dialogueCompleted)
            {
                playerDetected = true;
                quiz.toggleIndicator(true);
            }
            else if ((ObjectiveManager.Instance.currentSection == objectiveIndex && !ObjectiveManager.Instance.dialogueCompleted) || ObjectiveManager.Instance.currentSection < objectiveIndex)
            {
                playerDetected = true;
                quiz.toggleLockIndicator(true);
            }

            else if (quiz.Iscomplete) {
                playerDetected = true;
                quiz.toggleCompleteIndicator(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
            quiz.toggleIndicator(false);
            quiz.toggleLockIndicator(false);
            quiz.toggleCompleteIndicator(false);
        }
    }

    private void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.click);
            if (ObjectiveManager.Instance.currentSection == objectiveIndex && ObjectiveManager.Instance.dialogueCompleted)
            {
                quiz.StartQuiz();
            }
            else if(ObjectiveManager.Instance.currentSection == objectiveIndex)
            {
                UIManager.Instance.ShowAlert("You must talk to the NPC first!", 2f);
            }
            else if (ObjectiveManager.Instance.currentSection < objectiveIndex)
            {
                UIManager.Instance.ShowAlert("You must complete the previous section first!", 2f);
            }
            else if (quiz.Iscomplete)
            {
                UIManager.Instance.ShowAlert("You have already completed this Quiz!", 2f);
            }
        }
    }
}
