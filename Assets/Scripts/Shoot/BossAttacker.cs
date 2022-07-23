using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacker : Attacker
{
    public Transform player;
    public LayerMask whatIsPlayer;

    public float timeBetweenAttacks;
    public float attackRange;

    private bool alreadyAttacked;
    [HideInInspector]

    private EnemyMovementBehaviour enemyMovement;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform hand;
    [SerializeField] private GameObject projectile;
    private int numOfAttacks = 0;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovementBehaviour>();
    }

    void Update()
    {
        if (IsPlayerInAttackRange())
            AttackPlayer();
    }

    private void AttackPlayer()
    {
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        animator.SetBool("walk", false);
        

        if (!alreadyAttacked)
        {
            animator.SetTrigger("attack");
            animator.SetTrigger("attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            Attack(hand.position, player.position - hand.position);
        }
    }

    public override void Attack(Vector3 From, Vector3 direction)
    {
        numOfAttacks = 0;
        Invoke("DelayAttacks", 0.4f);
    }

    public void DelayAttacks()
    {
        Vector3 direction = player.position +  new Vector3(0,0.2f,0) - hand.position;
        GameObject proj = Instantiate(projectile, hand.position + transform.forward, Quaternion.identity);
        proj.transform.LookAt(player);
        proj.GetComponent<Rigidbody>().AddForce(direction * 300);
        numOfAttacks++;
        if(numOfAttacks < 5)
        {
            Invoke("DelayAttacks", 0.1f);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public bool IsPlayerInAttackRange(){
        return Physics.CheckSphere(transform.position, attackRange, whatIsPlayer, QueryTriggerInteraction.Ignore);
    }
}

