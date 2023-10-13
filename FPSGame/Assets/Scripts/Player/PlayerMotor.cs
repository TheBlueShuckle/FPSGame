using System;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [Header("Speeds")]
    public float defaultSpeed;
    public float crouchSpeed;
    public float sprintSpeed;

    [Header("Jumping")]
    public float gravity;
    public float jumpHeight;

    [Header("Crouch")]
    public bool lerpCrouch;
    public float crouchTimer;

    private CharacterController controller;
    private Vector3 velocity;

    private float speed;

    public bool IsGrounded { get; private set; }
    public bool IsCrouched { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool isSliding { get; private set; }

    [Header("Slide")]
    public float slideSpeed = 20;
    public float slideTimerMax = 2.5f;
    private Vector3 slideForward;
    private float slideTimer = 0.0f;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        isSliding = false;
    }

    void Update()
    {
        IsGrounded = controller.isGrounded;
        SetSpeed();

        if (lerpCrouch && !isSliding)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;

            if (IsCrouched)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
            }

            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0;
            }
        }

        if (isSliding)
        {
            slideTimer += Time.deltaTime;
            controller.height = Mathf.Lerp(controller.height, 1, 5 * Time.deltaTime);

            if (slideTimer > slideTimerMax * 0.5f)
            {
                speed = Mathf.Lerp(slideSpeed, crouchSpeed, slideTimer / slideTimerMax);
            }

            if (slideTimer > slideTimerMax)
            {
                isSliding = false;
            }
        }
    }

    //receive inputs for inputmanager and apply them to the character controller
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (isSliding)
        {
            controller.Move(slideForward * speed * Time.deltaTime);
        }

        else
        {
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;

        if (IsGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void SetSpeed()
    {
        if (isSliding)
        {
            speed = slideSpeed;
        }

        else if (IsCrouched)
        {
            speed = crouchSpeed;
        }

        else if (IsSprinting)
        {
            speed = sprintSpeed;
        }

        else
        {
            speed = defaultSpeed;
        }

        Debug.Log(speed);
    }

    public void Jump()
    {
        if (IsGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    public void Crouch()
    {
        IsCrouched = !IsCrouched;
        crouchTimer = 0;
        lerpCrouch = true;

        if (IsCrouched && IsSprinting && IsGrounded)
        {
            Slide();
        }
    }

    private void Slide()
    {
        isSliding = true;
        IsSprinting = false;
        speed = slideSpeed;
        slideTimer = 0;
        slideForward = transform.forward;
    }

    public void StartSprint(Vector2 input)
    {
        if (input.y > 0)
        {
            IsSprinting = true;
            IsCrouched = false;
            lerpCrouch = true;
        }
    }

    public void StopSprint()
    {
        IsSprinting = false;
    }

    public void UpdateSprinting(Vector2 input)
    {
        if (input.y <= 0)
        {
            StopSprint();
        }
    }
}
