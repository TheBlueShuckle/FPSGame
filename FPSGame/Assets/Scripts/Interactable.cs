using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;

    [SerializeField]
    public string promptMessage;

    // Will be called from player
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // Template function to be overridden by subclasses
    }
}
