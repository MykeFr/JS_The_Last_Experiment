using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalReachTutorial : MonoBehaviour
{
    public GameObject canvasPickUpAudioTextCanvas;

    private bool hasPassed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasPassed)
        {
            TutorialAudioText.Instance.tutorialStageAudioText(4);
            hasPassed = true;
        }
    }

}
