using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageFromObjects : MonoBehaviour
{
    private EnemyHealthBarHover enemy_health;
    private Health health;
    private BossHealthBarHover boss_health;
    private Rigidbody rig;

    public PhysicsGun gun;
    public float maxDamageOnHit;
    public float necessaryVelocity; // for max damage

    void Start()
    {
        enemy_health = GetComponent<EnemyHealthBarHover>();
        boss_health = GetComponent<BossHealthBarHover>();
        health = GetComponent<Health>();
        rig = GetComponent<Rigidbody>();
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("GroundLayer"))
        {
            if (enemy_health)
            {
                if (other.rigidbody && this.rig && (other.rigidbody.velocity.magnitude > 5f || this.rig.velocity.magnitude > 5f))
                    enemy_health.TakeDamage((other.rigidbody.velocity - this.rig.velocity).magnitude / necessaryVelocity * maxDamageOnHit, true);
                else if (this.rig && other.relativeVelocity.magnitude > 5f)
                    enemy_health.TakeDamage(other.relativeVelocity.magnitude / necessaryVelocity * maxDamageOnHit, true);
            }
            else if(boss_health)
            {
                if (other.rigidbody && this.rig && (other.rigidbody.velocity.magnitude > 5f || this.rig.velocity.magnitude > 5f))
                    boss_health.TakeDamage((other.rigidbody.velocity - this.rig.velocity).magnitude / necessaryVelocity * maxDamageOnHit, true);
                else if (this.rig && other.relativeVelocity.magnitude > 5f)
                    boss_health.TakeDamage(other.relativeVelocity.magnitude / necessaryVelocity * maxDamageOnHit, true);
            }
            else if(health && rig == null)
            {
                if (other.impulse.magnitude > 2f)
                    health.TakeDamage(other.impulse.magnitude / necessaryVelocity * maxDamageOnHit, new RaycastHit());
            }
        }
    }
}
