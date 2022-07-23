using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class EnemyHealthBarHover : HealthBarHover
{
    public GameObject brokenEnemy;
    public float impactForce = 30f;
    public GameObject EnemySquishSound;

    private EnemyAnimations animations;
    private Animation currentAnimation;

    private bool isDead = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        animations = GetComponent<EnemyAnimations>();
        currentAnimation = GetComponent<Animation>();
    }

    public override void Die(RaycastHit hit)
    {
        base.Die(hit);
        if (!isDead)
        {
            isDead = true;
            if (brokenEnemy)
            {
                GameObject deadEnemy = Instantiate(brokenEnemy, this.transform.position, this.transform.rotation);
                deadEnemy.GetComponent<Rigidbody>().velocity = this.gameObject.GetComponent<Rigidbody>().velocity;
                Destroy(deadEnemy, 60f);
            }
        }
        Destroy(this.gameObject);
    }

    public void TakeDamage(float damage, bool fromObjects)
    {
        bool isAlive = base.TakeDamage(damage, new RaycastHit());
        EnemyMovementBehaviour emb = GetComponent<EnemyMovementBehaviour>();
        if (fromObjects && EnemySquishSound)
        {
            GameObject squishSound = Instantiate(EnemySquishSound, this.transform);
            Destroy(squishSound, 3f);
        }

        if (isAlive)
        {
            if (currentAnimation.IsPlaying(animations.takeDamage.name))
                currentAnimation.Rewind();
            if (!currentAnimation.IsPlaying(animations.death.name) && !fromObjects)
                currentAnimation.Play(animations.takeDamage.name);
            if (emb && !fromObjects)
            {
                emb.enabled = false;
                Invoke(nameof(RestoreEMB), animations.takeDamage.length / 2);
            }
        }
    }

    private void RestoreEMB()
    {
        EnemyMovementBehaviour emb = GetComponent<EnemyMovementBehaviour>();
        if (emb)
            emb.enabled = true;
    }
}
