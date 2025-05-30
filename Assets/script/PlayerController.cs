using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100; // �������ֵ
    private int currentHealth; // ��ǰ����ֵ

    private void Start()
    {
        currentHealth = maxHealth; // ��ʼ����ǰ����ֵΪ�������ֵ
    }

    public void RestoreHealth(int amount)
    {

        currentHealth += amount; // ����Ѫ��
        currentHealth = Mathf.Min(currentHealth, maxHealth); // ����Ѫ���������������ֵ

        Debug.Log("Health Restored! Current Health: " + currentHealth);
    }
}