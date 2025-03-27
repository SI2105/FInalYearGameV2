using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    // Indicator
    public GameObject indicator;

    // Dialogues list
    public List<string> dialogues;

    // Writing speed
    public float writingSpeed;

    // Index on dialogue
    private int index;

    // Character index
    private int charIndex;

    // Started boolean
    private bool started;

    // Wait for next boolean
    private bool waitForNext;



    private void Awake()
    {
        ToggleIndicator(false);
    }

    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }


    public void StartDialogue()
    {
        if (started)
            return;

       
        started = true;

     
        UIManager.Instance.ShowDialogueWindow();

        // Hide the indicator
        ToggleIndicator(false);

     
        GetDialogue(0);
    }

    private void GetDialogue(int i)
    {
      
        index = i;

        // Reset the character index
        charIndex = 0;

        // Clear the dialogue text through UIManager
        UIManager.Instance.ClearDialogueText();

        // Start writing
        StartCoroutine(Writing());
    }

    // End Dialogue
    public void EndDialogue()
    {
        // Started is disabled
        started = false;

        // Disable wait for next as well
        waitForNext = false;

        // Stop all IEnumerators
        StopAllCoroutines();

        // Hide the window through UIManager
        UIManager.Instance.HideDialogueWindow();
    }

    // Writing logic
    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);

        UIManager.Instance.HideEnter(); 

        string currentDialogue = dialogues[index];


        

        UIManager.Instance.dialogueText.text += currentDialogue[charIndex];
  

        // Increase the character index 
        charIndex++;

        // Make sure you have reached the end of the sentence
        if (charIndex < currentDialogue.Length)
        {
            // Wait x seconds 
            yield return new WaitForSeconds(writingSpeed);

            // Restart the same process
            StartCoroutine(Writing());
        }
        else
        {
            // End this sentence and wait for the next one
            waitForNext = true;
            UIManager.Instance.ShowEnter();
        }
    }

    private void Update()
    {
        if (!started)
            return;

        if (waitForNext && Input.GetKeyDown(KeyCode.Return))
        {
            waitForNext = false;
            index++;

            // Check if we are in the scope of dialogues List
            if (index < dialogues.Count)
            {
                // If so fetch the next dialogue
                GetDialogue(index);
            }
            else
            {
                // If not end the dialogue process
                ToggleIndicator(true);
                EndDialogue();
            }
        }
    }
}