using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{


    public Dialogue mainDialogue;
    public Dialogue IndyG;
    public Dialogue NPac;
    public Dialogue Norfi;
    public Dialogue Suta;
    public Dialogue StickyO;
    private GameObject scriptManager;
    public void triggerDialogue()
    {
        scriptManager = GameObject.FindGameObjectWithTag("dialogueManager");
        int guideNumber = scriptManager.GetComponent<CreateData>().guideNumber;
        switch (guideNumber)
        {
            case 1:
                {
                    FindObjectOfType<DialougeManager>().addGuideSpecificDialogue(IndyG);
                    break;
                }
            case 2:
                {
                    FindObjectOfType<DialougeManager>().addGuideSpecificDialogue(NPac);
                    break;
                }
            case 3:
                {
                    FindObjectOfType<DialougeManager>().addGuideSpecificDialogue(Norfi);
                    break;
                }
            case 4:
                {
                    FindObjectOfType<DialougeManager>().addGuideSpecificDialogue(Suta);
                    break;
                }
            case 5:
                {
                    FindObjectOfType<DialougeManager>().addGuideSpecificDialogue(StickyO);
                    break;
                }
        }
        FindObjectOfType<DialougeManager>().startDialogue(mainDialogue);
    }
}


