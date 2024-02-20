using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] Transform wayPointTop;
    [SerializeField] Transform wayPointBottom;
    [SerializeField] float speed;

    bool isAtTop;
    bool isMoving;
    float currentSpeed;

    Vector3 startPos, stopPos;

    private void Update()
    {
        if (isMoving)
        {
            Move(startPos, stopPos);
        }
    }

    public void StartElevator()
    {
        if (isAtTop)
        {
            startPos = wayPointTop.position;
            stopPos = wayPointBottom.position;
            currentSpeed = -speed;
        }

        else 
        {
            startPos = wayPointBottom.position;
            stopPos = wayPointTop.position;
            currentSpeed = speed;
        }

        isMoving = true;
    }

    private void Move(Vector3 startPos, Vector3 stopPos)
    {
        Vector3 position = transform.position;
        position.y += currentSpeed * Time.deltaTime;

        if (startPos.y > stopPos.y)
        {
            if (position.y <= stopPos.y)
            {
                position.y = stopPos.y;
                isMoving = false;
                isAtTop = false;
            }
        }

        else
        {
            if (position.y >= stopPos.y)
            {
                position.y = stopPos.y;
                isMoving = false;
                isAtTop = true;
            }
        }

        transform.position = position;
    }
}
