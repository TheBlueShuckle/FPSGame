using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float defaultSpeed;
    public float gravity;
    public float jumpHeight;

    private CharacterController controller;
    private Vector3 velocity;

    private float speed;

    private bool isGrounded;
    private bool isCrouched;
    private bool isSprinting;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    //receive inputs for inputmanager and apply them to the character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (isCrouched)
        {
            speed = defaultSpeed / 3f;
        }

        else if (isSprinting)
        {
            speed = defaultSpeed * 1.5f;
        }

        else
        {
            speed = defaultSpeed;
        }

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y  = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    public void ToggleCrouch()
    {
        isCrouched = !isCrouched;
        Debug.Log("Toggled Crouching");
    }

    public void SprintingPressed()
    {
        isSprinting = true;
        Debug.Log("Started sprinting");
    }

    public void SprintingReleased()
    {
        isSprinting = false;
        Debug.Log("Stopped sprinting");
    }
}
