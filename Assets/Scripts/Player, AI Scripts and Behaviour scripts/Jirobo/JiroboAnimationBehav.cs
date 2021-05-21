using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiroboAnimationBehav : MonoBehaviour
{
///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    public GameObject pref;
    public GameObject gameManager;

    private void Start()
    {
        pref = GameObject.FindGameObjectWithTag("PreferenceStorage");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }
    public void AttackWithAPunch()
    {
        GetComponentInParent<JiroboBehaviour>().AttackPunch();
    }

    public void DestroyParentHitbox()
    {
        GetComponentInParent<BoxCollider2D>().enabled = false;
    }

    public void DieSFX()
    {
        //gameManager.GetComponent<Manager>().sound_Effects[16].Play();
    }

    public void GetPoints()
    {
        if (gameManager.GetComponent<Manager>().current_Loaded_Scene_Name != "NarutoTutorial" && gameManager.GetComponent<Manager>().current_Loaded_Scene_Name != "SasukeTutorial")
        {
            pref.GetComponent<PreferenceStorage>().score += 35f;
            pref.GetComponent<PreferenceStorage>().medallion++;
        }
    }

    public void FootStep()
    {
        //gameManager.GetComponent<Manager>().sound_Effects[15].Play();
    }
}
