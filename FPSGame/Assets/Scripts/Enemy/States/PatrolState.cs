using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTimer;
    [SerializeField] float waitTimerMax;

    public override void Enter()
    {
    }

    public override void Perform()
    {
        PatrolCycle();

        if (enemy.CanSensePlayer())
        {
            stateMachine.ChangeState(new AlertState());
        }
    }

    public override void Exit()
    {
    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer > waitTimerMax)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                {
                    waypointIndex++;
                }

                else
                {
                    waypointIndex = 0;
                }

                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }
    }
}
