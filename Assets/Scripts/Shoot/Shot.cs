using UnityEngine.Audio;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public float damage = 10f;
    public float fireRate = 15f; // per second
    public bool autoFire = false;
    public float range = 100f;
    public ParticleSystem GunParticleSystem;
    public AudioSource sound;

    private float nextTimeToFire = 0f;

    public bool Fire()
    {
        if (Time.time < nextTimeToFire)
            return false;

        nextTimeToFire = Time.time + 1f / fireRate;

        return true;
    }
}
