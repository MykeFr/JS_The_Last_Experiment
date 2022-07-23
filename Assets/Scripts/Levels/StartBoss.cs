using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoss : MonoBehaviour
{

    private bool first = true;
    [SerializeField] private  GameObject boss;
    [SerializeField] private DoorOpen doorOpen;
    [SerializeField] private StopAnimations stopAnimations;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag.Equals("Player") && first)
        {
            if(boss != null)
            {
                first = false;
                boss.GetComponent<BossMovementBehaviour>().enabled = true;
                boss.GetComponent<BossAttacker>().enabled = true;
                doorOpen.Close();
                doorOpen.allowPassage = false;
                LabAudioText.Instance.labAudioText(2);
            }
            stopAnimations.enemiesToStop = new GameObject[1];
            stopAnimations.enemiesToStop[0] = boss;
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag.Equals("Player") && first)
        {
            if (boss != null)
            {
                first = false;
                boss.GetComponent<BossMovementBehaviour>().enabled = false;
                boss.GetComponent<BossAttacker>().enabled = false;
                doorOpen.allowPassage = true;
            }
            stopAnimations.enemiesToStop = null;
        }
    }
}
