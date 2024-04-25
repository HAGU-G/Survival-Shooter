using System;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour
{
    public int maxHp = 100;
    private int currentHp;
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
            {
                OnDie();
            }
            isDead = value;
        }
    }

    public int damage = 20;

    public float syncAnimationSpeed = 4.0f;
    public float maxSpeed = 5.0f;

    public event Action OnDamaged;
    public event Action OnDie;

    private void Awake()
    {
        currentHp = maxHp;
    }
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

    }


    public void Damaged(int damage, Vector3 hitPos, Vector3 hitNormal)
    {
        if (OnDamaged != null)
            OnDamaged();
        AffectOnHealth(-damage);
        Debug.Log(currentHp);
        Debug.Log(IsDead);
    }

    public void AffectOnHealth(int deltaHP)
    {
        currentHp += deltaHP;
        if (currentHp < 0)
        {
            currentHp = 0;
            IsDead = true;
        }
    }
}
