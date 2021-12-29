using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByakuganController : MonoBehaviour
{
    public GameObject gamemanager;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
    }
    
    public void ActivatingByakugan()
    {
        gamemanager.GetComponent<Manager>().sound_Effects[25].Play();
    }

    public void DeactByakugan()
    {
        gamemanager.GetComponent<Manager>().sound_Effects[27].Play();
    }
}
