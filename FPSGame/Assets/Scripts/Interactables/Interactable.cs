using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;

    [SerializeField]
    public string promptMessage;

    public virtual void OnLookStart()
    {

    }

    public void OnInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().onInteract.Invoke();
        }
        Interact();
    }

    public virtual void OnLookStop()
    {

    }

    protected virtual void Interact()
    {
        // Template function to be overridden by subclasses
    }
}
