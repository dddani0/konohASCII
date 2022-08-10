using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharinganSystem : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    [Header("Access GameObject &")]
    public GameObject gamemanager; //This for Sounds
    [Header("Set time properties")]
    [Range(0,1)]public float timeSlider;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
    }

    public void ActivateSharinganSound() //2
    {
        gamemanager.GetComponent<Manager>().sound_Effects[2].Play();
    }

    public void SlowDownTime()
    {
        Time.timeScale = timeSlider;
    }

    public void DefaultTime() //Let me try somthn //Epic bruh reddit gold moment
    {
        Time.timeScale = 1.0f;
    }

    public void SharinganExhaustedSound() //3
    {
        gamemanager.GetComponent<Manager>().sound_Effects[3].Play();
        Time.timeScale = 1.0f;
    }
}
