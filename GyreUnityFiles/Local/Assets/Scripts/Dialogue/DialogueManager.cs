using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI.ProceduralImage;
using System.Linq;

public class DialougeManager : MonoBehaviour
{
    [Serializable]
    public class dialogueQuestion
    {
        public string questionText;
        public bool singleAnswer;
        public bool slider;
        public string sliderMinValue;
        public string sliderMaxValue;
        public string[] answers;
    }

    public Queue<string> sentences;
    private Queue<string> guideSpecificDialogue;
    [SerializeField] float pauseBetweenMessages;
    [SerializeField] float timeToShowMessages;
    [SerializeField] GameObject guideChatBox;
    [SerializeField] GameObject guideQuestionBox;
    public GameObject chatBoxesParent;
    public GameObject gyreGuideDialogue;
    [SerializeField] GameObject gelizaImageBox;
    public Sprite gelizaImage;
    public GameObject guideNameBox;
    public GameObject guideImageBox;
    public Sprite guideIcon;
    public GameObject smallGuideIcon;
    public GameObject smallGuideName;
    [SerializeField] GameObject youChoseBox;
    [SerializeField] GameObject questionToggle;
    [SerializeField] GameObject questionSlider;
    [SerializeField] GameObject releaseButton;
    [SerializeField] GameObject progressUI;
    [SerializeField] GameObject guideSelectIcons;
    private bool introductionOver = false;
    public dialogueQuestion[] questions;
    private int questionNumber = 0;
    [SerializeField] GameObject imageBoxPrefab;
    public GameObject endScreens;
    public GameObject converseObj;
    public GameObject converseObjTop;
    public GameObject loadScreen;
    [SerializeField] float loadScreenFadeTime;
    [SerializeField] GameObject onboardingScreen1;
    [SerializeField] GameObject onboardingScreen2;
    [SerializeField] GameObject onboardingScreen3;
    [SerializeField] GameObject tutorialScreen;
    public GameObject LetsStartButton;
    private Animator gelizaAnimator;
    public int setenceAmount = 0;
    public float userMultipler;

    public GameObject canvas;
    public GameObject loadMenu;
    [SerializeField] GameObject endMenu;
    [SerializeField] float dialogueTextSpeed;
    [SerializeField] GameObject guideDescObj;
    public Camera playerCamera;
    public int dataQuestionNumber = 0;
    public int imagesShown = 0;
    public int globalEventsShown = 0;
    public bool onGuideLoadScreen = false;
    public bool generateGuideIcon = true;
    public string lastComparisonMadeTitle;
    public string lastComparisonMadeDesc;
    public string guideBio;
    

    private void Start()
    {
        resetGuide();
        sentences = new Queue<string>();
        guideSpecificDialogue = new Queue<string>();
        gelizaAnimator = gelizaImageBox.GetComponent<Animator>();
    }

    public void changeOnboardingScreen()
    {
        onboardingScreen1.SetActive(false);
        onboardingScreen2.SetActive(true);
        StartCoroutine(gameObject.GetComponent<CreateData>().downloadImagesOnStart());
    }

    public void startJourney()
    {
        LetsStartButton.GetComponent<Button>().interactable = false;
        onboardingScreen3.SetActive(false);
        converseObj.SetActive(true);
        gelizaImageBox.GetComponent<DialogueTrigger>().triggerDialogue();
    }

