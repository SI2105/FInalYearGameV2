using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool playerDetected;
    public int objectiveIndex;
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player") {
            //if (ObjectiveManager.Instance.currentSection == objectiveIndex)
            //{
            //    playerDetected = true;
            //    dialogue.ToggleIndicator(playerDetected);
            //}
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
        if (playerDetected && Input.GetKeyDown(KeyCode.E))
        {
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


