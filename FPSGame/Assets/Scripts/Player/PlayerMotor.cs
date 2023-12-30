using System;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMotor : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] StatData statData;

    [Header("Jumping")]
    [SerializeField] float gravity;
    [SerializeField] float airResistance;

    [Header("Slide")]
    [SerializeField] float slideTimerMax = 2.5f;

    private Vector3 slideForward;
    private float slideTimer = 0.0f;

    private CharacterController controller;
    private Vector3 storedInput;
    private float fallingVelocity = 0.0f;
    private float storedSpeed = 0.0f;
    private float speedDropOffTimer = 0.0f;
    private float speedDropOffTimerMax = 0.0f;

    private float speed;
    private bool lerpCrouch;
    private float crouchTimer;

    public bool jumpButtonPressed = false;

    public float CurrentSpeedBuff { get; set; }
    public bool IsGrounded { get; private set; }
    public bool IsCrouched { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool isSliding { get; private set; }


    void Awake()
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
            float tempHeight = controller.height;
            slideTimer += Time.deltaTime;

            controller.height = Mathf.Lerp(controller.height, 1, slideTimer / slideTimerMax);

            float totalDistanceMoved = 0.0f;
            CollisionFlags flag = 0;

            while (totalDistanceMoved < (tempHeight - controller.height)/2 && flag != CollisionFlags.Below)
            {
                flag = controller.Move(new Vector3(0, -0.1f, 0));
                totalDistanceMoved += 0.1f;
            }

            if (slideTimer > slideTimerMax * 0.5f)
            {
                float slideSpeed = statData.stats[StatType.SlideSpeed].Value;
                float crouchSpeed = statData.stats[StatType.CrouchSpeed].Value;
                speed = Mathf.Lerp(slideSpeed, crouchSpeed, slideTimer / slideTimerMax);
            }

            if (slideTimer > slideTimerMax || !controller.isGrounded)
            {
                isSliding = false;
            }
        }
    }

    //receive inputs for inputmanager and apply them to the character controller. Runs every frame
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = new(input.x, 0, input.y);
        moveDirection = transform.TransformDirection(moveDirection);

        if (controller.isGrounded)
        {
            storedInput = moveDirection;
            storedSpeed = speed;

            if (input.x == 0 && input.y == 0)
            {
                storedSpeed = 0.0f;
            }

            speedDropOffTimer = 0.0f;
            speedDropOffTimerMax = Mathf.Sqrt(storedSpeed);
        }

        if (jumpButtonPressed && controller.isGrounded && !isSliding)
        {
            Jump();
        }

        jumpButtonPressed = false;

        fallingVelocity -= gravity * Time.deltaTime;

        if (controller.isGrounded && fallingVelocity < 0.0f)
        {
            fallingVelocity = -2.0f;
        }

        if (isSliding)
        {
            MovementSliding();
        }

        else if (controller.isGrounded)
        {
            MovementGrounded(moveDirection);
        }

        else
        {
            MovementFalling();
        }
    }

    private void Jump()
    {
        float jumpHeight = statData.stats[StatType.JumpHeight].Value;
        fallingVelocity = Mathf.Sqrt(jumpHeight * 3.0f * gravity);
        IsCrouched = false;
    }

    private void MovementSliding()
    {
        Vector3 tempSlideForward = speed * slideForward;
        tempSlideForward.y = fallingVelocity;

        controller.Move(Time.deltaTime * tempSlideForward);
    }

    private void MovementGrounded(Vector3 moveDirection)
    {
        moveDirection *= speed;
        moveDirection.y = fallingVelocity;
        controller.Move(Time.deltaTime * moveDirection);
    }

    private void MovementFalling()
    {
        speedDropOffTimer += Time.deltaTime;
        float tempSpeed = 0.0f;

        if (speedDropOffTimer < speedDropOffTimerMax)
        {
            tempSpeed = Mathf.Lerp(storedSpeed, 0, speedDropOffTimer / speedDropOffTimerMax);
        }

        Vector3 tempStoredInput = storedInput * tempSpeed;

        tempStoredInput.y = fallingVelocity;

        controller.Move(Time.deltaTime * tempStoredInput);
    }

    private void SetSpeed()
    {
        if (isSliding)
        {
            float slideSpeed = statData.stats[StatType.SlideSpeed].Value;
            speed = slideSpeed;
        }

        else if (IsCrouched)
        {
            float crouchSpeed = statData.stats[StatType.CrouchSpeed].Value;
            speed = crouchSpeed;
        }

        else if (IsSprinting)
        {
            float sprintSpeed = statData.stats[StatType.SprintSpeed].Value;
            speed = sprintSpeed;
        }

        else
        {
            float defaultSpeed = statData.stats[StatType.DefaultMovementSpeed].Value;
            speed = defaultSpeed;
        }

        if (CurrentSpeedBuff != 0)
        {
            speed *= CurrentSpeedBuff;
            print("Speed increased x" + CurrentSpeedBuff);
        }
    }

    public void Crouch()
    {
        if (IsGrounded)
        {
            IsCrouched = !IsCrouched;
            crouchTimer = 0;
            lerpCrouch = true;

            if (IsCrouched && IsSprinting)
            {
                Slide();
            }
        }

    }

    private void Slide()
    {
        isSliding = true;
        IsSprinting = false;

        float slideSpeed = statData.stats[StatType.SlideSpeed].Value;
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
