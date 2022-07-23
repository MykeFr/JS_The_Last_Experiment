using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardClamped : MonoBehaviour
{
    public Transform cam;
    void Start() {
        if(!cam)
            cam = GameObject.Find("Main Camera")?.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }
}
