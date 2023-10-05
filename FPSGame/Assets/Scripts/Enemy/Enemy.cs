using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    public NavMeshAgent Agent { get => agent; }
    public Path path;

    [SerializeField] // for debugging
    private string currentState;
    private GameObject player;

    [Header("Speeds")]
    [SerializeField] float patrolSpeed;
    [SerializeField] float chaseSpeed;

    [Header("Sight Values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();

        ChangeSpeed();
    }

    public bool CanSeePlayer()
    {
        if (player == null)
        {
            return false;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
        {
            Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

            if (angleToPlayer >= -fieldOfView && 
                angleToPlayer <= fieldOfView && 
                LineOfSightIsClear(targetDirection))
            {
                return true;
            }
        }

        return false;
    }

    private bool LineOfSightIsClear(Vector3 targetDirection)
    {
        Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, sightDistance))
        {
            if (hitInfo.transform.gameObject == player)
            {
                Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                return true;
            }
        }

        return false;
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    private void ChangeSpeed()
    {
        if (stateMachine.activeState.GetType() == typeof(AttackState))
        {
            agent.speed = chaseSpeed;
        }

        if (stateMachine.activeState.GetType() == typeof(PatrolState))
        {
            agent.speed = patrolSpeed;
        }
    }
}
