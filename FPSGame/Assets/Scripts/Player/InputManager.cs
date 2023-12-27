using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerControls.OnFootActions onFoot;
    private PlayerControls.InInventoryActions inInventory;

    [SerializeField] private InventoryManager inventoryManager;

    private Gun gun;
    private PickUpController pickUpController;

    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerInteract interact;

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

        playerControls = new PlayerControls();
        onFoot = playerControls.OnFoot;
        inInventory = playerControls.InInventory;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        interact = GetComponent<PlayerInteract>();

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.SprintStart.performed += ctx => motor.StartSprint(onFoot.Movement.ReadValue<Vector2>());
        onFoot.SprintStop.performed += ctx => motor.StopSprint();

        onFoot.Shoot.started += ctx => StartFiring();
        onFoot.Shoot.canceled += ctx => StopFiring();
        onFoot.Reload.performed += ctx => Reload();

        onFoot.Drop.performed += ctx => DropGun();

        onFoot.Interact.performed += ctx => interact.Interact();

        onFoot.OpenInventory.performed += ctx => inventoryManager.OpenInventory();

        inInventory.Escape.performed += ctx => inventoryManager.CloseInventory();
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

        if (gun != null)
        {
            gun.IsMoving(onFoot.Movement.ReadValue<Vector2>());
            gun.GetComponent<WeaponSway>().Sway(onFoot.Look.ReadValue<Vector2>());
        }

        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());

        motor.UpdateSprinting(onFoot.Movement.ReadValue<Vector2>());
    }

    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    #region - Enable / Disable -

    private void OnEnable()
    {
        playerControls.Enable();

        onFoot.OpenInventory.performed += SwitchToInMenu;
        inInventory.Escape.performed += SwitchToOnFoot;
    }

    private void OnDisable()
    {
        playerControls.Disable();

        onFoot.OpenInventory.performed -= SwitchToInMenu;
        inInventory.Escape.performed -= SwitchToOnFoot;
    }

    private void SwitchToInMenu(InputAction.CallbackContext ctx)
    {
        inInventory.Enable();
        onFoot.Disable();
    }

    private void SwitchToOnFoot(InputAction.CallbackContext ctx)
    {
        inInventory.Disable();
        onFoot.Enable();
    }

    #endregion

    void StartFiring()
    {
        if (gun != null)
        {
            fireCoroutine = StartCoroutine(gun.Fire());
        }
    }

    void StopFiring()
    {
        if (gun != null)
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
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
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }

            pickUpController.Drop();

            pickUpController = null;
            gun = null;
        }
    }
}
