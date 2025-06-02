using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieNightBehavior : MonoBehaviour
{
    [Header("Behavior Changes")]
    public float daySpeedMultiplier = 0.6f;
    public float nightSpeedMultiplier = 1.4f;
    public float dayDamageMultiplier = 0.7f;
    public float nightDamageMultiplier = 1.3f;

    [Header("Visual Changes")]
    public SpriteRenderer zombieSprite;
    public Color dayColor = Color.white;
    public Color nightColor = new Color(1.2f, 0.8f, 0.8f); // Slightly red tint

    private DayNightCycle dayNightManager;
    private float baseSpeed = 1f;
    private float baseDamage = 10f;

    void Start()
    {
        dayNightManager = DayNightCycle.Instance;

        // Get base values from your existing zombie scripts
        // baseSpeed = GetComponent<YourZombieMovement>().speed;
        // baseDamage = GetComponent<YourZombieDamage>().damage;

        if (dayNightManager != null)
        {
            dayNightManager.OnDayStart += ApplyDayBehavior;
            dayNightManager.OnNightStart += ApplyNightBehavior;

            // Apply initial behavior
            if (dayNightManager.IsDay)
                ApplyDayBehavior();
            else
                ApplyNightBehavior();
        }
    }

    void ApplyDayBehavior()
    {
        if (zombieSprite != null)
            zombieSprite.color = dayColor;

        // Apply to your zombie scripts:
        // GetComponent<YourZombieMovement>().speed = baseSpeed * daySpeedMultiplier;
        // GetComponent<YourZombieDamage>().damage = baseDamage * dayDamageMultiplier;
    }

    void ApplyNightBehavior()
    {
        if (zombieSprite != null)
            zombieSprite.color = nightColor;

        // Apply to your zombie scripts:
        // GetComponent<YourZombieMovement>().speed = baseSpeed * nightSpeedMultiplier;
        // GetComponent<YourZombieDamage>().damage = baseDamage * nightDamageMultiplier;
    }

    void OnDestroy()
    {
        if (dayNightManager != null)
        {
            dayNightManager.OnDayStart -= ApplyDayBehavior;
            dayNightManager.OnNightStart -= ApplyNightBehavior;
        }
    }
}