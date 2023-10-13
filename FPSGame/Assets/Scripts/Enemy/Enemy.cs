using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float senseDistanceRunning = 40f;
    public float senseDistanceWalking = 30f;
    private float senseDistance;


    [Header("Attack")]
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float damageCooldownTotalSeconds = 1;
    private float damageCooldownSeconds = 0;

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

    public bool CanSensePlayer()
    {
        if (player == null)
        {
            return false;
        }

        if (player.GetComponent<PlayerMotor>().IsSprinting)
        {
            senseDistance = senseDistanceRunning;
        }

        else if (!player.GetComponent<PlayerMotor>().IsCrouched)
        {
            senseDistance = senseDistanceWalking;
        }

        else
        {
            return false;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < senseDistance)
        {
            Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
            return LineOfSightIsClear(targetDirection, senseDistance);
        }

        return false;
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
                LineOfSightIsClear(targetDirection, sightDistance))
            {
                return true;
            }
        }

        return false;
    }

    private bool LineOfSightIsClear(Vector3 targetDirection, float distance)
    {
        Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance))
        {
            if (hitInfo.transform.gameObject == player)
            {
                Debug.DrawRay(ray.origin, ray.direction * distance);
                return true;
            }
        }

        return false;
    }

    public void MoveTowardsPlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    public void ChasePlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < meleeRange)
        {
            agent.SetDestination(transform.position);
            Attack();
        }

        else
        {
            damageCooldownSeconds = damageCooldownTotalSeconds;
            agent.SetDestination(player.transform.position);
        }
    }

    private void Attack()
    {
        damageCooldownSeconds += Time.deltaTime;

        if (damageCooldownSeconds > damageCooldownTotalSeconds)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            damageCooldownSeconds = 0;
        }
    }

    private void ChangeSpeed()
    {
        if (stateMachine.activeState.GetType() == typeof(AttackState))
        {
            agent.speed = chaseSpeed;
        }

        else
        {
            agent.speed = patrolSpeed;
        }
    }
}
