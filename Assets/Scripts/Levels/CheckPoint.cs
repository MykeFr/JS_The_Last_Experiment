using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameMaster gm;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag.Equals("Player"))
        {
            gm.check = true;
            gm.restartPos = this.transform.position;
            gm.restartRot = this.transform.rotation;
        }
    }
}
