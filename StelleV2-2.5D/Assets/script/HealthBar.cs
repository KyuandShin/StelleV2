using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth; // 用于引用PlayerHealth组件
    public Text healthText; //血量计数的UI组件

    private Image healthBar; //定义用来显示生命值的Image组件

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


