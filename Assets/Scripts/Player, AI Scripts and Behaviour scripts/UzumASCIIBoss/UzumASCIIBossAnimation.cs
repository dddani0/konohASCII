using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UzumASCIIBossAnimation : MonoBehaviour
{
///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    public GameObject gamemanager;

    private void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
    }

    public void PunchAttack()
    {
        GetComponentInParent<UzumASCIIBossBehav>().PunchDamage();
    }

    public void SmokeBomb()
    {
        Instantiate(GetComponentInParent<UzumASCIIBossBehav>().gamemanager.GetComponent<Manager>().particle_effects[1], transform.position, Quaternion.identity);
    }

    public void CleanUp()
    {
        GetComponentInParent<UzumASCIIBossBehav>().DestCleanUp();
    }

    public void PoofSFX()
    {
        GetComponentInParent<UzumASCIIBossBehav>().gamemanager.GetComponent<Manager>().sound_Effects[11].Play();
    }

    public void DisableCol()
    {
        GetComponentInParent<BoxCollider2D>().enabled = false;
    }
    public void EnableCol()
    {
        GetComponentInParent<BoxCollider2D>().enabled = true;
    }
    public void WeaponShower()
    {
        for (int i = 0; i < gamemanager.GetComponent<Manager>().weaponSpots.Length; i++)
        {
            Instantiate(gamemanager.GetComponent<Manager>().weapons[Random.Range(0, 4)], gamemanager.GetComponent<Manager>().weaponSpots[i].position, gamemanager.GetComponent<Manager>().weaponSpots[i].rotation);
        }
    }
    public void PrepareForSecondPhase()
    {
        GetComponentInParent<UzumASCIIBossBehav>().boss.SetActive(true);
    }

    public void EndBossMode()
    {
        gamemanager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().bossValues = 1;
        StartCoroutine(ToSummaryDelay());
        IEnumerator ToSummaryDelay()
        {
            GetComponentInParent<UzumASCIIBossBehav>().gamemanager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
            yield return new WaitForSeconds(0.75f);
            SceneManager.LoadScene(6);
        }
    }

    public void SpawnAClone()
    {
        Instantiate(gamemanager.GetComponent<Manager>().weapons[gamemanager.GetComponent<Manager>().weapons.Length - 1], GetComponentInParent<UzumASCIIBossBehav>().cloneSpot.position, Quaternion.identity); //Spawn a clone in the middle of the arena
    }
    public void Rasengan()
    {
        Instantiate(gamemanager.GetComponent<Manager>().weapons[4], GetComponentInParent<UzumASCIIBossBehav>().rasenganSpot.position, GetComponentInParent<UzumASCIIBossBehav>().rasenganSpot.rotation);
    }
}