using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class progressStatus : MonoBehaviour
{
    [Serializable]
    public class comparisons
    {
        public string comparisonTitle;
        public string comparisonDesc;
        public Sprite comparisonSprite;
        public float co2perUnit;
    }
    private int progressAmount;
    private float pollutionAmount;
    private GameObject dialogueManager;
    [SerializeField] GameObject carbonBar;
    [SerializeField] GameObject progressBar;
    [SerializeField] GameObject feedbackImage;
    [SerializeField] Sprite positiveImage;
    [SerializeField] Sprite negativeImage;
    [SerializeField] GameObject feedbackTextObjTitle;
    [SerializeField] GameObject feedbackTextObjDesc;
    [SerializeField] string feedbackTextPosTitle;
    [SerializeField] string feedbackTextPosDesc;
    [SerializeField] string feedbackTextNegTitle;
    [SerializeField] string feedbackTextNegDesc;
    [SerializeField] comparisons[] comparisonList;
    [SerializeField] GameObject comparisonTitle;
    [SerializeField] GameObject comparisonDesc;
    [SerializeField] GameObject comparisonImg;
    [SerializeField] float timeToWait;
    [SerializeField] GameObject handle;
    public bool positive = false;
    void Start()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("dialogueManager");
        int maxSetences = dialogueManager.GetComponent<DialougeManager>().setenceAmount;
        progressAmount = maxSetences - dialogueManager.GetComponent<DialougeManager>().sentences.Count;
        pollutionAmount = dialogueManager.GetComponent<CreateData>().pollutionAmount;
        progressBar.GetComponent<Slider>().maxValue = maxSetences;
        progressBar.GetComponent<Slider>().value = progressAmount;
        carbonBar.GetComponent<Slider>().value = pollutionAmount;
        pollutionAmount = Mathf.Round(pollutionAmount * 10.0f) * 0.1f;
        handle.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pollutionAmount.ToString() + "g";
        showFeedback();
        showComparison();
        StartCoroutine(nextMessage());
    }

    private void showFeedback()
    {
        Debug.Log(positive);
        if (positive)
        {
            feedbackImage.GetComponent<Image>().sprite = positiveImage;
            feedbackTextObjTitle.GetComponent<TextMeshProUGUI>().text = feedbackTextPosTitle;
            feedbackTextObjDesc.GetComponent<TextMeshProUGUI>().text = feedbackTextPosDesc;
        }
        else
        {
            feedbackImage.GetComponent<Image>().sprite = negativeImage;
            feedbackTextObjTitle.GetComponent<TextMeshProUGUI>().text = feedbackTextNegTitle;
            feedbackTextObjDesc.GetComponent<TextMeshProUGUI>().text = feedbackTextNegDesc;
        }
    }

    private void showComparison()
    {
        int comparisonNum = Random.Range(0, comparisonList.Length);

        comparisonTitle.GetComponent<TextMeshProUGUI>().text = comparisonList[comparisonNum].comparisonTitle;
        string comparisonDescText = comparisonList[comparisonNum].comparisonDesc;
        float currentNum;
        switch (comparisonNum)
        {
            case 0:
                currentNum = pollutionAmount /comparisonList[comparisonNum].co2perUnit;
                comparisonDescText += System.Math.Round(currentNum, 5) + "km";    
                break;
            case 1:
                currentNum = pollutionAmount / comparisonList[comparisonNum].co2perUnit;
                comparisonDescText += System.Math.Round(currentNum, 2) + " hours";
                break;
            case 2:
                currentNum = pollutionAmount / comparisonList[comparisonNum].co2perUnit;
                comparisonDescText += System.Math.Round(currentNum, 2) + " cups worth of water";
                break;
        }
        Debug.Log(comparisonDescText);
        comparisonDesc.GetComponent<TextMeshProUGUI>().text = comparisonDescText;

        comparisonImg.GetComponent<Image>().sprite = comparisonList[comparisonNum].comparisonSprite;
        dialogueManager.GetComponent<DialougeManager>().lastComparisonMadeTitle = comparisonList[comparisonNum].comparisonTitle;
        dialogueManager.GetComponent<DialougeManager>().lastComparisonMadeDesc = comparisonDescText;
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueManager.GetComponent<DialougeManager>().chatBoxesParent.GetComponent<RectTransform>());
    }

    private IEnumerator nextMessage()
    {
        yield return new WaitForSeconds(timeToWait);
        dialogueManager.GetComponent<DialougeManager>().converseObjTop.SetActive(true);
        dialogueManager.GetComponent<DialougeManager>().displayNextSentence();
    }
}
