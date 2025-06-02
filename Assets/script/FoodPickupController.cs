using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickupController : MonoBehaviour
{
    [Header("Item Settings")]
    public PickupType itemType = PickupType.Food;
    public int amount = 1;
    public string itemName = "Food";
    public bool consumeImmediately = false; // For health packs

    [Header("Visual")]
    public GameObject pickupEffect;
    public SpriteRenderer itemSprite;
    public Color highlightColor = Color.yellow;

    public enum PickupType
    {
        Food,
        Medicine,
        HealthPack
    }

    private bool playerNearby = false;
    private int C = 0;
    private Color originalColor;
    private InventoryManager inventory;

    void Start()
    {
        inventory = InventoryManager.Instance;
        if (itemSprite != null)
        {
            originalColor = itemSprite.color;
        }

        // Set default item name based on type
        if (itemName == "Food")
        {
            switch (itemType)
            {
                case PickupType.Food: itemName = "Food"; break;
                case PickupType.Medicine: itemName = "Medicine"; break;
                case PickupType.HealthPack: itemName = "Health Pack"; break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            collision.GetComponent<PlayerMovement>()?.EnablePickUp();

            // Highlight item
            if (itemSprite != null)
            {
                itemSprite.color = highlightColor;
            }

            // Show pickup prompt
            if (inventory != null)
            {
                inventory.ShowPickupPrompt($"{itemName} (x{amount})");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            collision.GetComponent<PlayerMovement>()?.DisablePickUp();

            // Remove highlight
            if (itemSprite != null)
            {
                itemSprite.color = originalColor;
            }

            // Hide pickup prompt
            if (inventory != null)
            {
                inventory.HidePickupPrompt();
            }
        }
    }

    private void Update()
    {
        // Get input from ChannelScript (keeping your existing input system)
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            ChannelScript channelScript = player.GetComponent<ChannelScript>();
            if (channelScript != null)
            {
                C = channelScript.C;
            }
        }

        // Handle pickup input
        if (playerNearby && (Input.GetKeyDown(KeyCode.Space) || C == 1))
        {
            PickupItem();

            // Reset channel input if needed
            // if (C == 1) channelScript.C = 0;
        }
    }

    private void PickupItem()
    {
        if (inventory == null)
        {
            Debug.LogWarning("No InventoryManager found!");
            return;
        }

        // Spawn pickup effect
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        // Add to inventory based on type
        switch (itemType)
        {
            case PickupType.Food:
                inventory.AddFood(amount);
                break;

            case PickupType.Medicine:
                inventory.AddMedicine(amount);
                break;

            case PickupType.HealthPack:
                if (consumeImmediately)
                {
                    inventory.RestoreHealth(amount);
                }
                else
                {
                    inventory.AddMedicine(amount); // Treat as medicine
                }
                break;
        }

        // Hide pickup prompt
        inventory.HidePickupPrompt();

        // Disable/destroy the item
        gameObject.SetActive(false);
        // Or use: Destroy(gameObject);
    }
}
