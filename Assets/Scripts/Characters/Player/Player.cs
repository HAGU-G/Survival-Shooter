using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : EntityBehaviour
{
    private Rigidbody rb;
    private PlayerInput input;
    private Animator animator;

    private LineRenderer bulletLine;
    public ParticleSystem gunParticles;

    private Vector3 inputVelocity;

    public Slider slider;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        bulletLine = GetComponentInChildren<LineRenderer>();
        bulletLine.enabled = false;

        OnDie += () =>
        {
            animator.SetTrigger("Death");
        };
    }
    private void Update()
    {
        // TODO 데미지 받을 때마다 갱신되도록 변경
        slider.value = (float)currentHp / maxHp;

        if (IsDead)
            return;

        //이동
        inputVelocity = Vector3.forward * input.Vertical * maxSpeed
            + Vector3.right * input.Horizontal * maxSpeed;
        if (inputVelocity.magnitude > maxSpeed)
            inputVelocity = inputVelocity.normalized * maxSpeed;
        rb.velocity = inputVelocity + Vector3.up * rb.velocity.y;
        animator.SetFloat("speed", ((input.Vertical != 0f) ? input.Vertical : input.Horizontal) * maxSpeed / syncAnimationSpeed);

        //회전
        if (!GameManager.Instance.IsPaused)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer("Floor")))
                transform.LookAt(hitInfo.point);
        }

        //공격
        attackTimer += Time.deltaTime;
        if (input.Attack && attackTimer >= attackInterval)
        {
            attackTimer = 0f;

            Physics.Raycast(new(gunParticles.transform.position, gunParticles.transform.forward),
                out RaycastHit shotHitInfo,
                float.MaxValue,
                ~(1 << LayerMask.NameToLayer("Ignore Raycast")),
                QueryTriggerInteraction.Ignore);
            StartCoroutine(CoShotEffect(shotHitInfo.point));
            var entity = shotHitInfo.collider.GetComponent<EntityBehaviour>();
            if (entity != null)
            {
                entity.Damaged(damage, shotHitInfo.point, shotHitInfo.normal);
            }
        }


    }

    private IEnumerator CoShotEffect(Vector3 hitPos)
    {
        bulletLine.enabled = true;
        bulletLine.SetPosition(0, gunParticles.transform.position);
        bulletLine.SetPosition(1, hitPos);
        gunParticles.Play();


        yield return new WaitForSeconds(0.03f);

        bulletLine.enabled = false;
    }

    private void RestartLevel()
    {
        GameManager.Instance.GameOver();
    }
}
