using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static (float, float) GetMoveInput(){
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        return (horizontal, vertical);
    }

    public static bool IsMovingForward(){
        (float horizontal, float vertical) = GetMoveInput();
        return vertical > 0f;
    }

    public static (float, float) GetLookInput(){
        float X = Input.GetAxis("Look X");
        float y = Input.GetAxis("Look Y");
        return (X, y);
    }

    public static bool JumpInputDown(){
        return CrossPlatformInputManager.GetButtonDown("Jump");
    }

    public static bool Fire1Down(){
        return CrossPlatformInputManager.GetButtonDown("Fire1");
    }

    public static bool Fire1Up(){
        return CrossPlatformInputManager.GetButtonUp("Fire1");
    }

    public static bool Fire1Pressed(){
        return CrossPlatformInputManager.GetButton("Fire1");
    }

    public static bool Fire2Down(){
        return CrossPlatformInputManager.GetButtonDown("Fire2");
    }

    public static bool Fire2Up(){
        return CrossPlatformInputManager.GetButtonUp("Fire2");
    }

    public static bool Fire2Pressed(){
        return CrossPlatformInputManager.GetButton("Fire2");
    }
}