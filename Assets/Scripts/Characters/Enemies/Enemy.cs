using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : EntityBehaviour
{
    public IObjectPool<Enemy> pool = null;

    private Animator animator;
    private GameObject[] players;
    private NavMeshAgent agent;
    public CapsuleCollider capsuleCollider;

    private bool isSinking;

    protected override void Awake()
    {
        base.Awake();
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
            agent.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            foreach (var c in GetComponents<Collider>())
                c.excludeLayers = 1 << LayerMask.NameToLayer("Player");
        };
    }

    private void OnEnable()
    {
        NavMesh.SamplePosition(Vector3.zero, out NavMeshHit hit, 50f, NavMesh.AllAreas);
        transform.position = hit.position;
        animator.ResetTrigger("Death");
        agent.enabled = true;
        IsDead = false;
        currentHp = maxHp;
        gameObject.layer = LayerMask.NameToLayer("Default");
        foreach (var c in GetComponents<Collider>())
            c.excludeLayers = 1 << LayerMask.NameToLayer("Nothing");
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

        if (players.Length > 0)
        {
            NavMeshPath a = new();
            agent.SetDestination(players[0].transform.position);
        }
        else
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

        if (agent.velocity.magnitude > 0)
        {
            animator.SetFloat("speed", agent.velocity.magnitude / syncAnimationSpeed);
        }


    }

    public void StartSinking()
    {
        isSinking = true;
    }
}
