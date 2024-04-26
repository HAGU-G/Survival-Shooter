using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : EntityBehaviour
{
    public IObjectPool<Enemy> pool = null;

    private Animator animator;
    private Player target;
    private NavMeshAgent agent;
    public CapsuleCollider capsuleCollider;

    private bool isSinking;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").GetComponent<Player>();

        animator.speed = maxSpeed;

    }

    private void Start()
    {
        OnDie += () =>
        {
            animator.SetTrigger("Death");
            agent.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            foreach (var c in GetComponents<Collider>())
                c.excludeLayers = 1 << LayerMask.NameToLayer("Player");

            StopCoroutine(CoPathFind());
        };


    }

    private void OnEnable()
    {
        animator.ResetTrigger("Death");
        IsDead = false;

        agent.enabled = true;
        NavMesh.SamplePosition(Vector3.zero, out NavMeshHit hit, 50f, NavMesh.AllAreas);
        
        transform.position = hit.position;
        currentHp = maxHp;

        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (var c in GetComponents<Collider>())
            c.excludeLayers = 1 << LayerMask.NameToLayer("Nothing");

        StartCoroutine(CoPathFind());
    }

    private void Update()
    {
        if (isSinking)
        {
            transform.position += Vector3.down * Time.deltaTime;
            if (capsuleCollider.bounds.max.y < 0)
            {
                isSinking = false;
                pool.Release(this);
            }
        }

        if (IsDead)
            return;

        if (agent.velocity.magnitude > 0)
        {
            animator.SetFloat("speed", agent.velocity.magnitude / syncAnimationSpeed);
        }

        attackTimer += Time.deltaTime;
    }

    private IEnumerator CoPathFind()
    {
        while (!IsDead)
        {
            if (target != null && !target.IsDead)
            {
                agent.SetDestination(target.transform.position);
            }
            else
            {
                target = GameObject.FindWithTag("Player").GetComponent<Player>();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (attackTimer < attackInterval)
            return;

        attackTimer = 0f;

        var player = other.GetComponent<Player>();
        if (player == target && !target.IsDead)
        {
            player.Damaged(damage, other.ClosestPoint(transform.position), transform.position - other.transform.position);
            agent.ResetPath();
        }
    }

    public void StartSinking()
    {
        isSinking = true;
    }
}
