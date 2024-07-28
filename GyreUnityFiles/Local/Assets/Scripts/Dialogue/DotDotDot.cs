using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DotDotDot : MonoBehaviour
{
    [SerializeField] float dotCreationSpeed;
    [SerializeField] int maxDots;
    private int dotsAdded = 0;
    void Start()
    {
        Invoke("addDot", dotCreationSpeed);
    }

    private void addDot()
    {
        dotsAdded++;
        if (dotsAdded < maxDots)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text += ".";
        }
        else
        {
            reset();
        }
        Invoke("addDot", dotCreationSpeed);
    }

    private void reset()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "";
        dotsAdded = 0;
    }
}
