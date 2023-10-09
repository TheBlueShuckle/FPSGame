using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{

    public override void Enter()
    {
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            enemy.ChasePlayer();
        }

        else if (enemy.CanSensePlayer())
        {
            stateMachine.ChangeState(new AlertState());
        }

        else
        {
            stateMachine.ChangeState(new PatrolState());
        }
    }

    public override void Exit()
    {
    }
}
