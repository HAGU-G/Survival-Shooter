using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : EntityBehaviour
{
    private Rigidbody rb;
    private PlayerInput input;
    private Animator animator;

    public LineRenderer bulletLine;
    public ParticleSystem gunParticles;
    public CinemachineVirtualCamera vCamera; 

    public Slider hpSlider;
    public Slider skillSlider;
    public AudioSource audioAttack;
    public Image skillSliderImage;
    public GameObject attackSphere;

    private int skillPoint;
    public int maxSkillPoint = 10;
    private int SkillPoint
    {
        get => skillPoint;
        set
        {
            skillPoint = Mathf.Clamp(skillPoint = value, 0, maxSkillPoint);
            skillSlider.value = skillPoint;
            if (skillPoint == maxSkillPoint)
                skillSliderImage.color = Color.red;
            else
                skillSliderImage.color = Color.gray;
          }
    }

    public AudioClip audioGunShot;
    public AudioClip countDown;
    public AudioClip countDownLast;
    public AudioClip countDone;
    public Light shotLight;


    public Animator hitEffect;

    private Vector3 inputVelocity;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        OnDie += () =>
        {
            animator.SetTrigger("Death");
            audioSource.PlayOneShot(audioDeath);
            GameManager.Instance.Gameover();
            hitEffect.SetTrigger("Gameover");
        };
        OnDamage += () =>
        {
            if (!IsDead)
            {
                audioSource.PlayOneShot(audioHurt);
                hitEffect.SetTrigger("Hit");
                SkillPoint++;
            }
        };
    }

    private void Start()
    {
        attackSphere.SetActive(false);
        bulletLine.enabled = false;
        shotLight.enabled = false;
    }

    private void Update()
    {
        // TODO 데미지 받을 때마다 갱신되도록 변경
        hpSlider.value = (float)currentHp / maxHp;

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

            if (Physics.Raycast(new(gunParticles.transform.position, gunParticles.transform.forward),
                out RaycastHit shotHitInfo,
                float.MaxValue,
                ~(1 << LayerMask.NameToLayer("Ignore Raycast")),
                QueryTriggerInteraction.Ignore))
            {
                StartCoroutine(CoShotEffect(shotHitInfo.point));
                var entity = shotHitInfo.collider.GetComponent<EntityBehaviour>();
                if (entity != null)
                {
                    if (entity.Damaged(damage, shotHitInfo.point, shotHitInfo.normal))
                        SkillPoint++;
                }
                audioAttack.PlayOneShot(audioGunShot);
            }
        }

        //스킬
        if (input.Skill && SkillPoint == maxSkillPoint && !attackSphere.activeSelf)
            StartCoroutine(UseSkill());

    }

    private IEnumerator CoShotEffect(Vector3 hitPos)
    {
        bulletLine.enabled = true;
        shotLight.enabled = true;
        bulletLine.SetPosition(0, gunParticles.transform.position);
        bulletLine.SetPosition(1, hitPos);
        gunParticles.Play();


        yield return new WaitForSeconds(0.03f);

        bulletLine.enabled = false;
        shotLight.enabled = false;
    }

    private void RestartLevel()
    {
        GameManager.Instance.Restart();
    }

    private IEnumerator UseSkill()
    {
        SkillPoint = 0;
        audioAttack.PlayOneShot(countDown);
        vCamera.m_Lens.FieldOfView = 70;

        yield return new WaitForSeconds(1f);
        audioAttack.PlayOneShot(countDown);
        vCamera.m_Lens.FieldOfView = 85;
        attackSphere.SetActive(true);

        yield return new WaitForSeconds(1f);
        audioAttack.PlayOneShot(countDownLast);
        vCamera.m_Lens.FieldOfView = 100;


        yield return new WaitForSeconds(1f);
        audioAttack.PlayOneShot(countDone);
        vCamera.m_Lens.FieldOfView = 60;
        attackSphere.SetActive(false);
    }
}
