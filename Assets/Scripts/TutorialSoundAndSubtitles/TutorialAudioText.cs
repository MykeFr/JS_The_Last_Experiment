using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAudioText : MonoBehaviour
{

    private static string tutorialBegin = "(Scarlet) This is your final test before you can go out there. Move around a bit to warm up(using WASD), don't forget you have a double jump gadget (jump using SPACE) when you are ready look for a way to get out of there.";
    private static string pickUpWeapon2 = "(Scarlet) Aim to where you want to go and shoot (mouse1) to use the hook";
    private static string pickUpWeapon1 = "(Scarlet) Aim to the object you want to control and shoot (mouse1) to use the physics gun, (pull and push the object with the mouse wheel, control the rotation with Q E and R F)";
    private static string pickUpWeapon0 = "(Scarlet) Aim for the enemy in front of you and shoot him (mouse1)";
    private static string tutorialEnd = "(Scarlet) Now get rid of all those pests.";

    public GameObject subtitlesCanvas;
    public AudioManager audioManager;

    private static TutorialAudioText _instance;
    public static TutorialAudioText Instance
    {
        get
        {
            return _instance;
        }
    }



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            subtitlesCanvas.SetActive(true);//11.72
            subtitlesCanvas.GetComponent<Text>().text = tutorialBegin;
            audioManager.Play("IntroSound");
            StartCoroutine(deactivateCanvas(subtitlesCanvas, 12f));
        }
    }


    public void tutorialStageAudioText(int TutorialStage)
    {

        switch (TutorialStage)
        {
            case 0:
                subtitlesCanvas.SetActive(true);//4 Pistol
                subtitlesCanvas.GetComponent<Text>().text = pickUpWeapon0;
                audioManager.Play("PistolAudio");
                audioManager.Pause("PhysicsGunAudio");
                StartCoroutine(deactivateCanvas(subtitlesCanvas, 4f));
                break;

            case 1:
                subtitlesCanvas.SetActive(true);//10.5 physics
                subtitlesCanvas.GetComponent<Text>().text = pickUpWeapon1;
                audioManager.Play("PhysicsGunAudio");
                audioManager.Pause("GraplingSound");
                StartCoroutine(deactivateCanvas(subtitlesCanvas, 10.5f));
                break;

            case 2:
                subtitlesCanvas.SetActive(true);//4 Grapling
                subtitlesCanvas.GetComponent<Text>().text = pickUpWeapon2;
                audioManager.Play("GraplingSound");
                audioManager.Pause("IntroSound");
                StartCoroutine(deactivateCanvas(subtitlesCanvas, 4f));
                break;

            case 3:
                subtitlesCanvas.SetActive(true);//11.72
                subtitlesCanvas.GetComponent<Text>().text = tutorialBegin;
                audioManager.Play("IntroSound");
                StartCoroutine(deactivateCanvas(subtitlesCanvas, 12f));
                break;

            case 4:
                subtitlesCanvas.SetActive(true);//3
                subtitlesCanvas.GetComponent<Text>().text = tutorialEnd;
                audioManager.Play("TutorialEndingAudio");
                audioManager.Pause("PistolAudio");
                StartCoroutine(deactivateCanvas(subtitlesCanvas, 3f));
                break;
        }

        
    }

    IEnumerator deactivateCanvas(GameObject subtitlesCanvas, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        subtitlesCanvas.SetActive(false);
    }

}
