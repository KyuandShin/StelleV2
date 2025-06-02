using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; 
    public int currentHealth; 
    public float dieTime;
    public GameObject deathScreen; 

    void Start()
    {
        currentHealth = maxHealth;
        deathScreen.SetActive(false); 
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Take damage!");
        currentHealth -= damage;

        
        GetComponent<Animator>().SetTrigger("Hit");
        SoundManager.PlayHurtClip(); 

        
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    
    public void RestoreHealth(int amount)
    {
        currentHealth += amount;

        
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log("Health Restored! Current Health: " + currentHealth);
    }

    void Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        dieTime = 4; 
        Invoke("ShowDeathScreen", 1);
        Invoke("RestartGame", dieTime);
        Invoke("KillPlayer", dieTime);
    }

    void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
    }

    void KillPlayer()
    {
        gameObject.SetActive(false);
    }

    void RestartGame()
    {
        GameManager.instance.RestartGame();
    }
}




