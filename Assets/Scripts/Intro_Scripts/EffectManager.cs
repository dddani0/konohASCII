using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectManager : MonoBehaviour
{
    [Header("In the Intro sequence, this will be instantiated at player's position")]
    [Header("Player is in the UI, note for later!")]
    [Header("")]
    public GameObject smokeBombParticle;
    public Transform smokeBombTransform;
    [Header("Sound for when the Fuma Shuriken Spins")]
    [Header("")]
    public AudioSource fumaShurikenSpinningSFX;
    [Header("Sound for when uzumASCII uses the smokebomb")]
    [Header("")]
    public AudioSource poofSFX;
    [Header("Sound for when uzumASCII runs")]
    [Header("")]
    public AudioSource footStepSFX;
    [Header("Transitioning to the menu, access transition functions")]
    [Header("")]
    public GameObject transition;

    public void SmokeBomb()
    {
        Instantiate(smokeBombParticle, smokeBombTransform.position, Quaternion.identity);
    }
    public void ThrowingShuriken()
    {
        fumaShurikenSpinningSFX.Play();
    }
    public void PoofSFX()
    {
        poofSFX.Play();
    }
    public void FootStepSFX()
    {
        footStepSFX.Play();
    }
    public void TransitionToMenu()
    {
        transition.GetComponent<TransitionBehaviour>().TransitionIn();
    }
    public void TransitionToMenuScene()
    {
        SceneManager.LoadScene(1);
    }
}