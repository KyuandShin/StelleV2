using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScrollPickup : MonoBehaviour
{

    public GameObject[] scrolls;
    public GameObject[][] scrollGroups; // 存储每个卷轴的剧情组
    private int currentScrollIndex = 0;
    private int currentImageIndex = 0;
    private bool isDisplaying = false;
    private bool playerNearby = false;
    public int C = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            collision.GetComponent<PlayerMovement>().EnablePickUp();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            collision.GetComponent<PlayerMovement>().DisablePickUp();
        }
    }

    private void Update()
    {
        C = GameObject.Find("Player").gameObject.GetComponent<ChannelScript>().C;

        if (playerNearby && (Input.GetKeyDown(KeyCode.Space) || C == 1))
        {
            GameObject nearestScroll = FindNearestScroll();
            if (nearestScroll != null)
            {
                PickupScroll(nearestScroll);
            }
        }

        if (isDisplaying && Input.GetMouseButtonDown(0))


        {
            NextImage();
        }
    }

    GameObject FindNearestScroll()
    {
        GameObject player = GameObject.Find("Player");
        GameObject nearestScroll = null;
        float nearestDistance = Mathf.Infinity;  // 初始设置为无穷大

        foreach (GameObject scroll in scrolls)
        {
            // 只考虑活动的（没有被拾取的）卷轴
            if (scroll.activeSelf)
            {
                float distance = Vector2.Distance(player.transform.position, scroll.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestScroll = scroll;
                }
            }
        }

        return nearestScroll;
    }

    public void PickupScroll(GameObject scroll)
    {
        int index = System.Array.IndexOf(scrolls, scroll);
        if (index < 0)
        {
            Debug.LogError("Scroll not found: " + scroll.name);
            return;
        }

        scrolls[index].SetActive(false);
        scrollGroups[index][0].SetActive(true);
        isDisplaying = true;

        currentScrollIndex = index;
        currentImageIndex = 0;
    }

    void NextImage()
    {
        scrollGroups[currentScrollIndex][currentImageIndex].SetActive(false);

        currentImageIndex++;
        if (currentImageIndex < scrollGroups[currentScrollIndex].Length)
        {
            scrollGroups[currentScrollIndex][currentImageIndex].SetActive(true);
        }
        else
        {
            isDisplaying = false;
            currentScrollIndex++;
            currentImageIndex = 0;
        }
    }
}
