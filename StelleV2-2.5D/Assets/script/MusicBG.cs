using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBG : MonoBehaviour
{
    private AudioSource audioSrc; //������ƵԴ
    private AudioClip bg; //������Ƶ����
    public float volume = 0.5f; //��������ΧΪ0.0��1.0

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>(); //��ȡ��ƵԴ���
        bg = Resources.Load<AudioClip>("bg"); //������Ƶ����

        audioSrc.volume = volume; //��������
        audioSrc.clip = bg; //������Ƶ����
        audioSrc.Play(); //������Ƶ
    }

    // Update is called once per frame
    void Update()
    {

    }
}
