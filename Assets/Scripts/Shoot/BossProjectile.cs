using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float damage = 2.5f;
    public LayerMask whatIsPlayer;
    [SerializeField] GameObject exploded;
    private PlayerHealthBar player_health;
    public ParticleSystem explosion;
    [SerializeField] GameObject nada;

    void Start()
    {
        player_health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthBar>();
    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player") && player_health)
        {
            player_health.TakeDamage(damage, new RaycastHit());
        }
        Destroy(this.gameObject);
        GameObject n = Instantiate(nada, this.transform.position, explosion.transform.rotation);
        Instantiate(explosion, this.transform.position, explosion.transform.rotation);
        Destroy(n, 2);
        
        
    }
}
