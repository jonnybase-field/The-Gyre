using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartAnimation : MonoBehaviour
{
    [SerializeField] Animator jellyFishAnimator;
    [SerializeField] float constantStartTime;
    [SerializeField] bool randomisedStartTime;
    [SerializeField] float startTimeMin;
    [SerializeField] float startTimeMax;
    void Start()
    {
        jellyFishAnimator = gameObject.GetComponent<Animator>();
        if (randomisedStartTime)
        {
            constantStartTime = Random.Range(startTimeMin, startTimeMax);
        }
        jellyFishAnimator.Play(0, -1, constantStartTime);
    }
}
