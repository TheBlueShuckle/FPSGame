using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{
    private bool doorOpen;

    [SerializeField]
    private GameObject door;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
    }
}
