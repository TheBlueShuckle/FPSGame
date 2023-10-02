using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    private Gun gun;
    private PickUpController pickUpController;

    private PlayerMotor motor;
    private PlayerLook look;
    private bool isSpinting;

    Coroutine fireCoroutine;

    void Awake()
    {
        if (gameObject.GetComponentInChildren<Gun>() != null)
        {
            gun = gameObject.GetComponentInChildren<Gun>();
        }

        if (gameObject.GetComponentInChildren<PickUpController>() != null)
        {
            pickUpController = gameObject.GetComponentInChildren<PickUpController>();
        }

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
        onFoot.Reload.performed += _ => Reload();

        onFoot.Drop.performed += _ => DropGun();
    }

    void Update()
    {
        if (gun == null && gameObject.GetComponentInChildren<Gun>() != null)
        {
            gun = gameObject.GetComponentInChildren<Gun>();
        }

        if (pickUpController == null)
        {
            pickUpController = gameObject.GetComponentInChildren<PickUpController>();
        }

        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());

        if (gun != null)
        {
            gun.isMoving(onFoot.Movement.ReadValue<Vector2>());
        }
    }

    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    void StartFiring()
    {
        if (gun != null)
        {
            fireCoroutine = StartCoroutine(gun.RapidFire());
            gun.IsShooting(true);
        }
    }

    void StopFiring()
    {
        if (gun != null)
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                gun.IsShooting(false);
            }
        }
    }

    void Reload()
    {
        if (gun != null)
        {
            gun.StartCoroutine(gun.Reload());
        }
    }

    void DropGun()
    {
        if (pickUpController != null)
        {
            gun.StopAllCoroutines();

            pickUpController.Drop();

            pickUpController = null;
            gun = null;
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
