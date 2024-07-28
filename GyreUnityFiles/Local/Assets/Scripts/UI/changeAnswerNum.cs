using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeAnswerNum : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    [SerializeField] int answerNum;

    public void changeAnswer()
    {
        if(targetObject.activeSelf == false)
        {
            targetObject.SetActive(true);
        }
        targetObject.GetComponent<respondToQuestion>().answerValue = answerNum;
    }
}
