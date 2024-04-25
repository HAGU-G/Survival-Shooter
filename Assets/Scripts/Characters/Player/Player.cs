using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : EntityBehaviour
{
    private Rigidbody rb;
    private PlayerInput input;
    private Animator animator;

    private LineRenderer shotEffect;
    public GameObject gunBarrelEnd;

    private Vector3 inputVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        shotEffect = GetComponentInChildren<LineRenderer>();
        shotEffect.enabled = false;
    }
    private void Update()
    {
        //ÀÌµ¿
        inputVelocity = Vector3.forward * input.Vertical * maxSpeed
            + Vector3.right * input.Horizontal * maxSpeed;
        if (inputVelocity.magnitude > maxSpeed)
            inputVelocity = inputVelocity.normalized * maxSpeed;
        rb.velocity = inputVelocity + Vector3.up * rb.velocity.y;
        animator.SetFloat("speed", ((input.Vertical != 0f) ? input.Vertical : input.Horizontal) * maxSpeed / syncAnimationSpeed);

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer("Floor")))
            transform.LookAt(hitInfo.point);


        if (input.Attack)
        {
            shotEffect.enabled = true;
            Physics.Raycast(new(gunBarrelEnd.transform.position, gunBarrelEnd.transform.forward), out RaycastHit shotHitInfo, float.MaxValue);
            shotEffect.SetPosition(0,gunBarrelEnd.transform.position);
            shotEffect.SetPosition(1,shotHitInfo.point);
            var entity = shotHitInfo.collider.GetComponent<EntityBehaviour>();
            if(entity != null)
            {
                entity.Damaged(damage, shotHitInfo.point, shotHitInfo.normal);
            }
        }
    }
}
