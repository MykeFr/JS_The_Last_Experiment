using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 200f;
    public Vector3 forward;

    void Update()
    {
        this.transform.Rotate(forward * rotationSpeed * Time.deltaTime);
    }
}