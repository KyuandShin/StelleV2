using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioSource audioSrc;

    public static AudioClip attack;
    public static AudioClip step;
    public static AudioClip pickup;
    public static AudioClip healspell;
    public static AudioClip enemyattack;
    public static AudioClip hurt;
    public static AudioClip bg;

    // Start is called before the first frame update

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        attack = Resources.Load<AudioClip>("attack");
        step = Resources.Load<AudioClip>("step");
        pickup = Resources.Load<AudioClip>("pickup");
        healspell = Resources.Load<AudioClip>("healspell");
        enemyattack = Resources.Load<AudioClip>("enemyattack");
        hurt = Resources.Load<AudioClip>("hurt");



    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlayPlayerAttackClip()
    {
        audioSrc.PlayOneShot(attack);
    }

    public static void PlayEnemyAttackClip()
    {
        audioSrc.PlayOneShot(enemyattack);
    }

    public static void PlayPickUpClip()
    {
        audioSrc.PlayOneShot(pickup);
    }

    public static void PlayPlayerStepClip()
    {
        audioSrc.PlayOneShot(step);
    }


    public static void PlayHealSpellClip()
    {
        audioSrc.PlayOneShot(healspell);
    }

    public static void PlayHurtClip()
    {
        audioSrc.PlayOneShot(hurt);
    }

}


