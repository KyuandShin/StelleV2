using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth; 
    public Text healthText; 

    private Image healthBar; 

    
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    
    void Update()
    {
        healthBar.fillAmount = (float)playerHealth.currentHealth / (float)playerHealth.maxHealth;
        healthText.text = playerHealth.currentHealth.ToString() + "/" + playerHealth.maxHealth.ToString();

    }

}


