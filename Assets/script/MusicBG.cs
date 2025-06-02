using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBG : MonoBehaviour
{
    private AudioSource audioSrc; 
    private AudioClip bg; 
    public float volume = 0.5f; 

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>(); 
        bg = Resources.Load<AudioClip>("bg"); 

        audioSrc.volume = volume; 
        audioSrc.clip = bg; 
        audioSrc.Play();
    }


    void Update()
    {

    }
}
