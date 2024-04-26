using System;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public ParticleSystem hitParticles;
    protected AudioSource audioSource;
    public AudioClip audioDeath;
    public AudioClip audioHurt;

    public int maxHp = 100;
    protected int currentHp;
    private bool isDead = false;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
        private set
        {
            if (!isDead && value && OnDie != null)
                OnDie();
            isDead = value;
        }
    }

    public int damage = 20;
    protected float attackTimer = 0f;
    public float attackInterval = 0.16f;

    public float syncAnimationSpeed = 4.0f;
    public float maxSpeed = 5.0f;

    public event Action OnDie;
    public event Action OnDamage;


    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        currentHp = maxHp;
    }

    protected virtual void OnEnable()
    {
        IsDead = false;
    }

    public void Damaged(int damage, Vector3 hitPos, Vector3 hitNormal)
    {
        AffectOnHealth(-damage);
        if (hitParticles != null)
        {
            hitParticles.transform.position = hitPos;
            hitParticles.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitParticles.Play();
        }
        if (OnDamage != null)
            OnDamage();
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
