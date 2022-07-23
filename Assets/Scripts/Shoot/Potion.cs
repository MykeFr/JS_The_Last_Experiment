using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public float damage;
    public float radius;
    public ParticleSystem explosion;
    public LayerMask whatIsPlayer;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] GameObject exploded;
    private float rotationSpeed = 200f;
    private Vector3 forward;
    private PlayerHealthBar player_health;

    void Start()
    {
        forward = this.transform.forward;
        player_health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthBar>();
    }

    void Update()
    {
        this.transform.Rotate(forward * rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (player_health && Physics.CheckSphere(collision.transform.position, radius, whatIsPlayer))
            player_health.TakeDamage(damage, new RaycastHit());

        if (collision.gameObject.layer != enemyMask)
        {
            Instantiate(explosion, this.transform.position, explosion.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
