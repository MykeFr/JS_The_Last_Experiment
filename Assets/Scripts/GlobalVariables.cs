using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public bool freeLookCamera = false;
    public bool StopAnimations = false;
    public Camera Main;
    public Camera FL;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            freeLookCamera = !freeLookCamera;
            Main.enabled = !freeLookCamera;
            FL.enabled = freeLookCamera;
        }
    }
}
