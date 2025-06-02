using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    public float dayDurationMinutes = 5f; // Real minutes for one full day
    public float startTime = 6f; // Start at 6 AM (0-24)

    [Header("Lighting")]
    public SpriteRenderer darkOverlay; // Black sprite that covers screen
    public AnimationCurve darknessLevel; // How dark it gets over 24 hours
    public Color nightTint = new Color(0f, 0f, 0f, 0.7f); // Max darkness

    [Header("UI")]
    public TMP_Text timeDisplay;
    public TMP_Text dayDisplay;
    public Image dayNightIcon;
    public Sprite sunIcon;
    public Sprite moonIcon;

    [Header("Audio")]
    public AudioSource ambientAudio;
    public AudioClip daySound;
    public AudioClip nightSound;

    // Current game state
    public float currentTime = 6f; // 0-24 hours
    public int currentDay = 1;

    // Properties
    public bool IsDay => currentTime >= 6f && currentTime < 20f;
    public bool IsNight => !IsDay;
    public float TimeSpeed => 24f / (dayDurationMinutes * 60f);

    // Events for other systems
    public System.Action OnDayStart;
    public System.Action OnNightStart;
    public System.Action OnNewDay;

    private bool wasDay = true;

    public static DayNightCycle Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            currentTime = startTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetupDarknessOverlay();
        SetupDefaultCurve();
        UpdateLighting();
        UpdateUI();
        UpdateAmbientSound();
    }

    void Update()
    {
        // Advance time
        currentTime += TimeSpeed * Time.deltaTime;

        // Handle new day
        if (currentTime >= 24f)
        {
            currentTime = 0f;
            currentDay++;
            OnNewDay?.Invoke();

            // Trigger daily food consumption
            if (InventoryManager.Instance != null)
            {
                StartCoroutine(DelayedFoodConsumption());
            }
        }

        // Check for day/night transitions
        CheckTransitions();

        // Update visuals
        UpdateLighting();
        UpdateUI();
    }

    void SetupDarknessOverlay()
    {
        if (darkOverlay == null)
        {
            // Create darkness overlay automatically
            GameObject overlayObj = new GameObject("DarknessOverlay");
            overlayObj.transform.SetParent(transform);

            darkOverlay = overlayObj.AddComponent<SpriteRenderer>();
            darkOverlay.sprite = CreateDarknessSprite();
            darkOverlay.color = Color.clear;
            darkOverlay.sortingOrder = 1000; // On top of everything

            // Scale to cover camera view
            Camera cam = Camera.main;
            float height = cam.orthographicSize * 2f;
            float width = height * cam.aspect;
            overlayObj.transform.localScale = new Vector3(width, height, 1f);
        }
    }

    Sprite CreateDarknessSprite()
    {
        // Create a simple black texture
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
    }

    void SetupDefaultCurve()
    {
        if (darknessLevel == null || darknessLevel.keys.Length == 0)
        {
            darknessLevel = new AnimationCurve();
            darknessLevel.AddKey(0f, 0.8f);    // Midnight - very dark
            darknessLevel.AddKey(0.25f, 0f);   // 6 AM - bright
            darknessLevel.AddKey(0.5f, 0f);    // Noon - brightest
            darknessLevel.AddKey(0.75f, 0f);   // 6 PM - still bright
            darknessLevel.AddKey(0.83f, 0.5f); // 8 PM - getting dark
            darknessLevel.AddKey(1f, 0.8f);    // Midnight - very dark
        }
    }

    void CheckTransitions()
    {
        bool isCurrentlyDay = IsDay;

        if (wasDay && !isCurrentlyDay)
        {
            // Day to night
            OnNightStart?.Invoke();
            UpdateAmbientSound();
            ShowMessage("Night falls... Be careful!");
        }
        else if (!wasDay && isCurrentlyDay)
        {
            // Night to day
            OnDayStart?.Invoke();
            UpdateAmbientSound();
            ShowMessage("Dawn breaks. Zombies retreat.");
        }

        wasDay = isCurrentlyDay;
    }

    void UpdateLighting()
    {
        if (darkOverlay == null) return;

        float normalizedTime = currentTime / 24f;
        float darkness = darknessLevel.Evaluate(normalizedTime);

        Color overlayColor = nightTint;
        overlayColor.a = darkness;
        darkOverlay.color = overlayColor;
    }

    void UpdateUI()
    {
        if (timeDisplay != null)
        {
            int hours = Mathf.FloorToInt(currentTime);
            int minutes = Mathf.FloorToInt((currentTime - hours) * 60f);
            timeDisplay.text = $"{hours:00}:{minutes:00}";
        }

        if (dayDisplay != null)
        {
            dayDisplay.text = $"Day {currentDay}";
        }

        if (dayNightIcon != null)
        {
            dayNightIcon.sprite = IsDay ? sunIcon : moonIcon;
        }
    }

    void UpdateAmbientSound()
    {
        if (ambientAudio == null) return;

        AudioClip targetClip = IsDay ? daySound : nightSound;
        if (targetClip != null && ambientAudio.clip != targetClip)
        {
            StartCoroutine(FadeAmbientSound(targetClip));
        }
    }

    IEnumerator FadeAmbientSound(AudioClip newClip)
    {
        float originalVolume = ambientAudio.volume;

        // Fade out
        while (ambientAudio.volume > 0)
        {
            ambientAudio.volume -= originalVolume * Time.deltaTime / 1f;
            yield return null;
        }

        // Switch clip
        ambientAudio.clip = newClip;
        ambientAudio.Play();

        // Fade in
        while (ambientAudio.volume < originalVolume)
        {
            ambientAudio.volume += originalVolume * Time.deltaTime / 1f;
            yield return null;
        }
    }

    IEnumerator DelayedFoodConsumption()
    {
        yield return new WaitForSeconds(1f);
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ConsumeDailyRations();
        }
    }

    void ShowMessage(string message)
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ShowStatusMessage(message);
        }
    }

    // Public utility methods
    public string GetTimeString()
    {
        int hours = Mathf.FloorToInt(currentTime);
        int minutes = Mathf.FloorToInt((currentTime - hours) * 60f);
        return $"{hours:00}:{minutes:00}";
    }

    public void SetTime(float time)
    {
        currentTime = Mathf.Clamp(time, 0f, 24f);
        UpdateLighting();
        UpdateUI();
    }

    public void SkipToTime(float targetTime)
    {
        if (targetTime < currentTime)
        {
            currentDay++;
        }
        currentTime = targetTime;
        UpdateLighting();
        UpdateUI();
    }
}

