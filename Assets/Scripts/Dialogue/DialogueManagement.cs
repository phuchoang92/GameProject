using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManagement : MonoBehaviour, ISaveable
{
    [SerializeField] TextMeshProUGUI text;
    public Animator animator;
    private Queue<string> sentences;
    private bool isDisplay=false;

    private void Awake()
    {
        sentences = new Queue<string>();
    }
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        isDisplay = true;
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count> 0) 
        { 
            // text.text = sentences.Dequeue();
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        else
        {
            animator.SetBool("IsOpen", false);
            isDisplay = false;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        text.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return null;
        }
    }
    public object CaptureState()
    {
        return isDisplay;
    }

    public void RestoreState(object state)
    {
        animator.SetBool("IsOpen", (bool)state);
        sentences.Clear();
    }
}
