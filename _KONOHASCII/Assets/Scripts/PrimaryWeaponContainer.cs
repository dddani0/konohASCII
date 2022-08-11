using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PrimaryWeaponContainer : MonoBehaviour
{
    public WeaponTemplate primaryWeapon;
    [Space] public SpriteRenderer weaponSprite;

    private void Start()
    {
        FetchRudimentaryValues();
    }

    private void FetchRudimentaryValues()
    {
        weaponSprite = GetComponentInChildren<SpriteRenderer>();
        weaponSprite.sprite = primaryWeapon.weaponSprite;
    }
}