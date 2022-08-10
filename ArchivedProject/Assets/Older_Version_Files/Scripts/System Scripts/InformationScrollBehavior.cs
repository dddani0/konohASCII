using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationScrollBehavior : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    public GameObject gamemanager;
    Animator animBehav;
    
    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        animBehav = GetComponent<Animator>();
    }

public void ScrollBack()
    {
        animBehav.SetTrigger("Back");
    }

    public void ScrollSFX()
    {
        gamemanager.GetComponent<Manager>().sound_Effects[0].Play();
    }
}
