﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    [FormerlySerializedAs("playerAnimation")] [Header("Resources")] 
    public PlayerAnimation playeranimation;
    public PlayerActionAttribute playerActionAttribute;
    [Space(20f)]
    [Header("Basic movement agility")]
    [Range(1f,5f)]public float groundcheck_radius;
    [Space]
    public bool canJump = true;
    public Transform groundcheck_position;
    [Space]
    public float movementSpeed;
    [Space]
    public float jump_force;
    [Space(20f)]
    [Header("Advanced movement agility")]
    [Range(1f,5f)]public float wallcheck_height_size;
    [Range(1f,5f)]public float wallcheck_width_size;
    [Space]
    public bool canGrip;
    public bool isGripped = false;
    private bool isGrippedActionTaken; //This is a discard boolean: i need to call the input from outside fixed update
    private bool isJumpActionTaken;    //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    public Transform wallcheck_position;
    [Space(20f)]
    [Header("Layermasks and button mapping")]
    public LayerMask groundlayer;
    public LayerMask walllayer;
    [Space]
    public KeyCode jump_keycode;
    public KeyCode grip_keycode;
    
    // Start is called before the first frame update
    void Start()
    {
        playeranimation = GetComponentInChildren<PlayerAnimation>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        InputDetection();
        WallGrip();
    }

    private void FixedUpdate()
    {
        Basic_Agility();
        Advanced_Agility();
    }

    private void InputDetection()
    {
        isJumpActionTaken = Input.GetKey(jump_keycode) && canJump;
        isGrippedActionTaken = Input.GetKeyDown(grip_keycode) && !playerActionAttribute.isBusy && canGrip;
    }
    
    private void Basic_Agility()
    {
        //Basic character mobility
        //-Movement
        //-Jump
        
        //Movement
        switch (playerActionAttribute.isBusy)
        {
            case true:
                rigidbody2D.velocity = new Vector2(0, 0);

                break;
            case false:
                rigidbody2D.velocity = new Vector2(movementSpeed * Time.fixedDeltaTime * Input.GetAxisRaw("Horizontal"), rigidbody2D.velocity.y);
                break;
        }
        //ground check for jumping, only when not in the air or is not gripped on wall
        if (!isGripped)
            GroundCheck();
        //Jump
        if (isJumpActionTaken)
        {
            playeranimation.SetAnimationState("jump",playeranimation.default_animator);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jump_force * Time.fixedDeltaTime);
            if (isGripped)
            {
                isGripped = false;
                playeranimation.gameObject.transform.localScale = new Vector2(playeranimation.gameObject.transform.localScale.x,1f);
            }
            ChangeRigidbodyState(isGripped); 
        }
    }

    private void Advanced_Agility()
    {
        //Advanced character mobility
        //-Wall walk
        //-Wall jump
        
        WallCheck();
        playeranimation.SetAnimationState("gripped",isGripped,playeranimation.default_animator);
    }

    private void WallGrip()
    {
        if (isGrippedActionTaken)
        {
            ChangeRigidbodyState(true);
            canGrip = false;
            isGripped = true;
            canJump = true;
            Collider2D[] wallcol = Physics2D.OverlapBoxAll(wallcheck_position.position,
                new Vector2(wallcheck_width_size, wallcheck_height_size),
                wallcheck_position.rotation.x, walllayer);
            Transform wallpos = null;
            if (wallcol.Length > 0)
                wallpos = wallcol[0].transform;
            switch (wallpos.position.x > this.gameObject.transform.position.x)
            {
                case true:
                    playeranimation.gameObject.transform.localScale = new Vector2(playeranimation.gameObject.transform.localScale.x,-1f);
                    break;
                case false:
                    playeranimation.gameObject.transform.localScale = new Vector2(playeranimation.gameObject.transform.localScale.x,1f);
                    break;
            }
        }
    }

    private void GroundCheck()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(groundcheck_position.position, groundcheck_radius, groundlayer);
        canJump = col.Length > 0;
        playeranimation.SetAnimationState("onGround",canJump,playeranimation.default_animator);
    }

    private void WallCheck()
    {
        Collider2D[] wallcol = Physics2D.OverlapBoxAll(wallcheck_position.position, new Vector2(wallcheck_width_size, wallcheck_height_size),
            wallcheck_position.rotation.x, walllayer);
        canGrip = wallcol.Length > 0 && canJump == false;
    }

    public void ChangeRigidbodyState(bool freeze)
    {
        switch (freeze)
        {
            case true:
                rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                break;
            case false:
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                break;
        }
    }

    public void ChangeRigidbodyState(bool freezex, bool freezey)
    {
        switch (freezex)
        {
            case true:
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                break;
            case false:
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;
        }
        switch (freezey)
        {
            case true:
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
                break;
            case false:
                rigidbody2D.constraints = RigidbodyConstraints2D.None;
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0.02f);
        Gizmos.DrawWireSphere(groundcheck_position.position,groundcheck_radius);
        Gizmos.DrawWireCube(wallcheck_position.position,new Vector3(wallcheck_width_size,wallcheck_height_size));
    }
}
