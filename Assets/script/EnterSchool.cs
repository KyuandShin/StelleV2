using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnterSchool : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "Scenes/School 1st floor"; // Include folder path if needed
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private float transitionDelay = 0.5f;
    [SerializeField] private bool useLoadingScreen = false;

    private bool isPlayerNear = false;
    private bool isTransitioning = false;

    private void Start()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        // Debug check if scene exists
        Debug.Log($"Checking if scene '{targetSceneName}' exists in build settings...");
        Debug.Log($"Scene exists: {DoesSceneExist(targetSceneName)}");
    }

    private bool DoesSceneExist(string sceneName)
    {
        // Check if the scene exists in build settings
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromBuild = System.IO.Path.GetFileNameWithoutExtension(path);
            if (sceneNameFromBuild == System.IO.Path.GetFileNameWithoutExtension(sceneName))
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (isPlayerNear && !isTransitioning && Input.GetKeyDown(KeyCode.F))
        {
            TryEnterScene();
        }
    }

    private void TryEnterScene()
    {
        // Get just the scene name without the folder path for the check
        string sceneNameWithoutPath = System.IO.Path.GetFileNameWithoutExtension(targetSceneName);

        if (DoesSceneExist(sceneNameWithoutPath))
        {
            Debug.Log($"Loading scene: {sceneNameWithoutPath}");
            StartSceneTransition();
        }
        else
        {
            Debug.LogError($"Scene '{sceneNameWithoutPath}' is not included in Build Settings! Please add it in File > Build Settings");
        }
    }

    private void StartSceneTransition()
    {
        isTransitioning = true;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }

        if (useLoadingScreen)
        {
            StartCoroutine(LoadSceneAsync());
        }
        else
        {
            Invoke(nameof(LoadSceneDirectly), transitionDelay);
        }
    }

    private System.Collections.IEnumerator LoadSceneAsync()
    {
        string sceneNameWithoutPath = System.IO.Path.GetFileNameWithoutExtension(targetSceneName);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNameWithoutPath);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            Debug.Log($"Loading progress: {asyncLoad.progress * 100}%");
            yield return null;
        }

        yield return new WaitForSeconds(transitionDelay);

        Debug.Log("Activating scene...");
        asyncLoad.allowSceneActivation = true;
    }

    private void LoadSceneDirectly()
    {
        string sceneNameWithoutPath = System.IO.Path.GetFileNameWithoutExtension(targetSceneName);
        SceneManager.LoadScene(sceneNameWithoutPath);
    }

    // Changed to OnTriggerEnter2D for 2D collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Player entered trigger zone");

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }
            else
            {
                Debug.Log("Press F to enter");
            }
        }
    }

    // Changed to OnTriggerExit2D for 2D collisions
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player left trigger zone");

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }
    }
}