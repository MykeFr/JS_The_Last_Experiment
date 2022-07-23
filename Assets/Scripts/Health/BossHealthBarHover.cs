using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class BossHealthBarHover : HealthBarHover
{
    public GameObject brokenEnemy;
    public float impactForce = 30f;

    [SerializeField] private Animator animator;
    private bool hasBelowHalfHP = false;
    private bool sufferingFromHalfHP = false;
    [SerializeField] AudioSource audioSource;
    private bool played = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void Die(RaycastHit hit)
    {
        base.Die(hit);
     
        BossMovementBehaviour bmb = GetComponent<BossMovementBehaviour>();
        if (bmb)
            bmb.enabled = false;

        BossAttacker ba = GetComponent<BossAttacker>();
        if (ba)
            ba.enabled = false;

        NavMeshAgent nm = GetComponent<NavMeshAgent>();
        if (nm)
            nm.enabled = false;

        animator.SetTrigger("death");

        if (brokenEnemy)
        {
            GameObject deadEnemy = Instantiate(brokenEnemy, this.transform.position, this.transform.rotation);
            Destroy(deadEnemy, 60f);
        }
        if (!played)
        {
            LabAudioText.Instance.labAudioText(4);
            played = true;
        }

        Destroy(this.gameObject, 10);
    }

    public void TakeDamage(float damage, bool fromObjects)
    {
        if(!sufferingFromHalfHP && (!hasBelowHalfHP || (hasBelowHalfHP && fromObjects)))
            base.TakeDamage(damage, new RaycastHit());

        if(health <= maxHealth*0.5f && !hasBelowHalfHP)
        {
            audioSource.Play();
            Invoke("PutOutline", 2);
            animator.SetTrigger("halfHP");
            hasBelowHalfHP = true;
            StartCoroutine(HalfLifeAudio());
            sufferingFromHalfHP = true;
            BossMovementBehaviour bmb = GetComponent<BossMovementBehaviour>();
            if (bmb)
                bmb.enabled = false;

            BossAttacker ba = GetComponent<BossAttacker>();
            if (ba)
            {
                ba.timeBetweenAttacks = 3f;
                ba.enabled = false;
            }
                

            NavMeshAgent nm = GetComponent<NavMeshAgent>();
            if (nm)
                nm.enabled = false;


            Invoke("ResetScripts", 4);
        }
    }

    IEnumerator HalfLifeAudio()
    {
        yield return new WaitForSeconds(5f);
        LabAudioText.Instance.labAudioText(3);
    }

    public void ResetScripts()
    {
        BossMovementBehaviour bmb = GetComponent<BossMovementBehaviour>();
        if (bmb)
            bmb.enabled = true;

        BossAttacker ba = GetComponent<BossAttacker>();
        if (ba)
            ba.enabled = true;

        NavMeshAgent nm = GetComponent<NavMeshAgent>();
        if (nm)
            nm.enabled = true;

        sufferingFromHalfHP = false;
    }

    public void PutOutline()
    {
        ObjectOutline oo = GetComponent<ObjectOutline>();
        if (oo)
            oo.enabled = true;
    }
}
