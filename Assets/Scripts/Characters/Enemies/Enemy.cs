using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EntityBehaviour
{
    private Animator animator;
    private GameObject[] players;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        players = GameObject.FindGameObjectsWithTag("Player");

        animator.speed = maxSpeed;
    }
    private void Start()
    {
        OnDie += () =>
        {
            animator.SetTrigger("Death");
        };
    }

    private void Update()
    {
        if (IsDead)
            return;

        if(players.Length > 0)
        {
            NavMeshPath a = new();
            agent.SetDestination(players[0].transform.position);
        }
        else
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        if(agent.velocity.magnitude > 0)
        {
            animator.SetFloat("speed", agent.velocity.magnitude / syncAnimationSpeed);
        }
    }

    public void StartSinking()
    {
    }
}
