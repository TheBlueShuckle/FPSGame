using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ElevatorHandler : MonoBehaviour
{
    [SerializeField] Transform wayPointTop;
    [SerializeField] Transform wayPointBottom;
    [SerializeField] float speed;

    public bool IsAtTop {  get; private set; }
    public bool IsMoving { get; private set; }
    float currentSpeed;

    Vector3 startPos, stopPos;

    private void Update()
    {
        if (IsMoving)
        {
            Move(startPos, stopPos);
        }
    }

    public void StartElevator()
    {
        if (IsAtTop)
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

        IsMoving = true;
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
                IsMoving = false;
                IsAtTop = false;
            }
        }

        else
        {
            if (position.y >= stopPos.y)
            {
                position.y = stopPos.y;
                IsMoving = false;
                IsAtTop = true;
            }
        }

        transform.position = position;
    }
}
