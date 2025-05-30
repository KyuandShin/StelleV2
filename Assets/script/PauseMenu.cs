using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // "keyCode.Escape" should be "KeyCode.Escape"
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
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void MainMenu()
    {
        GameIsPaused = false;
        Time.timeScale = 1.0f;
        
        // Check if the scene exists in build settings before loading
        try {
            // Try to load by name first
            SceneManager.LoadScene("Menu");
        }
        catch (System.Exception) {
            // If that fails, try to load the first scene (index 0)
            try {
                SceneManager.LoadScene(0);
            }
            catch (System.Exception ex) {
                Debug.LogError("Failed to load Menu scene: " + ex.Message);
                Debug.LogWarning("Make sure to add your scenes to Build Settings (File > Build Settings)");
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
