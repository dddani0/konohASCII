using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSelectionButtonBehaviour : MonoBehaviour
{
///////////////////////////////
UNNECCESSARY! UNNECCESSARY!
///////////////////////////////

    public GameObject manager;
    public GameObject[] weaponSelectorBoss;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager");
        manager.GetComponent<Manager>().ninja_UI_Weapons[manager.GetComponent<Manager>().selected_Weapon_Number].SetActive(true);
    }
    private void Update()
    {
        ButtonStateBoss();
    }
    public void ButtonStateBoss()
    {
        if (manager.GetComponent<Manager>().selected_Weapon_Number == (manager.GetComponent<Manager>().ninja_UI_Weapons.Count - 1))
        {
            weaponSelectorBoss[0].SetActive(false);
        }
        if (manager.GetComponent<Manager>().selected_Weapon_Number < (manager.GetComponent<Manager>().ninja_UI_Weapons.Count - 1))
        {
            weaponSelectorBoss[0].SetActive(true);
        }
        if (manager.GetComponent<Manager>().selected_Weapon_Number == (manager.GetComponent<Manager>().ninja_UI_Weapons.Count - manager.GetComponent<Manager>().ninja_UI_Weapons.Count))
        {
            weaponSelectorBoss[1].SetActive(false);
        }
        if (manager.GetComponent<Manager>().selected_Weapon_Number > (manager.GetComponent<Manager>().ninja_UI_Weapons.Count - manager.GetComponent<Manager>().ninja_UI_Weapons.Count))
        {
            weaponSelectorBoss[1].SetActive(true);
        }
    }
    public void WValueUp()
    {
        manager.GetComponent<Manager>().ninja_UI_Weapons[manager.GetComponent<Manager>().selected_Weapon_Number].SetActive(false);
        manager.GetComponent<Manager>().selected_Weapon_Number++;
        manager.GetComponent<Manager>().ninja_UI_Weapons[manager.GetComponent<Manager>().selected_Weapon_Number].SetActive(true);
    }
    public void WValueDown()
    {
        manager.GetComponent<Manager>().ninja_UI_Weapons[manager.GetComponent<Manager>().selected_Weapon_Number].SetActive(false);
        manager.GetComponent<Manager>().selected_Weapon_Number--;
        manager.GetComponent<Manager>().ninja_UI_Weapons[manager.GetComponent<Manager>().selected_Weapon_Number].SetActive(true);
    }
}
