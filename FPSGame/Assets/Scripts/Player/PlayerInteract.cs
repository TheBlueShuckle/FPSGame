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

    private Interactable currentTarget;

    private void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        HandleRaycast();

        if (currentTarget != null)
        {
            playerUI.UpdatePromptMessage(currentTarget.promptMessage);

            if (inputManager.onFoot.Interact.triggered)
            {
                currentTarget.OnInteract();
            }
        }

        else
        {
            playerUI.UpdatePromptMessage(null);
        }
    }

    private void HandleRaycast()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out hitInfo, rayDistance, mask))
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();

            if(interactable != null)
            {
                // if you're still looking at the same interactable
                if (interactable == currentTarget)
                {
                    return;
                }
                // if the new interactable you're looking at overlaps with the previous interactable
                else if (currentTarget != null)
                {
                    currentTarget.OnLookStop();
                    currentTarget = interactable;
                    currentTarget.OnLookStart();
                }
                // if the interactable you're looking at is new
                else
                {
                    currentTarget = interactable;
                    currentTarget.OnLookStart();
                }
            }
            // if interactable is null
            else if (currentTarget != null)
            {
                currentTarget.OnLookStop();
                currentTarget = null;
            }
        }
        // if not looking at anything
        else if (currentTarget != null)
        {
            currentTarget.OnLookStop();
            currentTarget = null;
        }
    }
}
