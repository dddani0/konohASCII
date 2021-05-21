using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionBehaviour : MonoBehaviour
{
///////////////////////////////
UNNECCESSARY? UNNCCESSARY?
///////////////////////////////

    public Animator transAnim;
    public String sceneName;
    public AudioSource scrollSound;

    void Start()
    {
        scrollSound = GetComponent<AudioSource>();
        transAnim = GetComponent<Animator>();
        TransitionOutInNewScene();
    }
    public void TransitionIn()
    {
        transAnim.SetTrigger("FadeIn");
    }
    public void TransitionOut()
    {
        transAnim.SetTrigger("FadeOut");
    }
    public void TransitionOutInNewScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName != "Intro")
        {
            TransitionOut();
        }
    }
    public void ScrollSoundSFX()
    {
        scrollSound.Play();
    }

    public void ResetTimeValue()
    {
        Time.timeScale = 1.0f; //Feast your eyes on the power of the TIME
    }
}