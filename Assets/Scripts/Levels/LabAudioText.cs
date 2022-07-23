using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabAudioText : MonoBehaviour
{

    const string LabBegin = "(Scarlet) Took us long enough to get here without being seen, don't screw this up now. Look around and find a way to get in, you can't just barge in through the front door.";
    const string FirstPuzzle = "(Scarlet) Looks like you are facing a different kind of problem now, try making use of that physics gun.";
    const string BossFight1 = "(Alex) WHO ARE YOU?! You look just like me!";
    const string BossFight2 = "(Scarlet) I guess this is what they got by doing experiments on you...";
    const string BossFight3 = "(Alex) Well whether he's a clone or not, this fight ends in the same way.";
    const string BossHalfLife1 = "(Scarlet) Did he just power up? ";
    const string BossHalfLife2 = "(Alex) Whatever he does he can't counter my physics gun, this is the strongest thing I got!";
    const string BossEnd1 = "(Alex) I got him!";
    const string BossEnd2 = "(Scarlet) Now get the kids and get out of there!";

    public GameObject subtitlesCanvas;
    public AudioManager audioManager;

    private static LabAudioText _instance;
    public static LabAudioText Instance
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
        }
    }


    public void labAudioText(int LabStage)
    {
        switch (LabStage)
        {
            case 0:// 0 Begin Lab Scene
                subtitlesCanvas.SetActive(true);
                subtitlesCanvas.GetComponent<Text>().text = LabBegin;
                audioManager.Play("LabIntroSound");//Time: 8.28
                StartCoroutine(deactivateCanvas(8.5f));
                break;

            case 1://1 Puzzle
                subtitlesCanvas.SetActive(true);
                subtitlesCanvas.GetComponent<Text>().text = FirstPuzzle;
                audioManager.Play("LabPuzzle");//Time:4.8
                StartCoroutine(deactivateCanvas(5f));
                break;

            case 2://2 BossFight
                subtitlesCanvas.SetActive(true);
                subtitlesCanvas.GetComponent<Text>().text = BossFight1;
                audioManager.Play("BossFightAudio1");
                StartCoroutine(bossFight2());
                break;

            case 3://BossHalfLife
                subtitlesCanvas.SetActive(true);//4 Grapling
                subtitlesCanvas.GetComponent<Text>().text = BossHalfLife1;
                audioManager.Play("BossHalfLifeAudio1");
                StartCoroutine(bossHalfLife2());
                break;

            case 4://BossEnd
                subtitlesCanvas.SetActive(true);//11.72
                subtitlesCanvas.GetComponent<Text>().text = BossEnd1;
                audioManager.Play("BossEndAudio1");
                StartCoroutine(bossEnd2());
                break;
        }


    }

    IEnumerator deactivateCanvas(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        subtitlesCanvas.SetActive(false);
    }

    //Audio Time: 2.5
    IEnumerator bossFight2()
    {
        yield return new WaitForSeconds(3f);
        subtitlesCanvas.GetComponent<Text>().text = BossFight2;
        audioManager.Play("BossFightAudio2");
        StartCoroutine(bossFight3());
    }

    /* 
     * Audio Time BossFight2: 3.216
     * Audio Time BossFight3: 4
     */
    IEnumerator bossFight3()
    {
        yield return new WaitForSeconds(4f);
        subtitlesCanvas.GetComponent<Text>().text = BossFight3;
        audioManager.Play("BossFightAudio3");
        StartCoroutine(deactivateCanvas(4.5f));
    }

    /*
     * Audio Time BossHlafLife1:1.5
     * Audio Time BossHalfLife2:4.4
     */
    IEnumerator bossHalfLife2()
    {
        yield return new WaitForSeconds(2f);
        subtitlesCanvas.GetComponent<Text>().text = BossHalfLife2;
        audioManager.Play("BossHalfLifeAudio2");
        StartCoroutine(deactivateCanvas(5f));
    }

    /*
     * Audio Time BossEnd1:0.9
     * Audio Time BossEnd2:2.1
     */
    IEnumerator bossEnd2()
    {
        yield return new WaitForSeconds(1.4f);
        subtitlesCanvas.GetComponent<Text>().text = BossEnd2;
        audioManager.Play("BossEndAudio2");
        StartCoroutine(deactivateCanvas(2.5f));
    }
}
