using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingScrollBehaviour : MonoBehaviour
{
    public GameObject gameManager;

    Animator scrollBehav;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        scrollBehav = GetComponent<Animator>();
    }
    public void ScrollOn()
    {
        scrollBehav.SetTrigger("Activate");
    }
    public void ScrollBack()
    {
        scrollBehav.SetTrigger("DeActivate");
    }

    public void ScrollSFX()
    {
        gameManager.GetComponent<Manager>().sound_Effects[1].Play();
    }
}
