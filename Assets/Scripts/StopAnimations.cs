using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StopAnimations : MonoBehaviour
{

    public bool stop = false;
    public GameObject[] enemiesToStop;
    public BossMovementBehaviour boss;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            stop = !stop;
            Stop();
            Restart();
        }

    }

    public void Stop()
    {
        if (stop && enemiesToStop != null)
        {
            for (int i = 0; i < enemiesToStop.Length; i++)
            {
                if (enemiesToStop[i] != null)
                {
                    if (enemiesToStop[i].GetComponent<EnemyMovementBehaviour>() != null)
                    {
                        enemiesToStop[i].GetComponent<EnemyMovementBehaviour>().enabled = false;
                        enemiesToStop[i].GetComponent<PotionAttacker>().enabled = false;
                        enemiesToStop[i].GetComponent<Knockback>().enabled = false;
                        enemiesToStop[i].GetComponent<NavMeshAgent>().enabled = false;
                        enemiesToStop[i].GetComponent<TakeDamageFromObjects>().enabled = false;
                        enemiesToStop[i].GetComponent<EnemyHealthBarHover>().enabled = false;
                    }
                    if (enemiesToStop[i].GetComponent<BossMovementBehaviour>() != null)
                    {
                        enemiesToStop[i].GetComponent<BossMovementBehaviour>().enabled = false;
                        enemiesToStop[i].GetComponent<BossAttacker>().enabled = false;
                        enemiesToStop[i].GetComponent<NavMeshAgent>().enabled = false;
                        boss.animator.SetBool("walk", false);
                    }
                }
            }
        }
    }

    public void Restart()
    {
        if (!stop && enemiesToStop != null)
        {
            for (int i = 0; i < enemiesToStop.Length; i++)
            {
                if (enemiesToStop[i] != null)
                {
                    if (enemiesToStop[i].GetComponent<EnemyMovementBehaviour>() != null)
                    {
                        enemiesToStop[i].GetComponent<EnemyMovementBehaviour>().enabled = true;
                        enemiesToStop[i].GetComponent<PotionAttacker>().enabled = true;
                        enemiesToStop[i].GetComponent<Knockback>().enabled = true;
                        enemiesToStop[i].GetComponent<NavMeshAgent>().enabled = true;
                        enemiesToStop[i].GetComponent<TakeDamageFromObjects>().enabled = true;
                        enemiesToStop[i].GetComponent<EnemyHealthBarHover>().enabled = true;
                    }

                    if (enemiesToStop[i].GetComponent<BossMovementBehaviour>() != null)
                    {
                        enemiesToStop[i].GetComponent<BossMovementBehaviour>().enabled = true;
                        enemiesToStop[i].GetComponent<BossAttacker>().enabled = true;
                        enemiesToStop[i].GetComponent<NavMeshAgent>().enabled = true;
                    }
                }
            }
        }
    }
}
