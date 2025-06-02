using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterSchool : MonoBehaviour
{
    [SerializeField] private string sceneName = "School 1st floor"; // Must match Build Settings
    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            if (Application.CanStreamedLevelBeLoaded(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("Scene '" + sceneName + "' not found in Build Settings.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Press F to enter.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("You left the entrance.");
        }
    }
}
