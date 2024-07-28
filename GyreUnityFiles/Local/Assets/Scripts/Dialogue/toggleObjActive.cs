using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleObjActive : MonoBehaviour
{
    [SerializeField] GameObject objectToToggle;
    [SerializeField] bool destroyObj;
    private GameObject canvas;

    private void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("canvas");
    }
    public void setObjectActive()
    {
        objectToToggle.SetActive(true);
    }

    public void setObjectDisactive()
    {
        if (destroyObj)
        {
            Destroy(objectToToggle);
        }
        else
        {
            objectToToggle.SetActive(false);
        }
    }
}