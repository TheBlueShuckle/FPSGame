using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    private PlayerUI playerUI;
    private InputManager inputManager;

    [SerializeField]
    private float rayDistance = 3f;
    [SerializeField]
    private LayerMask mask;

    private void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo; // storing collision info

        playerUI.UpdateText(string.Empty);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance);

        if (Physics.Raycast(ray, out hitInfo, rayDistance, mask) && hitInfo.collider.GetComponent<Interactable>() != null)
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

            playerUI.UpdateText(interactable.promptMessage);

            if (inputManager.onFoot.Interact.triggered)
            {
                interactable.BaseInteract();
            }
        }
    }
}
