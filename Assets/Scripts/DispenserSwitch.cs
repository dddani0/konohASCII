using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserSwitch : MonoBehaviour
{
///////////////////////////////
UNNECESSARY? UNNECESSARY?
///////////////////////////////

    public GameObject otherBoxcoll;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GetComponentInParent<WeaponDispenser>().activated || collision.CompareTag("Kunai") && !GetComponentInParent<WeaponDispenser>().activated)
        {
            Debug.Log("Activated");
            otherBoxcoll.SetActive(true);
            this.gameObject.SetActive(false);
            GetComponentInParent<WeaponDispenser>().activated = true;
        }
        else if (collision.CompareTag("Player") && GetComponentInParent<WeaponDispenser>().activated) //
        {
            Debug.Log("Deactivated");
            otherBoxcoll.SetActive(true);
            this.gameObject.SetActive(false);
            GetComponentInParent<WeaponDispenser>().activated = false;
        }
    }
}
