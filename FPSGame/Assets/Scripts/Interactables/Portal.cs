using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Interactable
{
    [SerializeField]
    private GameObject player;

    public Transform teleportTarget;
    private CharacterController characterController;

    protected override void Interact()
    {
        characterController = player.gameObject.GetComponent<CharacterController>();

        characterController.enabled = false;
        player.gameObject.transform.position = teleportTarget.position;
        characterController.enabled = true;

        Debug.Log("teleported to: " + teleportTarget.position);
    }
}
