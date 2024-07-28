using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField] float xSpin;
    [SerializeField] float ySpin;
    [SerializeField] float zSpin;
    void Start()
    {
        ySpin = Random.Range(5, 15) * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(xSpin, ySpin, zSpin, Space.Self);
    }
}
