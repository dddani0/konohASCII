using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventForZaku : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject stepCol;

    ZakuBehaviour zBehav;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        zBehav = GetComponentInParent<ZakuBehaviour>(); 
    }
    public void FireProjectile()
    {
        zBehav.ShootProjectileThisForChildrenAnimationEventHardCodeGang(); //Fucking Idiot
        //Instantiate(zBehav.gameManager.GetComponent<Manager>().weapons[5], zBehav.attackPos.position,  zBehav.attackPos.transform.rotation);
    }

    public void DestroyStepCol()
    {
        Destroy(stepCol); //Foolish mortal! *dies* Imagine getting owned by a gay 
    }

    public void EarnPoints()
    {
        if (gameManager.GetComponent<Manager>().current_Loaded_Scene_Name != "NarutoTutorial" && gameManager.GetComponent<Manager>().current_Loaded_Scene_Name != "SasukeTutorial")
        {
            gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().score += 35f;
            gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().medallion++;
        }
    }

    public void HandSeal()
    {
        gameManager.GetComponent<Manager>().sound_Effects[17].Play();
    }
}