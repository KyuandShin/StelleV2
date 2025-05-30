using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickupController : MonoBehaviour
{
    private bool playerNearby = false; // 用于检测玩家是否在附近
    public int C = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            collision.GetComponent<PlayerMovement>().EnablePickUp(); // 在这里加上了括号
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            collision.GetComponent<PlayerMovement>().DisablePickUp(); // 在这里加上了括号
        }
    }

    private void Update()
    {
        C= GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().C;

        if (playerNearby && (Input.GetKeyDown(KeyCode.Space)||C==1))
        {
            // 销毁触发器附近的食物对象
            //Destroy(gameObject);
            gameObject.SetActive(false);

            // 获取playerHealth脚本，并将血量恢复10
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.RestoreHealth(10);
                }
            }
           // GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().C = 0;

        }
    }
    
}
