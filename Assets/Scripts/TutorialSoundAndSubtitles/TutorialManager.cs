using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private static string VoiceLine1 = "(Scarlet) Good job you have cleared your final test";
    private static string VoiceLine2 = "(Soldier[rushing in]) Ma'am we have received word that the Aether Foundation is back in business!";
    private static string VoiceLine3 = "(Alex) Wait! Isn't that..?";
    private static string VoiceLine4 = "(Scarlet) Yes, Alex that's the organization that we rescued you from.";
    private static string VoiceLine5 = "(Soldier) We suspect they are once again running experiments on children.";
    private static string VoiceLine6 = "(Alex) Let me go after them!";
    private static string VoiceLine7 = "(Scarlet) No Alex, I can let you go now.";
    private static string VoiceLine8 = "(Alex) But I have to go, you saved me back then, now it's my time to save all those kids, i can't let them suffer like I did!";
    private static string VoiceLine9 = "(Scarlet) Looks like you aren´t giving me any chances here. Fine I will let you go. Just make sure you don´t fail!";
    private static string VoiceLine10 = "(Alex) - I won't, I cannot!";

    public GameObject finalCanvas;
    public GameObject finalReachCollider;
    public GameObject finalText;
    public AudioManager audioManager;
    public GameObject player;
    public GameObject mainCamera;
    public GameObject grapllingHook;
    public GameObject physicsGun;
    [SerializeField] private List<GameObject> enemies;

    private bool ended = false;
    private bool allPicked = false;


    void Update()
    {
        if (!allPicked)
        {
            if (player.GetComponent<WeaponSwitching>().activeWeapons.Count >= 3)
            {
                
                allPicked = true;
                finalReachCollider.SetActive(false);
            }
                
        }

        if (!ended)
        {
            ended = true;
            foreach (GameObject enemy in enemies)
            {
                if (!enemy.Equals(null))
                    ended = false;
            }
            if (ended)
            {
                finalCanvas.SetActive(true);
                finalText.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<PlayerShooter>().enabled = false;
                player.GetComponent<WeaponSwitching>().enabled = false;
                mainCamera.GetComponent<MouseLook>().enabled = false;
                grapllingHook.GetComponent<GrapplingHook>().enabled = false;
                physicsGun.GetComponent<PhysicsGun>().enabled = false;
                endTransitions();
            }
        }
    }

    private void endTransitions()
    {
        audioManager.Play("EndVoiceLine1");
        finalText.GetComponent<Text>().text = VoiceLine1;
        StartCoroutine(voiceLine2());
    }

    //Audio Time VoiceLine1:2.4
    IEnumerator voiceLine2()
    {
        yield return new WaitForSeconds(3f);
        finalText.GetComponent<Text>().text = VoiceLine2;
        audioManager.Play("EndVoiceLine2");
        StartCoroutine(voiceLine3());
    }

    //Audio Time VoiceLine2:4.1
    IEnumerator voiceLine3()
    {
        yield return new WaitForSeconds(4.5f);
        finalText.GetComponent<Text>().text = VoiceLine3;
        audioManager.Play("EndVoiceLine3");
        StartCoroutine(voiceLine4());
    }

    //Audio Time VoiceLine3:1.3
    IEnumerator voiceLine4()
    {
        yield return new WaitForSeconds(1.8f);
        finalText.GetComponent<Text>().text = VoiceLine4;
        audioManager.Play("EndVoiceLine4");
        StartCoroutine(voiceLine5());
    }

    //Audio Time VoiceLine4:3.7
    IEnumerator voiceLine5()
    {
        yield return new WaitForSeconds(4f);
        finalText.GetComponent<Text>().text = VoiceLine5;
        audioManager.Play("EndVoiceLine5");
        StartCoroutine(voiceLine6());
    }

    //Audio Time VoiceLine5:3.6
    IEnumerator voiceLine6()
    {
        yield return new WaitForSeconds(4f);
        finalText.GetComponent<Text>().text = VoiceLine6;
        audioManager.Play("EndVoiceLine6");
        StartCoroutine(voiceLine7());
    }

    //Audio Time VoiceLine6:1.4
    IEnumerator voiceLine7()
    {
        yield return new WaitForSeconds(2f);
        finalText.GetComponent<Text>().text = VoiceLine7;
        audioManager.Play("EndVoiceLine7");
        StartCoroutine(voiceLine8());
    }

    //Audio Time VoiceLine7:2.2
    IEnumerator voiceLine8()
    {
        yield return new WaitForSeconds(2.8f);
        finalText.GetComponent<Text>().text = VoiceLine8;
        audioManager.Play("EndVoiceLine8");
        StartCoroutine(voiceLine9());
    }

    //Audio Time VoiceLine8:6.1
    IEnumerator voiceLine9()
    {
        yield return new WaitForSeconds(6.1f);
        finalText.GetComponent<Text>().text = VoiceLine9;
        audioManager.Play("EndVoiceLine9");
        StartCoroutine(voiceLine10());
    }

    //Audio TimeVoiceLine9:6.1
    IEnumerator voiceLine10()
    {
        yield return new WaitForSeconds(6.5f);
        finalText.GetComponent<Text>().text = VoiceLine10;
        audioManager.Play("EndVoiceLine10");
        StartCoroutine(changeScene());
    }

    //Audio Time VoiceLine10:1.7
    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(2.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
