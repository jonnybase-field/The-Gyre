using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] Vector3 centerPoint;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(centerPoint);
    }
}
