using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDispenser : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////
///////////////////////////////
//UNNECCESSARY? UNNECCESSARY?
///////////////////////////////

    [Header("Access the GameManager")]
    public GameObject gamemanager;
    [Header("Dispenser transform positions")]
    public Transform[] dispPosition;
    [Header("Dispenser activated state")]
    public bool activated;
    [Space]
    public bool coroutineStarted;

    void Start()
    {
        activated = false;
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
    }

    void Update()
    {
        if (activated)
        {
            DispenseWeapons();
        }
    }

    public void DispenseWeapons()
    {
        StartCoroutine(DispenseWeaponsAtXRate());
    }
    IEnumerator DispenseWeaponsAtXRate()
    {
        WaitForSeconds waitfrscnds = new WaitForSeconds(0.25f); //Na mondjad
        if (!coroutineStarted)
        {
            for (int i = 0; i < dispPosition.Length; i++)
            {
                Instantiate(gamemanager.GetComponent<Manager>().weapons[1], dispPosition[i].position, dispPosition[i].rotation);
                coroutineStarted = true;
                yield return waitfrscnds; //nigga what in tarnation...?
                coroutineStarted = false;
            }
        }
    }
    public void ActivateDispenser()
    {
        activated = true;
    }

    public void DeActivateDispenser()
    {
        activated = false;
    }
}
