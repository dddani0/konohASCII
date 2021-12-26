using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScriptUzumASCIIAnimation : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////
///////////////////////////////
//IN PROGRESS! IN PROGRESS!
///////////////////////////////

    /// <summary>
    /// Handles all animations and miscs
    /// </summary>
    Animator uAnim;
    private void Start()
    {
        uAnim = GetComponent<Animator>();
    }
    private void Update()
    {
        uAnim.SetFloat("health", GetComponentInParent<PlayerScriptUzumASCII>().current_Health);
    }
    public void FootStep()
    {
        //GetComponentInParent<PlayerScriptUzumASCII>().gManager.GetComponent<Manager>().sound_Effects[5].Play();
    }
    public void ToSummary()
    {
        StartCoroutine(WaitAfterDeath());
    }
    IEnumerator WaitAfterDeath()
    {
        //GetComponentInParent<PlayerScriptUzumASCII>().gManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(6);
    }

    public void HandSealSFX()
    {
        //GetComponentInParent<PlayerScriptUzumASCII>().gManager.GetComponent<Manager>().sound_Effects[17].Play();
    }

    public void CloneJutsuHandSeal()
    {
        //GetComponentInParent<PlayerScriptUzumASCII>().gManager.GetComponent<Manager>().sound_Effects[18].Play();
    }

    public void AttackSoundEffect()
    {

    }
}
