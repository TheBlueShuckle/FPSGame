using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    private bool doorOpen;

    [SerializeField]
    private GameObject door;

    public override void OnLookStart()
    {
        base.OnLookStart();
    }

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
    }

    public override void OnLookStop()
    {
        base.OnLookStop();
    }
}
