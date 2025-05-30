using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreenController : MonoBehaviour
{
    public GameObject canvas;
    public Image[] introImages;
    private int currentImageIndex = 0;

    private void Start()
    {
        canvas.SetActive(true);
        ShowImage(0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canvas.activeSelf)
        {
            currentImageIndex++;
            // 如果还有更多的图片
            if (currentImageIndex < introImages.Length)
            {
                // 显示下一张图片
                ShowImage(currentImageIndex);
            }
            else
            {
                // 没有更多的图片，隐藏Canvas
                canvas.SetActive(false);
                // 重置currentImageIndex，便于下次观看
                currentImageIndex = 0;
            }
        }
    }

    void ShowImage(int index)
    {
        for (int i = 0; i < introImages.Length; i++)
        {
            introImages[i].enabled = (i == index);
        }
    }
}
