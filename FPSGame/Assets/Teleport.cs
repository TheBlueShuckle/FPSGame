using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    private CharacterController characterController;

    private void OnTriggerEnter(Collider other)
    {
        characterController = other.gameObject.GetComponent<CharacterController>();

        characterController.enabled = false;
        other.gameObject.transform.position = teleportTarget.position;
        characterController.enabled = true;

        Debug.Log("teleported to: " + teleportTarget.position);
    }
}
