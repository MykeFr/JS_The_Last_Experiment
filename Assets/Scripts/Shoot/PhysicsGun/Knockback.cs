using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    public PhysicsGun physgun;
    public Transform groundCheck;
    public float groudDistance = 0.2f;
    public LayerMask groundMask;

    private PotionAttacker p_a;
    private EnemyMovementBehaviour m_b;
    private NavMeshAgent nav;
    private Animation currentAnimation;
    private EnemyAnimations animations;
    private Rigidbody rig;

    private bool knocked = false;
    private bool released = true;

    void Start()
    {
        p_a = GetComponent<PotionAttacker>();
        m_b = GetComponent<EnemyMovementBehaviour>();
        nav = GetComponent<NavMeshAgent>();

        currentAnimation = GetComponent<Animation>();
        animations = GetComponent<EnemyAnimations>();

        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (released && !knocked && Physics.CheckSphere(groundCheck.position, groudDistance, groundMask, QueryTriggerInteraction.Ignore))
            EnableControllers();
    }

    public void DisableControllers()
    {
        p_a.enabled = false;
        m_b.enabled = false;
        nav.enabled = false;
    }

    public void EnableControllers()
    {
        rig.velocity = Vector3.zero;
        p_a.enabled = true;
        m_b.enabled = true;
        nav.enabled = true;
    }

    public void Grab(){
        released = false;
        DisableControllers();
    }

    public void Release()
    {
        released = true;
    }

    private void Recover()
    {
        if (knocked)
        {
            knocked = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (released && other.rigidbody && other.transform.gameObject.layer == LayerMask.NameToLayer("GroundLayer") && other.rigidbody.velocity.magnitude > 2)
        {
            DisableControllers();
            currentAnimation.Play(animations.death.name);
            knocked = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (released && other.rigidbody && other.transform.gameObject.layer == LayerMask.NameToLayer("GroundLayer") && other.rigidbody.velocity.magnitude != 0)
        {
            Invoke(nameof(Recover), 1f);
        }
    }
}
