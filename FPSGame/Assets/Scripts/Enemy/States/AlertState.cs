using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : BaseState
{
    public override void Enter()
    {

    }

    public override void Perform()
    {
        enemy.MoveTowardsPlayer();

        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }

        if (!enemy.CanSensePlayer())
        {
            stateMachine.ChangeState(new PatrolState());
        }
    }

    public override void Exit()
    {

    }
}
