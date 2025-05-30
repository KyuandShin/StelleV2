using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBG : MonoBehaviour
{
    private AudioSource audioSrc; //定义音频源
    private AudioClip bg; //定义音频剪辑
    public float volume = 0.5f; //音量，范围为0.0到1.0

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>(); //获取音频源组件
        bg = Resources.Load<AudioClip>("bg"); //加载音频剪辑

        audioSrc.volume = volume; //设置音量
        audioSrc.clip = bg; //设置音频剪辑
        audioSrc.Play(); //播放音频
    }

    // Update is called once per frame
    void Update()
    {

    }
}
