using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
  
    public GameObject indicator;
   
    public List<string> dialogues;
    
    public float writingSpeed;
    private int index;
    // Character index
    private int charIndex;
    
    private bool started;
   
    private bool waitForNext;

    private bool skipWriting;





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

        GameManager.Instance.player.disableInput();
        started = true;

     
        UIManager.Instance.ShowDialogueWindow();

       
        ToggleIndicator(false);

     
        GetDialogue(0);
    }

    private void GetDialogue(int i)
    {
      
        index = i;

        // Reset the character index
        charIndex = 0;

        //Clears the quiz text through UIManager
        UIManager.Instance.ClearDialogueText();

        skipWriting = false;
        StartCoroutine(Writing());
        
    }

    // End Dialogue
    public void EndDialogue()
    {
        // Started is disabled
        started = false;

        // Disable wait for next as well
        waitForNext = false;

        StopAllCoroutines();
        UIManager.Instance.HideDialogueWindow();
        GameManager.Instance.player.enableInput();

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.CompleteDialogueObjective();
        }
    }

    // Writing logic
    IEnumerator Writing()
    {
        string currentDialogue = dialogues[index];

        // Continue writing characters until the end
        while (charIndex < currentDialogue.Length)
        {
            if (skipWriting)
            {
                // Skip rest of typing: display full dialogue immediately
                UIManager.Instance.dialogueText.text = currentDialogue;
                charIndex = currentDialogue.Length;
                
                break;
            }

            UIManager.Instance.dialogueText.text += currentDialogue[charIndex];
            UIManager.Instance.updateIndexText(index + 1, dialogues.Count);
            charIndex++;
            
            yield return new WaitForSeconds(writingSpeed);
        }

        // Once complete, allow moving to the next dialogue line
        waitForNext = true;
        UIManager.Instance.ShowEnter();
    }

    private void Update()
    {
         if (!started)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            if (!waitForNext)
            {
                //If text is still being written, skip to full text
                skipWriting = true;
                AudioManager.instance.PlaySFX(AudioManager.instance.click);
            }
            else
            {
                // Otherwise, move to the next dialogue
                waitForNext = false;
                index++;
                if (index < dialogues.Count)
                {
                    GetDialogue(index);
                }
                else
                {
                    ToggleIndicator(true);
                    EndDialogue();
                }
            }
        }
    }
}