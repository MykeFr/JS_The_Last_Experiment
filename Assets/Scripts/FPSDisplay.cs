using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public Text FpsText;
    public KeyCode toggleKey;
    public float pollingTime = 1f;

    private float time = 0f;
    private bool displaying = false;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            displaying = !displaying;
            if (displaying)
                time = pollingTime;
            else
                FpsText.text = "";
        }
        if (displaying)
        {
            time += Time.deltaTime;

            if (time >= pollingTime)
            {
                int fps = (int)(1f / Time.unscaledDeltaTime);
                FpsText.text = fps.ToString() + " FPS";
                time = 0f;
            }
        }
    }
}
