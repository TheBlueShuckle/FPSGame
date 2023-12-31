using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Interactable
{
    [SerializeField]
    private GameObject player;

    public Transform teleportTarget;
    private CharacterController characterController;

    public override void OnLookStart()
    {
        base.OnLookStart();
    }

    protected override void Interact()
    {
        characterController = player.gameObject.GetComponent<CharacterController>();

        characterController.enabled = false;
        player.gameObject.transform.position = teleportTarget.position;
        characterController.enabled = true;

        Debug.Log("teleported to: " + teleportTarget.position);
    }

    public override void OnLookStop()
    {
        base.OnLookStop();
    }
}
