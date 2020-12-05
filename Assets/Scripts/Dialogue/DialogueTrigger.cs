using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    
    public Dialogue dialogue;
    //Apresenta diálogo apenas se ele não tiver sido apresentado uma vez
    private bool triggered = false; 

    public void TriggerDialogue() 
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !triggered) {
            triggered = true;
            TriggerDialogue();
        }        
    }

}
