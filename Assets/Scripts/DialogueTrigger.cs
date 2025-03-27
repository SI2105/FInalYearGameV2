using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool playerDetected;
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
        if (playerDetected && Input.GetKeyDown(KeyCode.E))
        {
            dialogue.StartDialogue();
        }
    }
}


