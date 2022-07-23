using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraMouseLook : MonoBehaviour
{
    public KeyCode toggleKey;
    public float clampDown = 80;
    public float clampUp = -90;
    public float sensitivity = 270f;
    public float moveSpeed = 20f;
    public Transform MainCamera;
    
    private float xRotation = 0f;
    private Camera cam;
    public GlobalVariables gv;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        
        if (gv.freeLookCamera)
        {
            (float x, float y) = InputHandler.GetLookInput();
            x *= sensitivity * Time.deltaTime;
            y *= sensitivity * Time.deltaTime;

            xRotation -= y;
            xRotation = Mathf.Clamp(xRotation, clampUp, clampDown);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, transform.eulerAngles.z);
            this.transform.parent.Rotate(Vector3.up * x);

            (float hor, float ver) = InputHandler.GetMoveInput();
            this.transform.parent.transform.position += transform.forward * ver * moveSpeed * Time.deltaTime;
            this.transform.parent.transform.position += transform.right * hor * moveSpeed * Time.deltaTime;
        }
        else
        {
            this.transform.parent.transform.position = MainCamera.position;
            this.transform.parent.transform.rotation = MainCamera.rotation;
        }
    }
}
