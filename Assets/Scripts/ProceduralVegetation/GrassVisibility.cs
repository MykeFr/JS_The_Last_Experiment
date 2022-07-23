using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassVisibility : MonoBehaviour
{
    public float range = 20f;
    void Start(){
        SphereCollider collider = this.gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = range;
    }

    void OnTriggerEnter(Collider other){
        if(other.name == "Grass")
            other.transform.GetChild(0).gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider other){
        if(other.name == "Grass")
            other.transform.GetChild(0).gameObject.SetActive(false);
    }
}
