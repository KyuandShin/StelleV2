using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Main inventory manager - attach this to a persistent GameObject
public class InventoryManager : MonoBehaviour
{
    [Header("Resources")]
    public int food = 0;
    public int medicine = 0;
    public int health = 100;
    public int maxHealth = 100;
    public int friendsCount = 1; // Stelle + friends

    [Header("Consumption Settings")]
    public int dailyFoodPerPerson = 1;
    public int medicineHealAmount = 25;

    [Header("UI References")]
    public Text healthText;
    public Text foodText;
    public Text medicineText;
    public Text friendsText;
    public Text statusText;
    public Text pickupPromptText; // "Press SPACE to pick up"

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip consumeSound;
    public AudioClip warningSound;

    public static InventoryManager Instance;
    private float statusMessageTimer = 0f;
    private const float STATUS_MESSAGE_DURATION = 2f;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
        HidePickupPrompt();
    }

    void Update()
    {
        // Handle status message timer
        if (statusMessageTimer > 0)
        {
            statusMessageTimer -= Time.deltaTime;
            if (statusMessageTimer <= 0)
            {
                if (statusText != null) statusText.text = "";
            }
        }

        // Debug keys (remove in final build)
        if (Input.GetKeyDown(KeyCode.F))
        {
            ConsumeDailyRations();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            UseMedicine();
        }
    }

    public void AddFood(int amount)
    {
        food += amount;
        PlaySound(pickupSound);
        ShowStatusMessage($"Found {amount} food!");
        UpdateUI();
    }

    public void AddMedicine(int amount)
    {
        medicine += amount;
        PlaySound(pickupSound);
        ShowStatusMessage($"Found {amount} medicine!");
        UpdateUI();
    }

    public void RestoreHealth(int amount)
    {
        int oldHealth = health;
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        int actualHealed = health - oldHealth;

        if (actualHealed > 0)
        {
            PlaySound(consumeSound);
            ShowStatusMessage($"Health restored by {actualHealed}!");
        }
        UpdateUI();
    }

    public void AddFriend()
    {
        friendsCount++;
        ShowStatusMessage($"New survivor joined! Group size: {friendsCount}");
        UpdateUI();
    }

    public bool ConsumeDailyRations()
    {
        int totalFoodNeeded = friendsCount * dailyFoodPerPerson;

        if (food >= totalFoodNeeded)
        {
            food -= totalFoodNeeded;
            PlaySound(consumeSound);
            ShowStatusMessage($"Group consumed {totalFoodNeeded} food");
            UpdateUI();
            return true;
        }
        else
        {
            PlaySound(warningSound);
            ShowStatusMessage($"Not enough food! Need {totalFoodNeeded}, have {food}");

            // Starvation penalty
            int starvationDamage = Mathf.Min(15, health - 1); // Don't kill player
            health -= starvationDamage;
            ShowStatusMessage($"Starvation! Lost {starvationDamage} health");
            UpdateUI();
            return false;
        }
    }

    public bool UseMedicine()
    {
        if (medicine <= 0)
        {
            PlaySound(warningSound);
            ShowStatusMessage("No medicine available!");
            return false;
        }

        if (health >= maxHealth)
        {
            ShowStatusMessage("Health is already full!");
            return false;
        }

        medicine--;
        int oldHealth = health;
        health = Mathf.Min(maxHealth, health + medicineHealAmount);
        int healedAmount = health - oldHealth;

        PlaySound(consumeSound);
        ShowStatusMessage($"Used medicine. Healed {healedAmount} HP");
        UpdateUI();
        return true;
    }

    public void UpdateUI()
    {
        if (healthText != null)
            healthText.text = $"Health: {health}/{maxHealth}";

        if (foodText != null)
        {
            int daysOfFood = friendsCount > 0 ? food / (friendsCount * dailyFoodPerPerson) : 0;
            foodText.text = $"Food: {food} ({daysOfFood} days)";
        }

        if (medicineText != null)
            medicineText.text = $"Medicine: {medicine}";

        if (friendsText != null)
            friendsText.text = $"Survivors: {friendsCount}";
    }

    public void ShowStatusMessage(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusMessageTimer = STATUS_MESSAGE_DURATION;
        }
    }

    public void ShowPickupPrompt(string itemName)
    {
        if (pickupPromptText != null)
        {
            pickupPromptText.text = $"Press SPACE to pick up {itemName}";
            pickupPromptText.gameObject.SetActive(true);
        }
    }

    public void HidePickupPrompt()
    {
        if (pickupPromptText != null)
        {
            pickupPromptText.gameObject.SetActive(false);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Get food efficiency info
    public string GetFoodEfficiencyInfo()
    {
        if (friendsCount <= 0) return "No survivors";

        int daysRemaining = food / (friendsCount * dailyFoodPerPerson);
        int dailyConsumption = friendsCount * dailyFoodPerPerson;

        return $"Daily consumption: {dailyConsumption} food\nDays remaining: {daysRemaining}";
    }
}