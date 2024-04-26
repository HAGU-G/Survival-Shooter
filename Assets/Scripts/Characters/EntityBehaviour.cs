using System;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public int maxHp = 100;
    public ParticleSystem hitParticles;
    protected int currentHp;
    private bool isDead = false;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
        set
        {
            if (!isDead && value && OnDie != null)
            {
                OnDie();
            }
            isDead = value;
        }
    }

    public int damage = 20;
    protected float attackTimer = 0f;
    public float attackInterval = 0.16f;

    public float syncAnimationSpeed = 4.0f;
    public float maxSpeed = 5.0f;

    public event Action OnDie;


    protected virtual void Awake()
    {
        currentHp = maxHp;
    }

    public void Damaged(int damage, Vector3 hitPos, Vector3 hitNormal)
    {
        AffectOnHealth(-damage);
        hitParticles.transform.position = hitPos;
        hitParticles.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitParticles.Play();
    }

    public void AffectOnHealth(int deltaHP)
    {
        currentHp += deltaHP;
        if (currentHp <= 0)
        {
            currentHp = 0;
            IsDead = true;
        }
    }
}
