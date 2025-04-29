using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    //acts as the trigger between player and the NPC and manages the interactions
    public Dialogue dialogue;
    private bool playerDetected;
    public int objectiveIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player") {
            playerDetected = true;
            dialogue.ToggleIndicator(playerDetected);
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerDetected = false;
            dialogue.ToggleIndicator(playerDetected);
        }

    }

    private void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.E) && !GameManager.Instance.gameCompleted)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.click);
            if (ObjectiveManager.Instance.currentSection == objectiveIndex)
            {
                dialogue.StartDialogue();
            }
            else {
                UIManager.Instance.ShowAlert("Complete your Current objective, come back to this person later", 2f);
            }
        }
    }
}


