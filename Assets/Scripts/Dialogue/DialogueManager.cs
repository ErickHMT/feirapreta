using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    
    public Text dialogueText;
    private Queue<string> sentences;
    public Animator animator;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue) {
        animator.SetBool("IsOpen", true);
        Debug.Log("Starting conversation: " + dialogue.name);
        sentences.Clear();

        foreach(string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence () {
         Debug.Log("Próximo.");
        if(sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        StartCoroutine("WaitForNextSentence");
    }

    void EndDialogue() 
    {
        animator.SetBool("IsOpen", false);
        Debug.Log("End of conversation.");
    }

    IEnumerator WaitForNextSentence() {
        yield return new WaitForSeconds(2f);
        DisplayNextSentence();
    }

    

}
