using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UzumASCII_Slide_manager : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    public GameObject gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
    }

    public void FootStepSfX()
    {
        //gamemanager.GetComponent<Manager>().sound_Effects[0].Play();
    }

    public void FumaShurikenThrowSFX()
    {
        //gamemanager.GetComponent<Manager>().sound_Effects[1].Play();
    }
    public void FumaShurikenSpinSFX()
    {
        //gamemanager.GetComponent<Manager>().sound_Effects[2].Play();
    }
}
