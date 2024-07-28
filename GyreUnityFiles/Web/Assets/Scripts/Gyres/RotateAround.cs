using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] GameObject rotationPoint;
    [SerializeField] float speed;

    private void FixedUpdate()
    {
        var fixedSpeed = speed * Time.deltaTime;
        gameObject.transform.RotateAround(rotationPoint.transform.position, new Vector3(0, 1, 0), fixedSpeed);
    }
}
