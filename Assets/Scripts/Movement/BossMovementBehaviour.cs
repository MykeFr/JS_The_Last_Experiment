
using UnityEngine;
using UnityEngine.AI;

public class BossMovementBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Animator animator;

    private void Awake()
    {
        if (!player)
            player = GameObject.Find("Player").transform;
        if (!agent)
            agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {


        BossAttacker ea = GetComponent<BossAttacker>();

        //EnemyShooter es = GetComponent<EnemyShooter>();
        if (!ea.IsPlayerInAttackRange())
            ChasePlayer();
        /*if (!agent.isStopped)
            currentAnimation.Play(animations.run.name);
        else if(!currentAnimation.IsPlaying(animations.attack.name) && !currentAnimation.IsPlaying(animations.takeDamage.name))
            currentAnimation.Play(animations.idle.name);*/

        // stop if reached minimum distance from player
        if (Vector3.Distance(transform.position, player.position) <= ea.attackRange)
        {
            agent.isStopped = true;
            agent.SetDestination(transform.position);
        }
    }

    private void ChasePlayer()
    {
        animator.SetBool("walk", true);
        agent.isStopped = false;
        //currentAnimation.Play(animations.run.name);
        agent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }
}
