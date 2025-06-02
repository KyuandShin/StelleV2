using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100; 
    private int currentHealth; 

    private void Start()
    {
        currentHealth = maxHealth; 
    }

    public void RestoreHealth(int amount)
    {

        currentHealth += amount; 
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log("Health Restored! Current Health: " + currentHealth);
    }
}