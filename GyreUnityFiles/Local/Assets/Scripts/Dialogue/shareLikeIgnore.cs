using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class shareLikeIgnore : MonoBehaviour
{
    [SerializeField] int answer;
    public GameObject pollutionImage;
    private GameObject dialogueManager;
    public bool globalImage = false;
    private string contextText = "none";

    private void Start()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("dialogueManager");
    }
    public void sendAnswer()
    {
        switch (answer)
        {
            case 1:
                {
                    dialogueManager.GetComponent<DialougeManager>().increasePollution(0.3f);
                    createPollutionImage();
                    StartCoroutine(dialogueManager.GetComponent<DialougeManager>().progressChatBox(false, null));
                    contextText = "You liked the image leading to " + (int)((dialogueManager.GetComponent<DialougeManager>().userMultipler * 10) + Random.Range(0, 10)) +" people seeing it";
                    break;
                }
            case 2:
                {
                    dialogueManager.GetComponent<DialougeManager>().increasePollution(0.1f);
                    StartCoroutine(dialogueManager.GetComponent<DialougeManager>().progressChatBox(true, null));
                    if (globalImage)
                    {
                        dialogueManager.GetComponent<CreateData>().globalStoriesURL[dialogueManager.GetComponent<DialougeManager>().globalEventsShown-1] = "none";
                    }
                    else
                    {
                        dialogueManager.GetComponent<CreateData>().imageURLS[dialogueManager.GetComponent<DialougeManager>().imagesShown-1] = "none";
                    }
                    contextText = "You disliked the image leading to " + (int)((dialogueManager.GetComponent<DialougeManager>().userMultipler * 3) + Random.Range(0, 5)) + " people seeing it";
                    break;
                }
            case 3:
                {
                    dialogueManager.GetComponent<DialougeManager>().increasePollution(0.5f);
                    StartCoroutine(dialogueManager.GetComponent<DialougeManager>().progressChatBox(false, null));
                    createPollutionImage();
                    contextText = "You shared the image leading to " + (int)((dialogueManager.GetComponent<DialougeManager>().userMultipler * 25) + Random.Range(0, 30)) + " people seeing it";
                    break;
                }
            case 4:
                {
                    StartCoroutine(dialogueManager.GetComponent<DialougeManager>().progressChatBox(true, gameObject.transform.parent.parent.parent.parent.gameObject));
                    if (globalImage)
                    {
                        dialogueManager.GetComponent<CreateData>().globalStoriesURL[dialogueManager.GetComponent<DialougeManager>().globalEventsShown-1] = "none";
                    }
                    else
                    {
                        dialogueManager.GetComponent<CreateData>().imageURLS[dialogueManager.GetComponent<DialougeManager>().imagesShown-1] = "none";
                    }
                    contextText = "You ignored and deleted the image which led to no one else seeing it";
                    break;
                }
        }
        removeButtonComp();
    }



    private void createPollutionImage()
    {

        GameObject guideIcon = dialogueManager.GetComponent<DialougeManager>().guideImageBox;
        float randomY = Random.Range(-5, 5);
        float randomX = Random.Range(-5, 5);
        Vector3 pollutionImagePos = new Vector3(guideIcon.transform.position.x + randomX, guideIcon.transform.position.y + randomY, guideIcon.transform.position.z);
        var newPollutionImage = Instantiate(pollutionImage, pollutionImagePos, Quaternion.identity, guideIcon.transform);
        if (globalImage)
        {
            newPollutionImage.GetComponent<Image>().sprite = dialogueManager.GetComponent<CreateData>().chosenEventSprites[dialogueManager.GetComponent<DialougeManager>().globalEventsShown-1];
        }
        else
        {
            newPollutionImage.GetComponent<Image>().sprite = dialogueManager.GetComponent<CreateData>().chosenImageSprites[dialogueManager.GetComponent<DialougeManager>().imagesShown-1];
        }
        foreach (Transform child in dialogueManager.GetComponent<DialougeManager>().smallGuideIcon.transform)
        {
            Destroy(child.gameObject);

        }

        foreach (Transform image in dialogueManager.GetComponent<DialougeManager>().guideImageBox.transform)
        {
            var smallImage = Instantiate(image.gameObject, dialogueManager.GetComponent<DialougeManager>().smallGuideIcon.transform.position, Quaternion.identity, dialogueManager.GetComponent<DialougeManager>().smallGuideIcon.transform);
            smallImage.GetComponent<RectTransform>().sizeDelta = new Vector2(dialogueManager.GetComponent<DialougeManager>().smallGuideIcon.GetComponent<RectTransform>().sizeDelta.x, dialogueManager.GetComponent<DialougeManager>().smallGuideIcon.GetComponent<RectTransform>().sizeDelta.y);
        }
    }


    private void removeButtonComp()
    {
        int buttonNum = 1;
        foreach (Transform button in gameObject.transform.parent)
        {
            if (button.GetComponent<Button>() != null)
            {
                if (buttonNum == answer)
                {
                    button.GetComponent<ProceduralImage>().color = button.GetComponent<Button>().colors.pressedColor;
                    button.GetChild(0).GetComponent<Image>().color = Color.black;
                }
                else
                {
                    button.GetComponent<ProceduralImage>().color = button.GetComponent<Button>().colors.normalColor;
                }
                buttonNum++;

                Destroy(button.GetComponent<Button>());
            }
        }
        dialogueManager.GetComponent<DialougeManager>().createGuideDialogueBox(contextText);
    }
}
