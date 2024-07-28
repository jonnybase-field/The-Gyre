using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotation : MonoBehaviour
{
    [SerializeField] float xSpreadMin, xSpreadMax;
    [SerializeField] bool randomiseYPos;
    [SerializeField] float randomYPosMin, randomYPosMax;
    [SerializeField] float ySpreadMin, ySpreadMax;
    [SerializeField] float zPosMin, zPosMax;
    [SerializeField] float rotationSpeedMin, rotationSpeedMax;
    [SerializeField] bool rotateClockwise;
    [SerializeField] Vector3 centerPoint;
    [SerializeField] bool startOffscreenAndMoveDownwards;
    [SerializeField] float yMoveSpeed;
    [SerializeField] float offScreenStartPos;
    private float bonusY;
    private float timer = 0;
    private float xSpreadIndividual, ySpreadIndividual, zPosIndividual, rotationSpeedIndividual;
    private void Start()
    {
        xSpreadIndividual = Random.Range(xSpreadMin, xSpreadMax);
        ySpreadIndividual = Random.Range(ySpreadMin, ySpreadMax);
        zPosIndividual = Random.Range(zPosMin, zPosMax);
        rotationSpeedIndividual = Random.Range(rotationSpeedMin, rotationSpeedMax);
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, zPosIndividual);
        if (randomiseYPos)
        {
            centerPoint = new Vector3(centerPoint.x, Random.Range(randomYPosMin, randomYPosMax), centerPoint.z);
        }
        if (startOffscreenAndMoveDownwards)
        {
            bonusY += offScreenStartPos;
        }
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime * rotationSpeedIndividual;
        if(bonusY > centerPoint.y)
        {
            bonusY -= yMoveSpeed;
        }
        Rotate();
    }

    void Rotate()
    {
        float x;
        if (rotateClockwise)
        {
            x = -Mathf.Cos(timer) * xSpreadIndividual;
        }
        else
        {
            x = Mathf.Cos(timer) * xSpreadIndividual;
        }
        float y = (Mathf.Sin(timer) * ySpreadIndividual) + bonusY;
        Vector3 pos = new Vector3(x, y, gameObject.transform.position.z);
        gameObject.transform.position = pos + centerPoint;
    }
}
