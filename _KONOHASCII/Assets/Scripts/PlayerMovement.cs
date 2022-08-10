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
    public PlayerAnimation playerAnimation;

    public PlayerAction playerAction;

    [Space(20f)] [Header("Basic movement agility")] [Range(1f, 5f)]
    public float groundCheckRadius;

    [Space] public bool canJump = true;
    public Transform groundCheckPosition;
    [Space] public float movementSpeed;
    [Space] public float jump_force;

    [Space(20f)] [Header("Advanced movement agility")] [Range(1f, 5f)]
    public float wallcheck_height_size;

    [Range(1f, 5f)] public float wallcheck_width_size;
    [Space] public bool canGrip;
    public bool isGripped = false;
    private bool isGrippedActionTaken;
    private bool isJumpActionTaken;
    public Transform wallcheck_position;

    [Space(20f)] [Header("Layermasks and button mapping")]
    public LayerMask ground_layer;

    public LayerMask wall_layer;
    [Space] public KeyCode jump_keycode;
    public KeyCode grip_keycode;

    // Start is called before the first frame update
    void Start()
    {
        FetchRudimentaryVariables();
    }

    private void Update()
    {
        FetchInput();
        WallGrip();
    }

    private void FixedUpdate()
    {
        BasicMovementAgility();
        AdvancedMovementAgility();
    }

    private void FetchInput()
    {
        isJumpActionTaken = Input.GetKey(jump_keycode) && canJump;
        isGrippedActionTaken = Input.GetKeyDown(grip_keycode) && !playerAction.isBusy && canGrip;
    }

    private void FetchRudimentaryVariables()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerAction = GetComponentInChildren<PlayerAction>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void BasicMovementAgility()
    {
        //Basic character mobility
        //-Movement
        //-Jump

        //Movement
        switch (playerAction.isBusy)
        {
            case true:
                rigidbody2D.velocity = new Vector2(0, 0);

                break;
            case false:
                rigidbody2D.velocity = new Vector2(movementSpeed * Time.fixedDeltaTime * Input.GetAxisRaw("Horizontal"),
                    rigidbody2D.velocity.y);
                break;
        }

        //ground check for jumping, only when not in the air or is not gripped on wall
        if (!isGripped)
            GroundCheck();
        //Jump
        if (isJumpActionTaken)
        {
            playerAnimation.SetAnimationState("jump", playerAnimation.defaultAnimator);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jump_force * Time.fixedDeltaTime);
            if (isGripped)
            {
                isGripped = false;
                playerAnimation.gameObject.transform.localScale =
                    new Vector2(playerAnimation.gameObject.transform.localScale.x, 1f);
            }

            ChangeRigidbodyState(isGripped, rigidbody2D);
        }
    }

    private void AdvancedMovementAgility()
    {
        //Advanced character mobility
        //-Wall jump

        WallCheck();
        playerAnimation.SetAnimationState("gripped", isGripped, playerAnimation.defaultAnimator);
    }

    private void WallGrip()
    {
        if (isGrippedActionTaken)
        {
            //rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
            ChangeRigidbodyState(true, rigidbody2D);
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
                    playerAnimation.gameObject.transform.localScale =
                        new Vector2(playerAnimation.gameObject.transform.localScale.x, 1f);
                    break;
                case false:
                    playerAnimation.gameObject.transform.localScale =
                        new Vector2(playerAnimation.gameObject.transform.localScale.x, -1f);
                    break;
            }
        }
    }

    private void GroundCheck()
    {
        //Checks jump condition
        Collider2D[] col = Physics2D.OverlapCircleAll(groundCheckPosition.position, groundCheckRadius, ground_layer);
        canJump = col.Length > 0;
        playerAnimation.SetAnimationState("onGround", canJump, playerAnimation.defaultAnimator);
    }

    private void WallCheck()
    {
        //Checks wallgrip condition
        Collider2D[] wallcol = Physics2D.OverlapBoxAll(wallcheck_position.position,
            new Vector2(wallcheck_width_size, wallcheck_height_size),
            wallcheck_position.rotation.x, wall_layer);
        canGrip = wallcol.Length > 0 && canJump == false;
        if (!isGripped)
            ChangeRigidbodyState(false, false, rigidbody2D);
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
        Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
        Gizmos.DrawWireCube(wallcheck_position.position, new Vector3(wallcheck_width_size, wallcheck_height_size));
    }
}