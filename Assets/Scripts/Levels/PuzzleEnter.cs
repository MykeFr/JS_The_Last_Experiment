using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEnter : MonoBehaviour
{
    private bool hasEntered = false; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasEntered)
        {
            LabAudioText.Instance.labAudioText(1);
            hasEntered = true;
        }
    }
}
