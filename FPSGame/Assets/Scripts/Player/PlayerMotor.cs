using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public float defaultSpeed;
    public float gravity;
    public float jumpHeight;
    public bool lerpCrouch;
    public float crouchTimer;

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

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;

            if (isCrouched)
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
            speed = defaultSpeed * 2f;
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
            velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    public void Crouch()
    {
        isCrouched = !isCrouched;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void StartSprint()
    {
        isSprinting = true;
    }

    public void StopSprint()
    {
        isSprinting = false;
    }
}
