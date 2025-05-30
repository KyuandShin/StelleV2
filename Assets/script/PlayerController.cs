using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100; // 最大生命值
    private int currentHealth; // 当前生命值

    private void Start()
    {
        currentHealth = maxHealth; // 初始化当前生命值为最大生命值
    }

    public void RestoreHealth(int amount)
    {

        currentHealth += amount; // 增加血量
        currentHealth = Mathf.Min(currentHealth, maxHealth); // 限制血量不超过最大生命值

        Debug.Log("Health Restored! Current Health: " + currentHealth);
    }
}