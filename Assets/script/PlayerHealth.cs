using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // ��ҵ��������ֵ
    public int currentHealth; // ��ҵĵ�ǰ����ֵ
    public float dieTime;
    public GameObject deathScreen; // ��Ҫ��ʾ��ȫ��ͼƬ

    void Start()
    {
        currentHealth = maxHealth;
        deathScreen.SetActive(false); // ��ʼ�����ͼƬ�����ص�
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Take damage!");
        currentHealth -= damage;

        // �������˶���
        GetComponent<Animator>().SetTrigger("Hit");
        SoundManager.PlayHurtClip(); // ����������Ƶ

        // ȷ������ֵ�������0
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    // ��������ֵ�ķ���
    public void RestoreHealth(int amount)
    {
        currentHealth += amount;

        // ȷ������ֵ���ᳬ���������ֵ
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log("Health Restored! Current Health: " + currentHealth);
    }

    void Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        dieTime = 4; // 4�����ʾͼƬ��3���������Ϸ
        Invoke("ShowDeathScreen", 1);
        Invoke("RestartGame", dieTime);
        Invoke("KillPlayer", dieTime);
    }

    void ShowDeathScreen()
    {
        deathScreen.SetActive(true); // ��ʾͼƬ
    }

    void KillPlayer()
    {
        gameObject.SetActive(false);
    }

    void RestartGame()
    {
        // ����GameManagerӦ��������Ϸ
        GameManager.instance.RestartGame();
    }
}




