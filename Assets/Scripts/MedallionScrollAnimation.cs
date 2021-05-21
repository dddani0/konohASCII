using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedallionScrollAnimation : MonoBehaviour
{
///////////////////////////////
UNNECESSARY? UNNECESSARY?
///////////////////////////////

    public GameObject gamemanager;

    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
    }

public void ScrollSFX()
    {
        gamemanager.GetComponent<Manager>().sound_Effects[5].Play();
    }
}