    public void addGuideSpecificDialogue(Dialogue dialogue)
    {
        guideSpecificDialogue.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            guideSpecificDialogue.Enqueue(sentence);
        }
        Debug.Log(sentences.Count);
    }

    public void startDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        Debug.Log(sentences.Count);
        setenceAmount = sentences.Count;
        displayNextSentence();
    }

    public void displayNextSentence()
    {
        if (sentences.Count == 0)
        {
            endDialogue();
        }
        else
        {
            bool iconNeeded = true;
            string sentence = sentences.Dequeue();
            if (sentence.Contains("(image)"))
            {
                sentence = showImage(sentence, false);
            }
            if (sentence.Contains("(event)"))
            {
                sentence = showImage(sentence, true);
            }
            if (sentence.Contains("(guide)"))
            {
                createGuideDialogueBox(sentence);
                iconNeeded = false;
            }
            if (sentence.Contains("(tutorial)"))
            {
                tutorialBox();
            }
            if (sentence.Contains("(question)"))
            {
                askQuestion();
            }
            if (sentence.Contains("(endOfExperience)"))
            {
                makeChatButton("Next");
            }
            if (iconNeeded)
            {
                generateGuideIcon = true;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatBoxesParent.GetComponent<RectTransform>());
        }
    }

    public void createGuideDialogueBox(string dialogueSentence)
    {
        if (!introductionOver)
        {
            gelizaAnimator.SetBool("Talking", false);
        }
        string sentence = dialogueSentence.Replace(" (guide)", string.Empty);
        if (sentence.Contains("(uniqueDialogue)"))
        {
            string updatedText = guideSpecificDialogue.Dequeue();
            sentence = updatedText;
        }
        var newTextBox = Instantiate(guideChatBox, chatBoxesParent.transform.position, Quaternion.identity, chatBoxesParent.transform);
        newTextBox.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = guideIcon;
        if (generateGuideIcon)
        {
            newTextBox.transform.GetChild(2).GetChild(0).GetComponent<Image>().enabled = true;
            newTextBox.transform.GetChild(2).GetComponent<ProceduralImage>().enabled = true;
            generateGuideIcon = false;
        }
        int childCount = 0;
        foreach (Transform child in newTextBox.transform)
        {
            if (childCount == 0)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
            childCount++;
        }
        float textCount = sentence.Length / timeToShowMessages;
        if (sentence.Contains("(auto)"))
        {
            sentence = sentence.Replace(" (auto)", string.Empty);
            StartCoroutine(displayChatBox(newTextBox, textCount, true));
        }
        else
        {
            StartCoroutine(displayChatBox(newTextBox, textCount, false));
        }
        newTextBox.transform.GetChild(1).GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = sentence;
    }

    private void askQuestion()
    {
        var newQuestionBox = Instantiate(guideQuestionBox, chatBoxesParent.transform.position, Quaternion.identity, chatBoxesParent.transform);
        newQuestionBox.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = questions[dataQuestionNumber].questionText;
        if (questions[dataQuestionNumber].slider)
        {
            var newSlider = Instantiate(questionSlider, newQuestionBox.transform.position, Quaternion.identity, newQuestionBox.transform.GetChild(1).GetChild(0));
            newSlider.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = questions[dataQuestionNumber].sliderMinValue;
            newSlider.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = questions[dataQuestionNumber].sliderMaxValue;
            newSlider.GetComponentInChildren<Slider>().minValue = int.Parse(questions[dataQuestionNumber].sliderMinValue);
            newSlider.GetComponentInChildren<Slider>().maxValue = int.Parse(questions[dataQuestionNumber].sliderMaxValue);
            newSlider.name = "questionSlider";
            newQuestionBox.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(true);
            newQuestionBox.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.GetComponent<respondToQuestion>().slider = true;
        }
        else
        {
            for (int i = 0; i < questions[dataQuestionNumber].answers.Length; i++)
            {
                var newAnswer = Instantiate(questionToggle, newQuestionBox.transform.position, Quaternion.identity, newQuestionBox.transform.GetChild(1).GetChild(0));
                newAnswer.GetComponentInChildren<TextMeshProUGUI>().text = questions[dataQuestionNumber].answers[i];
                newAnswer.GetComponent<respondToQuestion>().answerValue = i;
                newAnswer.GetComponent<respondToQuestion>().singleAnswer = questions[dataQuestionNumber].singleAnswer;
            }
        }
        int childCount = 0;
        foreach (Transform child in newQuestionBox.transform)
        {
            if (childCount == 0)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
            childCount++;
        }
        dataQuestionNumber++;
        StartCoroutine(displayChatBox(newQuestionBox, pauseBetweenMessages, false));
    }

    private IEnumerator displayChatBox(GameObject chatBox, float timeToWait, bool autoPlayNextSentence)
    {
        yield return new WaitForSeconds(timeToWait);
        int childCount = 0;
        foreach (Transform child in chatBox.transform)
        {
            if (childCount == 0)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
            childCount++;
        }
        if (!introductionOver)
        {
            gelizaAnimator.SetBool("Talking", true);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatBoxesParent.GetComponent<RectTransform>());
        if (!introductionOver)
        {
            yield return new WaitForSeconds(1);
        }
        else
        {
            yield return new WaitForSeconds(pauseBetweenMessages);

        }
        if (autoPlayNextSentence)
        {
            displayNextSentence();
        }
    }

    public void tutorialBox()
    {
        var newProgressBox = Instantiate(tutorialScreen, chatBoxesParent.transform.position, Quaternion.identity, chatBoxesParent.transform);
        StartCoroutine(displayChatBox(newProgressBox, pauseBetweenMessages, false));
    }


    public IEnumerator progressChatBox(bool positive, GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(pauseBetweenMessages*2);
        var newProgressBox = Instantiate(progressUI, chatBoxesParent.transform.position, Quaternion.identity, chatBoxesParent.transform);
        newProgressBox.GetComponent<progressStatus>().positive = positive;
        Debug.Log("this has ran");
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatBoxesParent.GetComponent<RectTransform>());
        if(objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
        yield return null;
    }

    public void answerQuestion(float[] answer)
    {
        questionNumber++;
        switch (questionNumber)
        {
            case 1:
                gameObject.GetComponent<CreateData>().contentType = answer[0];
                Debug.Log("Consumer Type");
                displayNextSentence();
                break;
            case 2:
                gameObject.GetComponent<CreateData>().interactionTypes = new float[answer.Length];
                for (int i = 0; i < answer.Length; i++)
                {
                    gameObject.GetComponent<CreateData>().interactionTypes[i] = answer[i];
                }
                displayNextSentence();
                Debug.Log("Interaction Types");
                break;
            case 3:
                gameObject.GetComponent<CreateData>().hoursADay = answer[0];
                Debug.Log("Hours a Day");
                displayNextSentence();
                break;
            case 4:
                gameObject.GetComponent<CreateData>().usersAge = answer[0];
                Debug.Log("Age");
                displayNextSentence();
                createUserMultipler();
                break;
        }
    }

    public void createYouChoseBox(string guideName, string guideDesc)
    {
        var newYouChoseBox = Instantiate(youChoseBox, chatBoxesParent.transform.position, Quaternion.identity, chatBoxesParent.transform);
        newYouChoseBox.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = guideDesc;
    }

    private void createUserMultipler()
    {
        float playerAge = gameObject.GetComponent<CreateData>().usersAge;
        float playerContentType = gameObject.GetComponent<CreateData>().contentType;
        float playerInteractionTypes = gameObject.GetComponent<CreateData>().interactionTypes.Length;
        float playerHours = gameObject.GetComponent<CreateData>().hoursADay;
        userMultipler = 1;
        userMultipler += (playerContentType / 75);
        userMultipler += (playerInteractionTypes / 50);
        userMultipler += (playerHours / 50);
        Debug.Log(userMultipler);
    }

    private string showImage(string newText, bool global)
    {
        converseObjTop.SetActive(false);
        increasePollution(0.05f);
        var newImageBox = Instantiate(imageBoxPrefab, chatBoxesParent.transform.position, Quaternion.identity, chatBoxesParent.transform);
        if (!global)
        {
            newImageBox.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<CreateData>().chosenImageSprites[imagesShown];
            newImageBox.transform.GetChild(1).GetChild(0).Find("Description").GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<CreateData>().imageDescriptions[imagesShown * 2];
            imagesShown++;
        }
        else
        {
            newImageBox.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<CreateData>().chosenEventSprites[globalEventsShown];
            foreach (Transform child in newImageBox.transform.GetChild(1).GetChild(0).GetChild(2).gameObject.transform)
            {
                child.GetComponent<shareLikeIgnore>().globalImage = true;
            }

            newImageBox.transform.GetChild(1).GetChild(0).Find("Description").GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<CreateData>().globalEventText[globalEventsShown];
            globalEventsShown++;
        }
        newText = newText.Replace(" (image)", string.Empty);
        StartCoroutine(displayChatBox(newImageBox, 0, false));
        return newText;
    }

    public void makeChatButton(string buttonText)
    {
        var newChatButton = Instantiate(releaseButton, chatBoxesParent.transform.position, Quaternion.identity, chatBoxesParent.transform);
        newChatButton.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
    }

    public void enterStatsPage()
    {
        endScreens.GetComponent<EndSection>().goToScreenOne();
        resetChat();
        converseObj.SetActive(false);
        introductionOver = true;
    }

    private void endDialogue()
    {
        if (!introductionOver)
        {
            guideDescObj.SetActive(true);
            gelizaImageBox.SetActive(false);
            guideNameBox.GetComponent<TextMeshProUGUI>().text = "Select a guide";
            guideImageBox.SetActive(true);
            var newImageBox = Instantiate(guideSelectIcons, chatBoxesParent.transform.position, Quaternion.identity, chatBoxesParent.transform);
            introductionOver = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatBoxesParent.GetComponent<RectTransform>());
        }
    }


    public void returnToLoadScreen()
    {
        gameObject.GetComponent<CreateData>().guideStatPageChatButton.gameObject.SetActive(true);
        gameObject.GetComponent<CreateData>().guideStatPageBackButton.gameObject.SetActive(false);
        gameObject.GetComponent<CreateData>().pollutionStat.text = "Energy Consumption = ";
    }

    public void toggleLoadMenu()
    {
        if (loadMenu.transform.GetChild(0).gameObject.activeSelf)
        {
            Debug.Log("Closing load menu");
            foreach (Transform child in loadMenu.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Opening load menu");
            foreach (Transform child in loadMenu.transform)
            {
                child.gameObject.SetActive(true);
            }
            resetGuide();
        }
    }
    public void increasePollution(float pollutionIncreaseAmount)
    {
        float pollutionAmount = gameObject.GetComponent<CreateData>().pollutionAmount;
        pollutionIncreaseAmount *= userMultipler;
        pollutionAmount += pollutionIncreaseAmount;
        gameObject.GetComponent<CreateData>().pollutionAmount = pollutionAmount;
    }

    public void resetChat()
    {
        foreach (Transform Textbox in chatBoxesParent.transform)
        {
            Destroy(Textbox.gameObject);
        }
        gelizaImageBox.SetActive(true);
        guideImageBox.SetActive(false);
    }

    public void resetGuide()
    {
        guideIcon = gelizaImage;
        dataQuestionNumber = 0;
        onGuideLoadScreen = false;
        questionNumber = 0;
        imagesShown = 0;
        globalEventsShown = 0;
        introductionOver = false;
        gameObject.GetComponent<CreateData>().pollutionAmount = 0;
        for (int i = 0; i < gameObject.GetComponent<CreateData>().imageURLS.Length; i++)
        {
            gameObject.GetComponent<CreateData>().imageURLS[i] = null;

        }
        for (int i = 0; i < gameObject.GetComponent<CreateData>().imageDescriptions.Length; i++)
        {
            gameObject.GetComponent<CreateData>().imageDescriptions[i] = null;

        }
        for (int i = 0; i < gameObject.GetComponent<CreateData>().globalEventText.Length; i++)
        {
            gameObject.GetComponent<CreateData>().globalEventText[i] = null;

        }
        for (int i = 0; i < gameObject.GetComponent<CreateData>().globalStoriesURL.Length; i++)
        {
            gameObject.GetComponent<CreateData>().globalStoriesURL[i] = null;
        }
        foreach (Transform child in guideImageBox.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
