using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cycleImage : MonoBehaviour
{
    [SerializeField] GameObject imageToChange;
    public Sprite[] imagesToCycle;
    [SerializeField] float timeBetweenCycle;
    private int spriteArrayLength;
    private int currentImageNumber;
    void Start()
    {
        spriteArrayLength = imagesToCycle.Length;
        StartCoroutine(changeImage());
    }

    IEnumerator changeImage()
    {
        imageToChange.GetComponent<Image>().sprite = imagesToCycle[currentImageNumber];
        currentImageNumber++;
        if(currentImageNumber == spriteArrayLength)
        {
            currentImageNumber = 0;
        }
        yield return new WaitForSeconds(timeBetweenCycle);
        yield return StartCoroutine(changeImage());
    }
}
