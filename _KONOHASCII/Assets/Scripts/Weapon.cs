using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public string weapon_name;
    [Space]
    public Sprite weapon_sprite;
    public Animator default_weapon_animator;
    public AnimatorOverrideController weapon_animator_controller;
    [Space] 
    public int damage;
    [Space] 
    public bool isRange;
    public int weapon_range;
    public float weapon_speed;
    [Space] 
    public bool is_weapon_flying;
}
