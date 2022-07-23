using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private PlayerHealthBar playerHealth;

    void Start(){
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthBar>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !other.attachedRigidbody){
            playerHealth.RestoreHealth();
            this.gameObject.SetActive(false);
        }
    }
}
