using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndSection : MonoBehaviour
{
    [SerializeField] GameObject endScreen1;
    [SerializeField] GameObject endScreen2;
    [SerializeField] GameObject endScreen3;
    [SerializeField] GameObject endScreen4;
    [SerializeField] GameObject endScreen5;
    [SerializeField] GameObject dialogueManager;
    [SerializeField] float screenChangeWaitTime;
    [SerializeField] GameObject screen1Guide;
    [SerializeField] GameObject screen2Guide;
    [SerializeField] GameObject screen3Guide;
    [SerializeField] GameObject onBoardingScreen1;
    [SerializeField] GameObject guideName;
    [SerializeField] GameObject guidePollution;
    [SerializeField] GameObject guideComparisonTitle;
    [SerializeField] GameObject guideComparisonDesc;
    private Animator guide3Animator;
    private Vector3 guide3Pos;

    private void Start()
    {
        guide3Animator = screen3Guide.GetComponent<Animator>();
    }
    public void goToScreenOne()
    {
        endScreen1.SetActive(true);
        dialogueManager.GetComponent<DialougeManager>().converseObj.SetActive(false);
        StartCoroutine(changeScreen());
        screen1Guide.GetComponent<Image>().sprite = dialogueManager.GetComponent<DialougeManager>().guideImageBox.GetComponent<Image>().sprite;
        screen2Guide.GetComponent<Image>().sprite = dialogueManager.GetComponent<DialougeManager>().guideImageBox.GetComponent<Image>().sprite;
        screen3Guide.GetComponent<Image>().sprite = dialogueManager.GetComponent<DialougeManager>().guideImageBox.GetComponent<Image>().sprite;
        foreach (Transform child in screen1Guide.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform image in dialogueManager.GetComponent<DialougeManager>().guideImageBox.transform)
        {
            var smallImage = Instantiate(image.gameObject, screen1Guide.transform.position, Quaternion.identity, screen1Guide.transform);
            smallImage.GetComponent<RectTransform>().sizeDelta = new Vector2(screen1Guide.GetComponent<RectTransform>().sizeDelta.x, screen1Guide.GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    private IEnumerator changeScreen()
    {
        yield return new WaitForSeconds(screenChangeWaitTime);
        goToScreenTwo();
    }

    public void goToScreenTwo()
    {
        endScreen1.SetActive(false);
        endScreen2.SetActive(true);
        foreach (Transform child in screen2Guide.transform)
        {
            Destroy(child.gameObject);

        }
        guidePollution.GetComponent<TextMeshProUGUI>().text = System.Math.Round(dialogueManager.GetComponent<CreateData>().pollutionAmount,2).ToString() + "g";
        guideName.GetComponent<TextMeshProUGUI>().text = dialogueManager.GetComponent<CreateData>().guideName;
        guideComparisonTitle.GetComponent<TextMeshProUGUI>().text = dialogueManager.GetComponent<DialougeManager>().lastComparisonMadeTitle ;
        guideComparisonDesc.GetComponent<TextMeshProUGUI>().text = dialogueManager.GetComponent<DialougeManager>().lastComparisonMadeDesc;
        foreach (Transform image in dialogueManager.GetComponent<DialougeManager>().guideImageBox.transform)
        {
            var smallImage = Instantiate(image.gameObject, screen2Guide.transform.position, Quaternion.identity, screen2Guide.transform);
            smallImage.GetComponent<RectTransform>().sizeDelta = new Vector2(screen2Guide.transform.parent.GetComponent<RectTransform>().sizeDelta.x, screen2Guide.transform.parent.GetComponent<RectTransform>().sizeDelta.y);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueManager.GetComponent<DialougeManager>().chatBoxesParent.GetComponent<RectTransform>());
    }

    public void goToScreenThree()
    {
        guide3Pos = screen3Guide.transform.position;
        endScreen2.SetActive(false);
        endScreen3.SetActive(true);
        foreach (Transform child in screen3Guide.transform)
        {
            Destroy(child.gameObject);

        }

        foreach (Transform image in dialogueManager.GetComponent<DialougeManager>().guideImageBox.transform)
        {
            var smallImage = Instantiate(image.gameObject, screen3Guide.transform.position, Quaternion.identity, screen3Guide.transform);
            smallImage.GetComponent<RectTransform>().sizeDelta = new Vector2(screen3Guide.GetComponent<RectTransform>().sizeDelta.x, screen3Guide.GetComponent<RectTransform>().sizeDelta.x);
        }
        guide3Animator.SetTrigger("release");
        StartCoroutine(changeScreenTwo());
    }

    private IEnumerator changeScreenTwo()
    {
        yield return new WaitForSeconds(screenChangeWaitTime);
        goToScreenFour();
    }
    public void goToScreenFour()
    {
        StartCoroutine(dialogueManager.GetComponent<CreateData>().activateGuide());
        guide3Animator.ResetTrigger("release");
        endScreen3.SetActive(false);
        screen3Guide.transform.position = guide3Pos;
        endScreen4.SetActive(true);
    }
    public void goToScreenFive()
    {
        endScreen4.SetActive(false);
        screen3Guide.transform.position = guide3Pos;
        endScreen5.SetActive(true);
    }

    public void startAgain()
    {
        endScreen5.SetActive(false);
        onBoardingScreen1.SetActive(true);
        dialogueManager.GetComponent<DialougeManager>().resetGuide();
        dialogueManager.GetComponent<DialougeManager>().resetChat();
    }
}
