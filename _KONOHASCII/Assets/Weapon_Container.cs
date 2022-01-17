using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Container : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [Space]
    public Weapon container_weapon;
    [Space] 
    public Sprite weapon_sprite;
    public SpriteRenderer weapon_sprite_renderer;
    public Animator weapon_animator;
    public AnimatorOverrideController weapon_override_controller;
    [Space] 
    private float weapon_speed;
    private int weapon_range;
    [Space] 
    private int isMovingRight = 1;

    private void Start()
    {
        weapon_sprite_renderer = GetComponentInChildren<SpriteRenderer>();
        weapon_animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (container_weapon != null)
            if (container_weapon.isRange)
                Throwable_Weapon(container_weapon,weapon_range,weapon_speed);
    }

    public void AssingNewWeapon(Weapon _weapon, int _ismovingright)
    {
        container_weapon = _weapon;
        weapon_sprite = _weapon.weapon_sprite;
        weapon_range = _weapon.weapon_range;
        weapon_speed = _weapon.weapon_speed;
        weapon_sprite = _weapon.weapon_sprite;
        isMovingRight = _ismovingright;
    }
    
    private void Update()
    {
        if (container_weapon != null)
            switch (container_weapon.isRange)
            {case true: /*Throwable_Weapon();*/ break; case false: /* Melee_Weapon();*/ break;}
    }

    private void LateUpdate()
    {
        weapon_sprite_renderer.sprite = weapon_sprite;
    }

    private void Throwable_Weapon(Weapon _weapon, int _range, float _speed)
    {
        rigidbody.velocity = new Vector2( Time.fixedDeltaTime * _speed * isMovingRight, rigidbody.velocity.y);
    }

    private void Melee_Weapon()
    {
        
    }
}
