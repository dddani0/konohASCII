using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceSelectionScrollBehaviour : MonoBehaviour
{
///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    public GameObject gamemanager;

    Animator anControll;

    private void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        anControll = GetComponent<Animator>();
    }

    public void ScrollBackToMainMenu()
    {
        anControll.SetTrigger("Back");
    }

    public void ScrollSFX()
    {
        gamemanager.GetComponent<Manager>().sound_Effects[0].Play();
    }
}
