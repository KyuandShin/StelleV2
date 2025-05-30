using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth; // ��������PlayerHealth���
    public Text healthText; //Ѫ��������UI���

    private Image healthBar; //����������ʾ����ֵ��Image���

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)playerHealth.currentHealth / (float)playerHealth.maxHealth;
        healthText.text = playerHealth.currentHealth.ToString() + "/" + playerHealth.maxHealth.ToString();

    }

}


