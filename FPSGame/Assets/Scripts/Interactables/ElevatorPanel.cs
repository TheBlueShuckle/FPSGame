using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : Interactable
{
    [SerializeField] GameObject doorFront;
    [SerializeField] GameObject doorBack;
    [SerializeField] Elevator elevator;

    public override void OnLookStart()
    {
        base.OnLookStart();
    }

    protected override void Interact()
    {
        elevator.StartElevator();
    }

    public override void OnLookStop()
    {
        base.OnLookStop();
    }
}
