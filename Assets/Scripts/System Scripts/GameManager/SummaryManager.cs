using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryManager : MonoBehaviour
{
///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    public GameObject prefObj;
    public GameObject victAnim;

    void Start()
    {
        prefObj = GameObject.FindGameObjectWithTag("PreferenceStorage");
        SetTheScene();
    }

    public void SetTheScene()
    {
        switch (prefObj.GetComponent<PreferenceStorage>().bossValues) {
            case 0:
                victAnim.GetComponent<Animator>().SetTrigger("Endless");
                break;
            case 1:
                victAnim.GetComponent<Animator>().SetTrigger("UchihASCII_Boss");
                break;
            case 2:
                victAnim.GetComponent<Animator>().SetTrigger("UzumASCII_Boss");
                break;
            case 3:
                victAnim.GetComponent<Animator>().SetTrigger("UchihASCII_Boss_Failure");
                break;
            case 4:
                victAnim.GetComponent<Animator>().SetTrigger("UzumASCII_Boss_Failure");
                break;
        }
        #region bin?
        /*if (prefObj.GetComponent<PreferenceStorage>().bossValues == 0) //Endless
        {
            victAnim.GetComponent<Animator>().SetTrigger("Endless");
        }
        else if (prefObj.GetComponent<PreferenceStorage>().bossValues == 1) //UchihASCII_Boss
        {
            victAnim.GetComponent<Animator>().SetTrigger("UchihASCII_Boss");
        }
        else if (prefObj.GetComponent<PreferenceStorage>().bossValues == 2) //UzumASCII_Boss
        {
            victAnim.GetComponent<Animator>().SetTrigger("UzumASCII_Boss");
        }
        else if (prefObj.GetComponent<PreferenceStorage>().bossValues == 3) //UchihASCII_Boss_Failure
        {
            victAnim.GetComponent<Animator>().SetTrigger("UchihASCII_Boss_Failure");
        }
        else if (prefObj.GetComponent<PreferenceStorage>().bossValues == 4) //UzumASCII_Boss_Failure
        {
            victAnim.GetComponent<Animator>().SetTrigger("UzumASCII_Boss_Failure");
        }*/
        #endregion
    }
}