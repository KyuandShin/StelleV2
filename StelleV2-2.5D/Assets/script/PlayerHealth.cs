using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // 玩家的最大生命值
    public int currentHealth; // 玩家的当前生命值
    public float dieTime;
    public GameObject deathScreen; // 你要显示的全屏图片

    void Start()
    {
        currentHealth = maxHealth;
        deathScreen.SetActive(false); // 初始情况下图片是隐藏的
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Take damage!");
        currentHealth -= damage;

        // 播放受伤动画
        GetComponent<Animator>().SetTrigger("Hit");
        SoundManager.PlayHurtClip(); // 播放受伤音频

        // 确保生命值不会低于0
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    // 增加生命值的方法
    public void RestoreHealth(int amount)
    {
        currentHealth += amount;

        // 确保生命值不会超过最大生命值
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log("Health Restored! Current Health: " + currentHealth);
    }

    void Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        dieTime = 4; // 4秒后显示图片，3秒后重启游戏
        Invoke("ShowDeathScreen", 1);
        Invoke("RestartGame", dieTime);
        Invoke("KillPlayer", dieTime);
    }

    void ShowDeathScreen()
    {
        deathScreen.SetActive(true); // 显示图片
    }

    void KillPlayer()
    {
        gameObject.SetActive(false);
    }

    void RestartGame()
    {
        // 告诉GameManager应该重启游戏
        GameManager.instance.RestartGame();
    }
}




