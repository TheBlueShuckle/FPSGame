using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTimer;
    [SerializeField] float waitTimerMax;

    private Path path;

    public override void Enter()
    {
        path = GameObject.FindGameObjectWithTag("Path").GetComponent<Path>();
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
        if (enemy.Agent.remainingDistance < 1f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer > waitTimerMax)
            {
                if (waypointIndex < path.waypoints.Count - 1)
                {
                    waypointIndex++;
                }

                else
                {
                    waypointIndex = 0;
                }

                enemy.Agent.SetDestination(path.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }
    }
}
