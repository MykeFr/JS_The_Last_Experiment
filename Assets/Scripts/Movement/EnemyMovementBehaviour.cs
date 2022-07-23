
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //States
    public float sightRange;
    public bool playerInSightRange;

    private EnemyAnimations animations;
    private Animation currentAnimation;

    private void Awake()
    {
        if (!player)
            player = GameObject.Find("Player").transform;
        if (!agent)
            agent = GetComponent<NavMeshAgent>();

        animations = GetComponent<EnemyAnimations>();
        currentAnimation = GetComponent<Animation>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        PotionAttacker ea = GetComponent<PotionAttacker>();
        if (ea != null)
        {
            if (!playerInSightRange) Patroling();
            else if (!ea.IsPlayerInAttackRange())
                ChasePlayer();
        }

        if (agent.isOnNavMesh && !agent.isStopped)
            currentAnimation.Play(animations.run.name);
        else if (!currentAnimation.IsPlaying(animations.attack.name) && !currentAnimation.IsPlaying(animations.takeDamage.name))
            currentAnimation.Play(animations.idle.name);

        if (ea != null)
            // stop if reached minimum distance from player
            if (Vector3.Distance(transform.position, player.position) <= ea.attackRange)
            {
                agent.isStopped = true;
                agent.SetDestination(transform.position);
            }
    }

    private void Patroling()
    {
        if (agent.isOnNavMesh)
            agent.isStopped = false;

        currentAnimation.Play(animations.run.name);

        // get new location
        if (!walkPointSet) SearchWalkPoint();

        // go to location
        if (walkPointSet && agent.isOnNavMesh)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        currentAnimation.Play(animations.run.name);
        agent.SetDestination(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
    }
}
