using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BedSleepingSystem : MonoBehaviour
{
    [Header("Sleep Settings")]
    public float sleepDuration = 8f; // Hours to sleep (6 PM to 2 AM = 8 hours)
    public float interactionRange = 2f;
    public KeyCode sleepKey = KeyCode.E;

    [Header("UI Elements")]
    public GameObject sleepPromptUI;
    public TMP_Text sleepPromptText;
    public TMP_Text sleepWarningText;
    public GameObject sleepWarningUI;
    public Button sleepButton;
    public Button cancelButton;

    [Header("Sleep Effects")]
    public GameObject sleepOverlay; // Black screen during sleep
    public float fadeSpeed = 2f;

    [Header("Audio")]
    public AudioSource bedAudioSource;
    public AudioClip sleepSound;
    public AudioClip wakeUpSound;

    private Transform player;
    private DayNightCycle dayNightManager;
    private bool playerInRange = false;
    private bool isSleeping = false;
    private Coroutine sleepCoroutine;

    // Sleep benefits (optional)
    [Header("Sleep Benefits")]
    public bool restoresHealth = true;
    public float healthRestoreAmount = 20f;
    public bool restoresHunger = false;
    public float hungerRestoreAmount = 10f;

    void Start()
    {
        dayNightManager = DayNightCycle.Instance;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Setup UI
        if (sleepPromptUI != null)
            sleepPromptUI.SetActive(false);

        if (sleepWarningUI != null)
            sleepWarningUI.SetActive(false);

        if (sleepOverlay != null)
        {
            sleepOverlay.SetActive(false);
            // Make sure overlay covers entire screen
            Canvas overlayCanvas = sleepOverlay.GetComponent<Canvas>();
            if (overlayCanvas != null)
                overlayCanvas.sortingOrder = 1000;
        }

        // Setup button events
        if (sleepButton != null)
            sleepButton.onClick.AddListener(StartSleeping);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(CancelSleep);

        // Subscribe to day/night events for warnings
        if (dayNightManager != null)
        {
            dayNightManager.OnNightStart += ShowNightWarning;
        }
    }

    void Update()
    {
        if (player == null || isSleeping) return;

        CheckPlayerDistance();
        HandleInput();
    }

    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        bool wasInRange = playerInRange;
        playerInRange = distance <= interactionRange;

        // Show/hide sleep prompt
        if (playerInRange && !wasInRange)
        {
            ShowSleepPrompt();
        }
        else if (!playerInRange && wasInRange)
        {
            HideSleepPrompt();
        }
    }

    void HandleInput()
    {
        if (playerInRange && Input.GetKeyDown(sleepKey))
        {
            TryToSleep();
        }
    }

    void ShowSleepPrompt()
    {
        if (sleepPromptUI != null && !isSleeping)
        {
            sleepPromptUI.SetActive(true);

            // Update prompt text based on time of day
            if (sleepPromptText != null)
            {
                if (dayNightManager != null && dayNightManager.IsNight)
                {
                    sleepPromptText.text = "Press E to Sleep (Skip to Dawn)";
                }
                else
                {
                    sleepPromptText.text = "Press E to Rest (Skip to Night)";
                }
            }
        }
    }

    void HideSleepPrompt()
    {
        if (sleepPromptUI != null)
            sleepPromptUI.SetActive(false);
    }

    void TryToSleep()
    {
        if (dayNightManager == null) return;

        // Show sleep confirmation dialog
        ShowSleepDialog();
    }

    void ShowSleepDialog()
    {
        if (sleepWarningUI == null) return;

        sleepWarningUI.SetActive(true);

        if (sleepWarningText != null && dayNightManager != null)
        {
            if (dayNightManager.IsNight)
            {
                sleepWarningText.text = "Sleep until dawn?\n(You'll skip the dangerous night hours)\n\nCurrent time: " + dayNightManager.GetTimeString();
            }
            else
            {
                sleepWarningText.text = "Rest until evening?\n(Time will pass quickly)\n\nCurrent time: " + dayNightManager.GetTimeString();
            }
        }

        // Pause game while showing dialog
        Time.timeScale = 0f;
    }

    public void StartSleeping()
    {
        if (sleepCoroutine != null)
            StopCoroutine(sleepCoroutine);

        sleepCoroutine = StartCoroutine(SleepSequence());
    }

    public void CancelSleep()
    {
        if (sleepWarningUI != null)
            sleepWarningUI.SetActive(false);

        // Resume game
        Time.timeScale = 1f;
    }

    IEnumerator SleepSequence()
    {
        isSleeping = true;

        // Hide UI
        if (sleepWarningUI != null)
            sleepWarningUI.SetActive(false);
        if (sleepPromptUI != null)
            sleepPromptUI.SetActive(false);

        // Resume time for sleep sequence
        Time.timeScale = 1f;

        // Play sleep sound
        if (bedAudioSource != null && sleepSound != null)
        {
            bedAudioSource.clip = sleepSound;
            bedAudioSource.Play();
        }

        // Fade to black
        yield return StartCoroutine(FadeToBlack());

        // Disable player movement (if you have a player controller)
        DisablePlayerMovement();

        // Skip time
        SkipTime();

        // Apply sleep benefits
        ApplySleepBenefits();

        // Wait a moment
        yield return new WaitForSeconds(1f);

        // Play wake up sound
        if (bedAudioSource != null && wakeUpSound != null)
        {
            bedAudioSource.clip = wakeUpSound;
            bedAudioSource.Play();
        }

        // Fade back in
        yield return StartCoroutine(FadeFromBlack());

        // Re-enable player movement
        EnablePlayerMovement();

        // Show wake up message
        ShowWakeUpMessage();

        isSleeping = false;
    }

    void SkipTime()
    {
        if (dayNightManager == null) return;

        if (dayNightManager.IsNight)
        {
            // Sleep until dawn (6 AM)
            dayNightManager.SkipToTime(6f);
        }
        else
        {
            // Rest until evening (18:00 / 6 PM)
            dayNightManager.SkipToTime(18f);
        }
    }

    void ApplySleepBenefits()
    {
        // You can integrate this with your health/hunger systems
        if (restoresHealth)
        {
            // Example: PlayerHealth.Instance.Heal(healthRestoreAmount);
            ShowMessage($"You feel refreshed! Health restored.");
        }

        if (restoresHunger)
        {
            // Example: PlayerHunger.Instance.Feed(hungerRestoreAmount);
            ShowMessage($"You feel less hungry after resting.");
        }
    }

    IEnumerator FadeToBlack()
    {
        if (sleepOverlay == null) yield break;

        sleepOverlay.SetActive(true);
        Image fadeImage = sleepOverlay.GetComponent<Image>();

        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f;
            fadeImage.color = color;

            while (color.a < 1f)
            {
                color.a += fadeSpeed * Time.deltaTime;
                fadeImage.color = color;
                yield return null;
            }
        }
    }

    IEnumerator FadeFromBlack()
    {
        if (sleepOverlay == null) yield break;

        Image fadeImage = sleepOverlay.GetComponent<Image>();

        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 1f;
            fadeImage.color = color;

            while (color.a > 0f)
            {
                color.a -= fadeSpeed * Time.deltaTime;
                fadeImage.color = color;
                yield return null;
            }
        }

        sleepOverlay.SetActive(false);
    }

    void ShowNightWarning()
    {
        // Show warning when night starts if player is not near a bed
        if (!playerInRange)
        {
            ShowMessage("It's getting dark! Find a bed to rest safely through the night.");

            // You could also show a UI warning that stays on screen
            StartCoroutine(ShowTemporaryWarning());
        }
    }

    IEnumerator ShowTemporaryWarning()
    {
        // Create temporary warning UI
        GameObject warningObj = new GameObject("NightWarning");
        Canvas canvas = warningObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        GameObject textObj = new GameObject("WarningText");
        textObj.transform.SetParent(warningObj.transform);

        Text warningText = textObj.AddComponent<Text>();
        warningText.text = "⚠️ DANGER: Night is falling! Seek shelter!";
        warningText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        warningText.fontSize = 24;
        warningText.color = Color.red;
        warningText.alignment = TextAnchor.MiddleCenter;

        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0.8f);
        rect.anchorMax = new Vector2(1, 0.9f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Flash the warning
        for (int i = 0; i < 6; i++)
        {
            warningText.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            warningText.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
        }

        Destroy(warningObj);
    }

    void DisablePlayerMovement()
    {
        // Add your player movement disabling code here
        // Example:
        // player.GetComponent<PlayerMovement>().enabled = false;
        // player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void EnablePlayerMovement()
    {
        // Add your player movement enabling code here
        // Example:
        // player.GetComponent<PlayerMovement>().enabled = true;
    }

    void ShowWakeUpMessage()
    {
        if (dayNightManager != null)
        {
            string timeStr = dayNightManager.GetTimeString();
            string message = dayNightManager.IsDay ?
                $"Good morning! Time: {timeStr}" :
                $"You wake up rested. Time: {timeStr}";
            ShowMessage(message);
        }
    }

    void ShowMessage(string message)
    {
        // Use your existing message system
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ShowStatusMessage(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw interaction range in editora
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

    void OnDestroy()
    {
        if (dayNightManager != null)
        {
            dayNightManager.OnNightStart -= ShowNightWarning;
        }
    }
}