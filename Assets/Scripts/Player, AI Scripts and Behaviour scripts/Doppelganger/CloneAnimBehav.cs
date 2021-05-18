using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CloneAnimBehav : MonoBehaviour
{
    [Header("Access the GameManager")]
    public GameObject gmanager;

    private void Start()
    {
        //gmanager = GetComponentInParent<CloneBehav>().game_Manager;
    }
    public void PoofParticle()
    {
        //Instantiate(gmanager.GetComponent<Manager>().particle_effects[1], transform.position, Quaternion.identity);
    }
    public void CloneJutsuSpawnSFX()
    {
        //gmanager.GetComponent<Manager>().sound_Effects[10].Play();
    }
    public void FistAttack()
    {
        GetComponentInParent<CloneBehav>().Punch_Attack();
    }
    public void DestroyGameObject()
    {
        //GetComponentInParent<CloneBehav>().DestroyGMBJCT();
    }
    public void RestoreValue()
    {
        //switch (GetComponentInParent<CloneBehav>().current_Loaded_Scene_Name != "UchihASCIIBoss")
        //{
        //    case true:
        //        GetComponentInParent<CloneBehav>().player_GameObject.GetComponent<PlayerScriptUzumASCII>().cloneCount--;
        //        break;
        //    case false:
        //        GetComponentInParent<CloneBehav>().boss.GetComponent<UzumASCIIBossBehav>().cloneCount--;
        //        break;
        //}
        /*if (GetComponentInParent<CloneBehav>().sceneName != "UchihASCIIBoss")
        {
            GetComponentInParent<CloneBehav>().player.GetComponent<PlayerScriptUzumASCII>().cloneCount--;
        }else
        {
            GetComponentInParent<CloneBehav>().boss.GetComponent<UzumASCIIBossBehav>().cloneCount--;
        }*/
    }
    public void CloneDestructionSFX()
    {
        //gmanager.GetComponent<Manager>().sound_Effects[11].Play();
    }

    public void FootStepSFX()
    {
        //gmanager.GetComponent<Manager>().sound_Effects[5].Play();
    }

    public void PunchSFX()
    {
        //gmanager.GetComponent<Manager>().sound_Effects[28].Play();
    }
    public void UnFreezeConst()
    {
  //      GetComponentInParent<CloneBehav>().cBody.constraints = RigidbodyConstraints2D.None;
    //    GetComponentInParent<CloneBehav>().cBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void FreezeConst()
    {
//        GetComponentInParent<CloneBehav>().cBody.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    public void SetCloneForNejisMind()
    {
        //if (GetComponentInParent<CloneBehav>().current_Loaded_Scene_Name == "UzumASCIIBoss")
        //{
        //    GetComponentInParent<CloneBehav>().boss.GetComponent<HyugASCIIBossScript>().SearchForClone(); //Make the clone change Hyugascii behav
        //}
    }

    public void UnSetClone()
    {
        //if (GetComponentInParent<CloneBehav>().current_Loaded_Scene_Name != "UzumASCIIBoss")
        //{
        //    for (int i = 0; i < GetComponentInParent<CloneBehav>().cloneModes.Length; i++)
        //    {
        //        GetComponentInParent<CloneBehav>().cloneModes[i].SetActive(true);
        //    }
        //    GetComponentInParent<CloneBehav>().cloneModeEnab.GetComponent<CloneModeShaderManager>().DisableObject(); // "Disable"
        //}
        //else /*(GetComponentInParent<CloneBehav>().sceneName == "UchihASCIIBoss")*/
        //{
        //    GetComponentInParent<CloneBehav>().boss.GetComponent<HyugASCIIBossScript>().cloneActive = false;
        //    for (int i = 0; i < GetComponentInParent<CloneBehav>().cloneModes.Length; i++)
        //    {
        //        GetComponentInParent<CloneBehav>().cloneModes[i].SetActive(true);
        //    }
        //    GetComponentInParent<CloneBehav>().cloneModeEnab.GetComponent<CloneModeShaderManager>().DisableObject(); // "Disable"
        //}
    }

    public void DisableCloneUI()
    {
       // GetComponentInParent<CloneBehav>().cloneModeEnab.GetComponent<Animator>().SetTrigger("Dis");
    }
}