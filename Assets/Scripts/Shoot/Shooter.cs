using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Attacker
{
    [HideInInspector]
    public Shot gun;

    public virtual void TargetHit(ref RaycastHit hit) { }

    public override void Attack(Vector3 From, Vector3 direction)
    {
        ShootRaycast(From, direction);
    }

    public virtual void ShootRaycast(Vector3 From, Vector3 direction)
    {
        if (!gun.Fire())
            return;

        // muzzle flash
        if (gun.GunParticleSystem)
            gun.GunParticleSystem.Play();

        if (gun.sound)
        {
            gun.sound.PlayOneShot(gun.sound.clip);
        }

        RaycastHit hit;
        if (Physics.Raycast(From, direction, out hit, gun.range, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            TargetHit(ref hit);
    }
}
