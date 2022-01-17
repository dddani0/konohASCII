using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    [FormerlySerializedAs("playerAnimation")] [Header("Resources")] 
    public PlayerAnimation playeranimation;
    public PlayerActionAttribute playeractionattribute;
    [Space(20f)]
    [Header("Basic movement agility")]
    [Range(1f,5f)]public float groundcheck_radius;
    [Space]
    public bool canJump = true;
    public Transform groundcheck_position;
    [Space]
    public float movement_speed;
    [Space]
    public float jump_force;
    [Space(20f)]
    [Header("Advanced movement agility")]
    [Range(1f,5f)]public float wallcheck_height_size;
    [Range(1f,5f)]public float wallcheck_width_size;
    [Space]
    public bool canGrip;
    public bool isGripped = false;
    private bool isGrippedActionTaken;
    private bool isJumpActionTaken;    
    public Transform wallcheck_position;
    [Space(20f)]
    [Header("Layermasks and button mapping")]
    public LayerMask ground_layer;
    public LayerMask wall_layer;
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
        isGrippedActionTaken = Input.GetKeyDown(grip_keycode) && !playeractionattribute.isBusy && canGrip;
    }
    
    private void Basic_Agility()
    {
        //Basic character mobility
        //-Movement
        //-Jump
        
        //Movement
        switch (playeractionattribute.isBusy)
        {
            case true:
                rigidbody2D.velocity = new Vector2(0, 0);

                break;
            case false:
                rigidbody2D.velocity = new Vector2(movement_speed * Time.fixedDeltaTime * Input.GetAxisRaw("Horizontal"), rigidbody2D.velocity.y);
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
            ChangeRigidbodyState(isGripped,rigidbody2D); 
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
            //rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
            ChangeRigidbodyState(true,rigidbody2D);
            canGrip = false;
            isGripped = true;
            canJump = true;
            isGrippedActionTaken = false;
            Collider2D[] wallcol = Physics2D.OverlapBoxAll(wallcheck_position.position,
                new Vector2(wallcheck_width_size, wallcheck_height_size),
                wallcheck_position.rotation.x, wall_layer);
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
        //Checks jump condition
        Collider2D[] col = Physics2D.OverlapCircleAll(groundcheck_position.position, groundcheck_radius, ground_layer);
        canJump = col.Length > 0;
        playeranimation.SetAnimationState("onGround",canJump,playeranimation.default_animator);
    }

    private void WallCheck()
    {
        //Checks wallgrip condition
        Collider2D[] wallcol = Physics2D.OverlapBoxAll(wallcheck_position.position, new Vector2(wallcheck_width_size, wallcheck_height_size),
            wallcheck_position.rotation.x, wall_layer);
        canGrip = wallcol.Length > 0 && canJump == false;
        if (!isGripped)
            ChangeRigidbodyState(false,false,rigidbody2D);
    }

    public void ChangeRigidbodyState(bool freeze, Rigidbody2D _rigidbody)
    {
        //It only works if "wallgripped" animation state isn't agged with "action"
        switch (freeze)
        {
            case true:
                var constraints = _rigidbody.constraints;
                constraints = RigidbodyConstraints2D.FreezePosition;
                _rigidbody.constraints = constraints;
                break;
            case false:
                _rigidbody.constraints = RigidbodyConstraints2D.None;
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;
        }
    }

    public void ChangeRigidbodyState(bool _freeze_x, bool _freeze_y, Rigidbody2D _rigidbody)
    {
        bool _isotheraxisfrozen = false;
        switch (_freeze_x)
        {
            case true:
                _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
                break;
            case false:
                _rigidbody.constraints = RigidbodyConstraints2D.None;
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                break;
        }
        switch (_freeze_y)
        {
            case true:
                _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
                break;
            case false:
                if (_freeze_x)
                    _isotheraxisfrozen = true;
                _rigidbody.constraints = RigidbodyConstraints2D.None;
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (_isotheraxisfrozen)
                    _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
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
