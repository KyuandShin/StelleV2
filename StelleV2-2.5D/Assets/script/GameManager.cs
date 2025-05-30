using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this line
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject endingCanvas; // Canvas displayed at the end of the game
    public Image[] endingImages; // Final story images
    private int currentEndingImageIndex = 0; // Index of the currently displayed story image

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            gameObject.SetActive(false);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void EndGame()
    {
        // Check if endingCanvas is assigned before starting the coroutine
        if (endingCanvas != null)
        {
            StartCoroutine(PlayEndingImages());
        }
        else
        {
            Debug.LogWarning("endingCanvas is not assigned in GameManager. Cannot show ending.");
            // Try to restart the game directly if we can't show the ending
            RestartGame();
        }
    }

    public void RestartGame()
    {
        try
        {
            SceneManager.LoadScene(0);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to restart game: " + ex.Message);
            Debug.LogWarning("Make sure to add your scenes to Build Settings (File > Build Settings)");
        }
    }

    IEnumerator PlayEndingImages()
    {
        Debug.Log("Ending Canvas: " + endingCanvas);
        
        // Check if endingCanvas is assigned
        if (endingCanvas == null)
        {
            Debug.LogError("endingCanvas is not assigned in GameManager");
            RestartGame();
            yield break;
        }
        
        // Check if endingImages array is valid
        if (endingImages == null || endingImages.Length == 0)
        {
            Debug.LogError("endingImages array is not assigned or empty in GameManager");
            RestartGame();
            yield break;
        }
        
        // Display the Canvas at the end of the game
        endingCanvas.SetActive(true);

        // Display each image in sequence, wait for the player to click the left mouse button before showing the next one
        for (currentEndingImageIndex = 0; currentEndingImageIndex < endingImages.Length; currentEndingImageIndex++)
        {
            if (endingImages[currentEndingImageIndex] != null)
            {
                endingImages[currentEndingImageIndex].enabled = true;
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                endingImages[currentEndingImageIndex].enabled = false;
            }
        }

        // After displaying all images, restart the game
        RestartGame();
    }
}
