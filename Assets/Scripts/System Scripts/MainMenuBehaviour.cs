using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    public GameObject gamemanager;
    Animator anim;

    private void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        anim = GetComponent<Animator>();
    }

    public void Scroll_Transition_In() //Call when I want the main menu thingy to appears
    {
        anim.SetTrigger("scrollInTrigger");
    }
    public void Scroll_Transition_Out() //Call when I want the main menu thingy to disappear
    {
        anim.SetTrigger("scrollOutTrigger");
    }

    public void Scroll_SFX()
    {
        gamemanager.GetComponent<Manager>().sound_Effects[1].Play();
    }
}
