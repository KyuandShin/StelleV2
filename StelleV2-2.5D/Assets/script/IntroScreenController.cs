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
            // ������и����ͼƬ
            if (currentImageIndex < introImages.Length)
            {
                // ��ʾ��һ��ͼƬ
                ShowImage(currentImageIndex);
            }
            else
            {
                // û�и����ͼƬ������Canvas
                canvas.SetActive(false);
                // ����currentImageIndex�������´ιۿ�
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
