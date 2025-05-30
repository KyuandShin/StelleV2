using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickupController : MonoBehaviour
{
    private bool playerNearby = false; // ���ڼ������Ƿ��ڸ���
    public int C = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            collision.GetComponent<PlayerMovement>().EnablePickUp(); // ���������������
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            collision.GetComponent<PlayerMovement>().DisablePickUp(); // ���������������
        }
    }

    private void Update()
    {
        C= GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().C;

        if (playerNearby && (Input.GetKeyDown(KeyCode.Space)||C==1))
        {
            // ���ٴ�����������ʳ�����
            //Destroy(gameObject);
            gameObject.SetActive(false);

            // ��ȡplayerHealth�ű�������Ѫ���ָ�10
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
