using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemies : MonoBehaviour
{

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private StopAnimations stopAnimations;


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag.Equals("Player"))
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if(enemies[i] != null)
                {
                    enemies[i].GetComponent<EnemyMovementBehaviour>().enabled = true;
                    enemies[i].GetComponent<PotionAttacker>().enabled = true;
                    enemies[i].GetComponent<Knockback>().enabled = true;
                    enemies[i].GetComponent<TakeDamageFromObjects>().enabled = true;
                    enemies[i].GetComponent<EnemyHealthBarHover>().enabled = true;
                }
                stopAnimations.enemiesToStop = enemies;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if(enemies[i] != null)
                {
                    enemies[i].GetComponent<EnemyMovementBehaviour>().enabled = false;
                    enemies[i].GetComponent<PotionAttacker>().enabled = false;
                    enemies[i].GetComponent<Knockback>().enabled = false;
                    enemies[i].GetComponent<TakeDamageFromObjects>().enabled = false;
                    enemies[i].GetComponent<EnemyHealthBarHover>().enabled = false;
                }
                stopAnimations.enemiesToStop = null;
            }
        }
    }
}
