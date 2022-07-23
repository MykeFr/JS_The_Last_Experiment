using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    public string expectedName;
    private bool contains;

    public bool ContainsObject()
    {
        return contains;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == expectedName)
            contains = true;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == expectedName)
            contains = false;
    }
}