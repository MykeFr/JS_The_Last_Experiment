using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject GameOverUI;
    public GameObject pauseMainMenu;
    public GameObject pauseOptions;
    public GameObject audioManager;
    public GameObject mainCamera;
    public GameObject grappling;
    public GameObject physicsGun;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for(int i = 0; i< resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMainMenu.SetActive(true);
        pauseOptions.SetActive(false);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        audioManager.GetComponent<AudioSource>().UnPause();
        GameIsPaused = false;
        grappling.GetComponent<GrapplingHook>().enabled = true;
        grappling.GetComponent<AudioSource>().enabled = true;
        physicsGun.GetComponent<AudioSource>().enabled = true;
        physicsGun.GetComponent<PhysicsGun>().enabled = true;
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        audioManager.GetComponent<AudioSource>().Pause();
        GameIsPaused = true;
        grappling.GetComponent<GrapplingHook>().enabled = false;
        grappling.GetComponent<AudioSource>().enabled = false;
        physicsGun.GetComponent<AudioSource>().enabled = false;
        physicsGun.GetComponent<PhysicsGun>().enabled = false;
    }

    public void LoadOptions()
    {
        pauseOptions.SetActive(true);
        pauseMainMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

    public void BackOptions()
    {
        pauseOptions.SetActive(false);
        pauseMainMenu.SetActive(true);
    }

    public void SetSensitivity(float sensitivity)
    {
        mainCamera.GetComponent<MouseLook>().sensitivity = sensitivity;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.Confined;
        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
        audioManager.GetComponent<AudioSource>().Pause();
        GameIsPaused = true;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
