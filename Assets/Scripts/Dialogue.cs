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

        charIndex = 0;

        UIManager.Instance.ClearDialogueText();

        skipWriting = false;
        StartCoroutine(Writing());
        
    }

    public void EndDialogue()
    {
     
        started = false;

        waitForNext = false;

        StopAllCoroutines();
        UIManager.Instance.HideDialogueWindow();
        GameManager.Instance.player.enableInput();

        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.CompleteDialogueObjective();
        }
    }

    //writing animation logic
    IEnumerator Writing()
    {
        string currentDialogue = dialogues[index];

        
        while (charIndex < currentDialogue.Length)
        {
            if (skipWriting)
            {
                //checks if writing animation is being skipped
                UIManager.Instance.dialogueText.text = currentDialogue;
                charIndex = currentDialogue.Length;
                
                break;
            }

            UIManager.Instance.dialogueText.text += currentDialogue[charIndex];
            UIManager.Instance.updateIndexText(index + 1, dialogues.Count);
            charIndex++;
            
            yield return new WaitForSeconds(writingSpeed);
        }

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
                //handles dialogue writing animation skip
                skipWriting = true;
                AudioManager.instance.PlaySFX(AudioManager.instance.click);
            }
            else
            {
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