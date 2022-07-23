using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionAttacker : Attacker
{
    public Transform player;
    public LayerMask whatIsPlayer;
    public float angleOfAttack = 45f;

    public float timeBetweenAttacks;
    public float attackRange;

    private bool alreadyAttacked;
    [HideInInspector]

    private EnemyMovementBehaviour enemyMovement;
    private Animation currentAnimation;
    private EnemyAnimations animations;

    [SerializeField] private Transform hand;
    [SerializeField] private GameObject potionPrefab;
    [SerializeField] private int potionDamage;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovementBehaviour>();
        currentAnimation = GetComponent<Animation>();
        animations = GetComponent<EnemyAnimations>();
    }

    void Update()
    {
        if (IsPlayerInAttackRange() && !currentAnimation.IsPlaying(animations.takeDamage.name))
            AttackPlayer();
    }

    private void AttackPlayer()
    {
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        if (!alreadyAttacked)
        {
            currentAnimation.Play(animations.attack.name);
            Attack(hand.position, player.position - hand.position);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public override void Attack(Vector3 From, Vector3 direction)
    {
        GameObject potion = Instantiate(potionPrefab, From + transform.forward, Quaternion.identity);
        ThrowPotion(potion);
        //potionAttack.GetComponent<Rigidbody>().AddForce(new Vector3(direction.x, direction.y + 20f, direction.z) * 25, ForceMode.VelocityChange);
    }

    private void ThrowPotion(GameObject potion)
    {
        var rigid = potion.GetComponent<Rigidbody>();

        Vector3 p = player.position;

        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = angleOfAttack * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(potion.transform.position.x, 0, potion.transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = potion.transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        // Fire!
        if (finalVelocity != new Vector3(float.NaN, float.NaN, float.NaN))
            rigid.velocity = finalVelocity;

        // Alternative way:
        // rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public bool IsPlayerInAttackRange()
    {
        return Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
    }
}

// explanation on throwing logic in https://forum.unity.com/threads/how-to-calculate-force-needed-to-jump-towards-target-point.372288/
