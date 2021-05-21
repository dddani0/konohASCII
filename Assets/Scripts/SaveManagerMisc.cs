using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManagerMisc : MonoBehaviour
{
///////////////////////////////
UNNECESSARY? UNNECESSARY?
///////////////////////////////

    public GameObject gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
    }

    public void KunaiWindUp()
    {
        gamemanager.GetComponent<Manager>().sound_Effects[1].Play();
    }

    public void KunaiHit()
    {
        gamemanager.GetComponent<Manager>().sound_Effects[0].Play();
    }
}
