﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionAttribute : MonoBehaviour
{
    [Header("Resources")]
    public PlayerMovement playermovement;
    public PlayerAnimation playeranimation;
    [Space(20f)]
    [Header("Weapon_Attribute")]
    public Weapon current_weapon;
    [Space] 
    public Transform[] weapon_position;
    [Space] 
    public int weapon_quantity;
    [Space(20f)] 
    [Header("Health")] 
    public float maximum_health;
    [SerializeField] private float current_health;
    [Space(20f)] 
    [Header("Chakra")] 
    public float chakra_maximum;
    [SerializeField] private float chakra_current;
    [Space(20f)] 
    [Header("Brakes")] 
    public bool isBusy;
    public bool isStaggered;
    [Space(20f)] 
    [Header("Layermasks and Button mapping")]
    public LayerMask enemylayer;
    [Space]
    public KeyCode attack_keycode;
    public KeyCode rangeattack_keycode; 

    void Update()
    {
        CheckBusyBooleanStatement();
        CQCAttack();
        RangeAttack();
    }

    private void CQCAttack()
    {
        if (Input.GetKey(attack_keycode) && !isBusy)
            playeranimation.SetAnimationState("attack",playeranimation.default_animator);
    }

    private void RangeAttack()
    {
        if (Input.GetKey(rangeattack_keycode) && !isBusy && !playeranimation.default_animator.GetCurrentAnimatorStateInfo(0).IsName("wallgrip"))
        {
            playeranimation.SetAnimationState("range_attack",playeranimation.default_animator);
            GameObject t_weapon;
            switch (playeranimation.gameObject.transform.localScale.x > 0)
            {
                case true:
                    t_weapon = Instantiate(
                        GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>().weapon_container,
                        weapon_position[0].position, weapon_position[0].rotation);
                    t_weapon.GetComponent<Weapon_Container>().AssingNewWeapon(current_weapon,1);
                    break;
                case false:
                    t_weapon = Instantiate(
                        GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>().weapon_container,
                        weapon_position[1].position, weapon_position[1].rotation);
                    t_weapon.GetComponent<Weapon_Container>().AssingNewWeapon(current_weapon,-1);
                    break;
            }

            
        }
    }

    private void CheckBusyBooleanStatement()
    {
        isBusy = playeranimation.default_animator.GetCurrentAnimatorStateInfo(0).IsTag("Action");
        
        if (isBusy || isStaggered) //freeze if staggered or action is taking place
            playermovement.ChangeRigidbodyState(true,false,playermovement.rigidbody2D);
    }
}