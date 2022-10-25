using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Button inputs")] PlayerMovementInput playerMovementInput;

    [Space] public float mouseYAxisInput;
    public float movementAxisInput;
    public float currentMouseInput, maxMouseInput, mouseInputDifference;
    [SerializeField] private bool isPlayerInMotion;
    [SerializeField] private bool isJumpPressed;
    [SerializeField] private bool isWallGrabPressed;
    public Rigidbody2D rigidbody2D;

    [Header("Resources")] public PlayerAnimation playerAnimation;

    public PlayerAction playerAction;


    [Space(20f)] [Header("Basic movement agility")] [Range(1f, 5f)] [SerializeField] [Space]
    public float groundCheckRadius;

    [Space] public bool canJump = true;
    public bool isGrounded;
    public Transform groundCheckPosition;


    /// <summary>
    /// 
    /// </summary>
    [Tooltip("Speed, which the player will progress.")]
    public float maximumSpeed;

    [Space,Tooltip("The maximum speed, the player can reach")] public float peakMovementSpeed;
    [Tooltip("The direction, which the player faces.")]
    private float xDirection;
    private Vector2 desiredVelocity;

    [Tooltip("Maximum speed, which the player can achieve on the ground.")]
    public float maximumLateralGroundAcceleration;

    [Tooltip("Maximum value, which the player will decelerates on the ground.")]
    public float maximumLateralGroundDeceleration;

    [Tooltip("Maximum speed, which the player can achieve in the air")]
    public float maximumAirLateralAcceleration;

    [Tooltip("Maximum speed, which the player will turn on the ground")]
    public float maximumGroundTurnSpeed;

    [Tooltip("Maximum value, which the player will decelerate in the air.")]
    public float maximumAirDeceleration;

    [Tooltip("Turning speed in the air")] public float maximumAirTurnSpeed;

    [SerializeField] private float currentTurnSpeed;
    [SerializeField] private Vector2 currentVelocity;
    [SerializeField] private float currentDeceleration;
    [SerializeField] private float currentAcceleration;

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

    private void Awake()
    {
        playerMovementInput = new PlayerMovementInput();
    }

    // Start is called before the first frame update
    void Start()
    {
        FetchRudimentaryVariables();
    }

    private void Update()
    {
        isPlayerInMotion = DeterminePlayerMotionState();
        desiredVelocity = CalculateDesiredVelocity(xDirection, peakMovementSpeed);
        FetchInput();
        WallGrip();
        GroundCheck();
        AssignAnimationVariables();
    }

    private void FixedUpdate()
    {
        GroundLateralMovement();
    }

    private void AssignAnimationVariables()
    {
        playerAnimation.SetAnimationState("xDirection",int.Parse(xDirection.ToString()),playerAnimation.defaultAnimator);
        playerAnimation.SetAnimationState("currentVelocity",math.abs(currentVelocity.x),playerAnimation.defaultAnimator);
        playerAnimation.SetAnimationState("hasReachedPeakVelocity",math.abs(currentVelocity.x) == (1f * peakMovementSpeed),playerAnimation.defaultAnimator);
    }
    
    private void GroundLateralMovement()
    {
    print(xDirection);
        switch (playerAction.isBusy)
        {
            case true:
                // currentVelocity.x = 0;
                // rigidbody2D.velocity = currentVelocity;
                break;
            case false:
                currentAcceleration = isGrounded ? maximumLateralGroundAcceleration : maximumAirLateralAcceleration;
                currentDeceleration = isGrounded ? maximumLateralGroundDeceleration : maximumAirDeceleration;
                currentTurnSpeed = isGrounded ? maximumGroundTurnSpeed : maximumAirTurnSpeed;

                switch (xDirection != 0)
                {
                    case true:
                        //Checks whether the player direction and the velocity direction matches
                        switch (Mathf.Sign(xDirection) != Mathf.Sign(rigidbody2D.velocity.x))
                        {
                            case true:
                                maximumSpeed = currentTurnSpeed * Time.deltaTime;
                                break;
                            case false:
                                maximumSpeed = currentAcceleration * Time.deltaTime;
                                break;
                        }

                        break;
                    case false:
                        maximumSpeed = currentDeceleration * Time.deltaTime;
                        break;
                }

                currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, desiredVelocity.x, maximumSpeed);
                rigidbody2D.velocity = currentVelocity;
                break;
        }
    }


    private void OnEnable()
    {
        playerMovementInput.Enable();
    }

    private void OnDisable()
    {
        playerMovementInput.Disable();
    }

    public void InputWallGrab(InputAction.CallbackContext context)
    {
        isWallGrabPressed = context.performed;
    }

    public void InputMovement(InputAction.CallbackContext context)
    {
        movementAxisInput = context.ReadValue<Vector2>().x;
        xDirection = movementAxisInput; //Fetches the player movement direction
    }

    public void FetchMouseInput(InputAction.CallbackContext context)
    {
        //Called via new input system: events/playablecharacter
        mouseYAxisInput = playerMovementInput.PlayableCharacter.Mouse.ReadValue<Vector2>().y;
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        isJumpPressed = context.performed;
        /*if (context.performed)
        {
            /*playerAnimation.SetAnimationState("jump", playerAnimation.defaultAnimator);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jump_force * Time.fixedDeltaTime);#1#
            // if (isGripped)
            // {
            //     isGripped = false;
            //     playerAnimation.gameObject.transform.localScale =
            //         new Vector2(playerAnimation.gameObject.transform.localScale.x, 1f);
            // }
            //
            // ChangeRigidbodyState(isGripped, rigidbody2D);
        }*/
    }

    private bool DeterminePlayerMotionState()
    {
        //Determines whether the player holds down the "movement" keys.
        bool _motion = movementAxisInput != 0;
        return _motion;
    }

    private void FetchInput()
    {
        //isJumpActionTaken = Input.GetKey(jump_keycode) && canJump;
        //isGrippedActionTaken = Input.GetKeyDown(grip_keycode) && !playerAction.isBusy && canGrip;
    }

    private void FetchRudimentaryVariables()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerAction = GetComponentInChildren<PlayerAction>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private Vector2 CalculateDesiredVelocity(float direction, float peakVeloValue)
    {
        Vector2 _desiredVelocity = new Vector2(direction, 0) * peakVeloValue;
        return _desiredVelocity;
    }
    
    private Vector2 FetchDesiredVelocity(float _direction, float _maximumSpeed)
    {
        Vector2 _desiredVelocity = new Vector2(_direction, 0f) * _maximumSpeed;
        return _desiredVelocity;
    }

    /// <summary>
    /// REWORK BELOW
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    private void BasicMovementAgility()
    {
        //Basic character mobility
        //-Movement
        //-Jump

        //Movement
        switch (playerAction.isBusy)
        {
            /*case true:
                switch (playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).IsName("AirAttack"))
                {
                    case true:
                        //In air attack
                        rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
                        break;
                    case false:
                        rigidbody2D.velocity = new Vector2(0, 0);
                        break;
                }

                break;*/
            case false:

                break;
        }

        //ground check for jumping, only when not in the air or is not gripped on wall
        if (!isGripped)
            GroundCheck();
        //Jump
        if (isJumpActionTaken)
        {
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
                        new Vector2(playerAnimation.gameObject.transform.localScale.x, -1f);
                    break;
                case false:
                    playerAnimation.gameObject.transform.localScale =
                        new Vector2(playerAnimation.gameObject.transform.localScale.x, 1f);
                    break;
            }
        }
    }

    private void GroundCheck()
    {
        //Checks jump condition
        Collider2D[] col = Physics2D.OverlapCircleAll(groundCheckPosition.position, groundCheckRadius, ground_layer);
        canJump = col.Length > 0;
        isGrounded = canJump;
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