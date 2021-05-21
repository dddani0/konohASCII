using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptUchihASCIIAnimation : MonoBehaviour
{
///////////////////////////////
UNNECESSARY? UNNECESSARY?
///////////////////////////////

    public void ChidoriAttack()
    {
        GetComponentInParent<PlayerScriptUchihASCII>().ChidoriJutsuTrig();
    }
    public void FireBallAttack()
    {
        GetComponentInParent<PlayerScriptUchihASCII>().FireBallJutsuTrig();
    }

    public void MimicFootstep()
    {
        GetComponentInParent<PlayerScriptUchihASCII>().gameManager.GetComponent<Manager>().sound_Effects[5].Play();
    }

    public void FireBallJutsuSFX()
    {
        GetComponentInParent<PlayerScriptUchihASCII>().gameManager.GetComponent<Manager>().sound_Effects[19].Play();
    }

    public void HandsealSFX()
    {
        GetComponentInParent<PlayerScriptUchihASCII>().gameManager.GetComponent<Manager>().sound_Effects[17].Play();
    }

    public void ChidoriWarmUp()
    {
        GetComponentInParent<PlayerScriptUchihASCII>().gameManager.GetComponent<Manager>().sound_Effects[22].Play();
    }
}