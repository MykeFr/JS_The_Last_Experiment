using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 100f;
    public bool startWithMaxHealth = true;
    public float startHealth = 100f;
    public float maxHealth = 100f;

    public virtual void Die(RaycastHit hit){}

    public virtual bool TakeDamage(float damage, RaycastHit hit){
        health -= damage;
        if(!isAlive())
            Die(hit);
        return isAlive();
    }

    public virtual bool isAlive(){
        return health > 0f;
    }   

    public virtual void RestoreHealth(){
        health = maxHealth;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(startWithMaxHealth)
            health = maxHealth;
        else
            health = startHealth;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if(!isAlive())
        //    Die(Vector3.zero);

        if(health > maxHealth)
            health = maxHealth;
    }
}