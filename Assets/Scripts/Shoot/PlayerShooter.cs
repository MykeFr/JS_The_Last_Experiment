using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : Shooter
{
    public Transform cam;
    public GameObject EnemyImpact;
    public float scaleDistance = 20f;
    public ParticleSystem GroundImpact;
    public float impactLifetime = 2f; // seconds

    public GlobalVariables gv;

    void Update()
    {
        if (!gv.freeLookCamera)
        {
            // allows firing while holding down fire button or a click
            bool isFiring = gun && ((InputHandler.Fire1Down() && !gun.autoFire) || (InputHandler.Fire1Pressed() && gun.autoFire));
            if (isFiring)
                Attack(cam.position + cam.forward, cam.forward);
        }
    }

    public override void TargetHit(ref RaycastHit hit)
    {
        if (gun)
        {
            EnemyHealthBarHover enemy = hit.transform.GetComponent<EnemyHealthBarHover>();
            if (enemy)
            {
                enemy.TakeDamage(gun.damage, false);
                if (EnemyImpact)
                {
                    GameObject impact = Instantiate(EnemyImpact, hit.point + hit.normal, Quaternion.LookRotation(hit.normal));
                    // scale according to distance from camera
                    impact.transform.localScale *= Vector3.Distance(hit.point, transform.position) / scaleDistance;
                    Destroy(impact.gameObject, impactLifetime);
                }
            }
            else
            {
                BossHealthBarHover boss = hit.transform.GetComponent<BossHealthBarHover>();
                if (boss)
                    boss.TakeDamage(gun.damage, false);
                else if (GroundImpact)
                {
                    ParticleSystem impact = Instantiate(GroundImpact, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impact.gameObject, impactLifetime);
                }
            }
        }
    }
}
