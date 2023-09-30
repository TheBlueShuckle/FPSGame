using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    [SerializeField] Gun gun;

    private PlayerMotor motor;
    private PlayerLook look;
    private bool isSpinting;

    Coroutine fireCoroutine;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.SprintStart.performed += ctx => motor.StartSprint(onFoot.Movement.ReadValue<Vector2>());
        onFoot.SprintStop.performed += ctx => motor.StopSprint();

        onFoot.Shoot.started += _ => StartFiring();
        onFoot.Shoot.canceled += _ => StopFiring();
        onFoot.Reload.performed += _ => gun.StartCoroutine(gun.Reload());
    }

    void Update()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        gun.isMoving(onFoot.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    void StartFiring()
    {
        fireCoroutine = StartCoroutine(gun.RapidFire());
        gun.IsShooting(true);
    }

    void StopFiring()
    {
        if(fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            gun.IsShooting(false);
        }
    }

    #region - Enable / Disable -

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }

    #endregion
}
