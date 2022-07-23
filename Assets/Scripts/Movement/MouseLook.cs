using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float clampDown = 80;
    public float clampUp = -90;
    public float sensitivity = 100f;
    public Transform player;
    
    float xRotation = 0f;

    public GlobalVariables gv;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        if (!gv.freeLookCamera)
        {
            (float x, float y) = InputHandler.GetLookInput();
            x *= sensitivity * Time.deltaTime;
            y *= sensitivity * Time.deltaTime;

            xRotation -= y;
            xRotation = Mathf.Clamp(xRotation, clampUp, clampDown);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, transform.eulerAngles.z);
            player.Rotate(Vector3.up * x);
        }
        
    }
}
