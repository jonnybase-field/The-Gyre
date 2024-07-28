using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class iconButton : MonoBehaviour {

    [SerializeField] GameObject dialogueManager;
    [SerializeField] int guideNumber;
    [SerializeField] string guideName;
    [SerializeField] Sprite guideImage;
    private GameObject guideImageObj;
    [SerializeField] GameObject submitButton;
    [SerializeField] GameObject objectToDestroy;
    [SerializeField] string guideDesc;
    [SerializeField] string guideBio;
    private GameObject guideDescObj;

    private void Awake()
    {
        guideImageObj = GameObject.FindGameObjectWithTag("guideImg");
        dialogueManager = GameObject.FindGameObjectWithTag("dialogueManager");
        guideDescObj = GameObject.FindGameObjectWithTag("guideDescObj");
    }
    public void buttonClicked()
    {
        guideImageObj.GetComponent<Image>().sprite = guideImage;
        submitButton.SetActive(true);
        dialogueManager.GetComponent<CreateData>().guideNumber = guideNumber;
        dialogueManager.GetComponent<DialougeManager>().guideNameBox.GetComponent<TextMeshProUGUI>().text = guideName;
        dialogueManager.GetComponent<DialougeManager>().guideIcon = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite;
        guideDescObj.GetComponent<TextMeshProUGUI>().text = guideDesc;
        dialogueManager.GetComponent<DialougeManager>().guideBio = guideBio;
    }

    public void submitAnswer()
    {
        dialogueManager.GetComponent<DialougeManager>().createYouChoseBox(guideName, dialogueManager.GetComponent<DialougeManager>().guideBio);
        dialogueManager.GetComponent<DialougeManager>().generateGuideIcon = true;
        Sprite chosenIcon = dialogueManager.GetComponent<DialougeManager>().guideIcon;
        dialogueManager.GetComponent<DialougeManager>().guideIcon = dialogueManager.GetComponent<DialougeManager>().gelizaImage;
        dialogueManager.GetComponent<DialougeManager>().createGuideDialogueBox("Bye for now!");
        dialogueManager.GetComponent<DialougeManager>().guideIcon = chosenIcon;
       dialogueManager.GetComponent<DialougeManager>().generateGuideIcon = true;
        dialogueManager.GetComponent<DialougeManager>().smallGuideIcon.GetComponent<Image>().sprite = guideImageObj.GetComponent<Image>().sprite;
        dialogueManager.GetComponent<DialougeManager>().smallGuideName.GetComponent<TextMeshProUGUI>().text = dialogueManager.GetComponent<DialougeManager>().guideNameBox.GetComponent<TextMeshProUGUI>().text;
        foreach (Transform child in dialogueManager.GetComponent<DialougeManager>().smallGuideIcon.transform)
        {
            Destroy(child.gameObject);

        }
        dialogueManager.GetComponent<DialougeManager>().gyreGuideDialogue.GetComponent<DialogueTrigger>().triggerDialogue();
        Destroy(objectToDestroy);
        guideDescObj.SetActive(false);
    }

}
