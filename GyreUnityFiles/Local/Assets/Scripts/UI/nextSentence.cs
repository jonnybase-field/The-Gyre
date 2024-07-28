using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextSentence : MonoBehaviour
{
    private GameObject dialogueManager;
    void Start()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("dialogueManager");
    }

    public void buttonPressed()
    {
        dialogueManager.GetComponent<DialougeManager>().displayNextSentence();
        Destroy(gameObject);
    }
}
