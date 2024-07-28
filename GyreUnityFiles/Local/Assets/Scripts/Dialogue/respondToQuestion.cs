using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class respondToQuestion : MonoBehaviour
{
    public float answerValue;
    public int questionNumber;
    public bool singleAnswer;
    private GameObject dialogueManager;
    public bool slider;
    void Start()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("dialogueManager");
    }

    private void FixedUpdate()
    {
        if (slider)
        {
            answerValue = gameObject.transform.parent.Find("questionSlider").GetComponentInChildren<Slider>().value;
            gameObject.transform.parent.Find("questionSlider").GetComponentInChildren<Slider>().gameObject.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = answerValue.ToString();
        }
    }

    public void toggleClicked()
    {
        if (gameObject.GetComponent<Toggle>().isOn)
        {
            if (singleAnswer)
            {
                int answerNumber = 0;
                foreach (Transform answer in gameObject.transform.parent)
                {
                    if (answer.gameObject.GetComponent<Toggle>() != null)
                    {
                        if (answerValue != answerNumber)
                        {
                            answer.gameObject.GetComponent<Toggle>().isOn = false;
                        }
                        answerNumber++;
                    }
                }
            }
        }
        transform.parent.GetChild(1).gameObject.SetActive(false);
        foreach (Transform answer in gameObject.transform.parent)
        {
            if (answer.gameObject.GetComponent<Toggle>() != null)
            {
                if (answer.gameObject.GetComponent<Toggle>().isOn)
                {
                    transform.parent.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }

    public void submitAnswers()
    {
        gameObject.SetActive(false);
        int answersArraySize = 0;
        foreach (Transform answer in gameObject.transform.parent)
        {
            if (answer.gameObject.GetComponent<Toggle>() != null)
            {
                if (answer.gameObject.GetComponent<Toggle>().isOn)
                {
                    answersArraySize++;
                }
            }
        }
        float[] answersToSubmit = new float[answersArraySize];
        int toggleBoxNumber = 0;
        foreach (Transform answer in gameObject.transform.parent)
        {
            if (answer.gameObject.GetComponent<Toggle>() != null)
            {
                if (answer.gameObject.GetComponent<Toggle>().isOn)
                {
                    answersToSubmit[toggleBoxNumber] = answer.gameObject.GetComponent<respondToQuestion>().answerValue;
                    toggleBoxNumber++;
                }
            }
        }
        foreach (Transform answer in gameObject.transform.parent)
        {
            if (answer.gameObject.GetComponent<Toggle>() != null)
            {
                answer.gameObject.GetComponent<Toggle>().enabled = false;
            }
        }
        if (slider)
        {
            gameObject.transform.parent.Find("questionSlider").GetComponentInChildren<Slider>().enabled = false;
        }
        if (singleAnswer)
        {
            dialogueManager.GetComponent<DialougeManager>().answerQuestion(answersToSubmit);
        }
        else
        {
            if (slider)
            {
                answersToSubmit = new float[1];
                answersToSubmit[0] = answerValue;
                dialogueManager.GetComponent<DialougeManager>().answerQuestion(answersToSubmit);
            }
            else
            {
                dialogueManager.GetComponent<DialougeManager>().answerQuestion(answersToSubmit);
            }
        }
        Destroy(gameObject.GetComponent<respondToQuestion>());
    }

    public void releaseButton()
    {
        dialogueManager.GetComponent<CreateData>().sendDataButton();
        dialogueManager.GetComponent<DialougeManager>().enterStatsPage();
    }
}
