using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Every snippet marked with //GMTK is inspired, helped from the "Platformer Toolkit"
    /// "Platformer Toolkit": https://gmtk.itch.io/platformer-toolkit
    /// "Behind the code": https://gmtk.itch.io/platformer-toolkit/devlog/395523/behind-the-code
    /// I highly suggest, you check it out for yourself!
    /// If you have further questions you can reach me out below!
    /// https://linktr.ee/devley0
    /// </summary>
    [Header("Button inputs")] PlayerMovementInput playerMovementInput;

    [Space] public float mouseYAxisInput;
    public float movementAxisInput;
    [SerializeField] private bool isPlayerInMotion;
    [SerializeField] private bool isPressingJump;
    [SerializeField] private bool isJumpExecuted;
    [SerializeField] private bool isWallGrabPressed;
    public bool pickUpButton;
    public Rigidbody2D rigidbody2D;

    [Header("Resources")] public PlayerAnimation playerAnimation;

    public PlayerAction playerAction;

    [Space(20f)] [Header("Smooth lateral movement")] [Range(1f, 5f)] [SerializeField] [Space]
    public float groundCheckRadius;

    [Space] public bool canJump = true;
    public bool isGrounded;
    public Transform groundCheckPosition;

    [Tooltip("Speed, which the player will progress.")]
    public float maximumSpeed;

    //GMTK
    [Space, Tooltip("The maximum speed, the player can reach")]
    public float peakMovementSpeed;

    [Tooltip("The direction, which the player faces.")]
    private float xDirection;

    private Vector2 desiredVelocity;

    [Tooltip("Maximum speed, which the player can achieve on the ground.")]
    public float maximumLateralGroundAcceleration; //30

    [Tooltip("Maximum value, which the player will decelerates on the ground.")]
    public float maximumLateralGroundDeceleration; //40

    [Tooltip("Maximum speed, which the player can achieve in the air")]
    public float maximumAirLateralAcceleration; //25

    [Tooltip("Maximum speed, which the player will turn on the ground")]
    public float maximumGroundTurnSpeed; //45

    [Tooltip("Maximum value, which the player will decelerate in the air.")]
    public float maximumAirDeceleration; //15

    [Tooltip("Turning speed in the air")] public float maximumAirTurnSpeed; //10

    [SerializeField] private float currentTurnSpeed;
    [SerializeField] private Vector2 currentVelocity;
    [SerializeField] private float currentDeceleration;
    [SerializeField] private float currentAcceleration;

    [Space(20f), Header("Smooth jump movement")] [Tooltip("The height, the player can achieve with jump")]
    public float jumpHeight; //1300

    [SerializeField] private float jumpSpeed;

    [Tooltip("The time, which it takes to reach the maximum height.")]
    public float timeToReachJumpHeightPeak;

    private Vector2 newGravity;
    public float gravityMultiplier;
    public float downwardMovementMultiplier;

    public float jumpCutOff;

    //GMTK end
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
        isPlayerInMotion = CheckPlayerMotion(isPlayerInMotion, int.Parse(xDirection.ToString()));
        canJump = FetchGroundInformation();
        isGrounded = FetchGroundInformation();
        canGrip = FetchWallInformation(wallcheck_position, wallcheck_width_size, wallcheck_height_size,
            wall_layer, canJump);
        PrefaceJumpAchievement();
        FetchInput();
        WallGrip();
        AssignAnimationVariables();
    }

    private void FixedUpdate()
    {
        AchieveRigidbody2D();
        AchieveGroundLateralMovement();
        AchieveJumpMovement();
    }

    private void AssignAnimationVariables()
    {
        playerAnimation.SetAnimationState("xDirection", int.Parse(xDirection.ToString()),
            playerAnimation.defaultAnimator);
        playerAnimation.SetAnimationState("currentVelocity", math.abs(currentVelocity.x),
            playerAnimation.defaultAnimator);
        playerAnimation.SetAnimationState("hasReachedPeakVelocity",
            math.abs(currentVelocity.x) == 1f * peakMovementSpeed, playerAnimation.defaultAnimator);
        playerAnimation.SetAnimationState("onGround", isGrounded, playerAnimation.defaultAnimator);
    }

    private void AchieveRigidbody2D()
    {
        currentVelocity = rigidbody2D.velocity;
    }

    private void AchieveGroundLateralMovement()
    {
        //GMTK
        switch (playerAction.isBusy)
        {
            case true:
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

                break;
        }

        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, desiredVelocity.x, maximumSpeed);
        rigidbody2D.velocity = currentVelocity;
    }

    private void AchieveJumpMovement()
    {
        //GMTK
        switch (isJumpExecuted && !playerAction.isBusy && isGrounded)
        {
            case true:
                playerAnimation.SetAnimationState("jump", playerAnimation.defaultAnimator);
                jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * rigidbody2D.gravityScale * jumpHeight);
                if (currentVelocity.y > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - currentVelocity.y, 0f);
                }
                else if (currentVelocity.y < 0f)
                {
                    jumpSpeed += Mathf.Abs(rigidbody2D.velocity.y);
                }

                currentVelocity.y += jumpSpeed;
                rigidbody2D.velocity = currentVelocity;
                isJumpExecuted = false;
                break;
            case false:
                isJumpExecuted = false;
                break;
        }

        isPressingJump = false;

        if (rigidbody2D.velocity.y > 0.01f)
        {
            if (isPressingJump && isJumpActionTaken)
            {
                gravityMultiplier = 1;
            }
            else
            {
                gravityMultiplier = jumpCutOff;
            }
        }
        else if (rigidbody2D.velocity.y < -0.01f)
            gravityMultiplier = downwardMovementMultiplier;

        else
            gravityMultiplier = 1;
    }

    private void PrefaceJumpAchievement()
    {
        //GMTK
        //Calculations to make jump gravity smooth
        newGravity = new Vector2(0,
            (-2 * jumpHeight) / (timeToReachJumpHeightPeak * timeToReachJumpHeightPeak));
        rigidbody2D.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravityMultiplier;
    }

    private void OnEnable()
    {
        playerMovementInput.Enable();
    }

    private void OnDisable()
    {
        playerMovementInput.Disable();
    }

    public void FetchPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
            playerAction.isPrimaryAttack = true;
        else if (context.canceled)
            playerAction.isPrimaryAttack = false;
    }

    public void FetchSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
            playerAction.isSecondaryAttack = true;
        else if (context.canceled)
            playerAction.isSecondaryAttack = false;
    }

    public void FetchBlockInput(InputAction.CallbackContext context)
    {
        if (context.started)
            playerAction.isBlocking = true;
        else if (context.canceled)
            playerAction.isBlocking = false;
    }

    public void FetchPickupInput(InputAction.CallbackContext context)
    {
        if (context.started)
            pickUpButton = true;
        else if (context.canceled)
            pickUpButton = false;
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
        if (context.started)
        {
            isPressingJump = context.performed;
            isJumpExecuted = true;
        }

        if (context.canceled)
            isPressingJump = false;
    }

    public void FetchPauseMenuInput(InputAction.CallbackContext context)
    {
        if (context.started)
            playerAction.gamemanager.GetComponent<Gamemanager>().isGamePaused = true;
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

    private bool CheckPlayerMotion(bool _isInMotion, int _motionValue)
    {
        bool _motion = math.abs(_motionValue) == 1;
        return _motion;
    }

    private bool FetchGroundInformation()
    {
        //Checks jump condition
        Collider2D[] col = Physics2D.OverlapCircleAll(groundCheckPosition.position, groundCheckRadius, ground_layer);
        bool _isTouchingCollisionObject = col.Length > 0;
        return _isTouchingCollisionObject;
    }

    private bool FetchWallInformation(Transform _wallCheckTransform, float _WallCheckWidth, float _WallCheckHeight,
        int wallLayerIndex, bool _isJumpAvailable)
    {
        /*Collider2D[] wallcol = Physics2D.OverlapBoxAll(wallcheck_position.position,
            new Vector2(wallcheck_width_size, wallcheck_height_size),
            wallcheck_position.rotation.x, wall_layer);*/

        Collider2D[] wallcol = Physics2D.OverlapBoxAll(_wallCheckTransform.position,
            new Vector2(_WallCheckWidth, _WallCheckHeight),
            _wallCheckTransform.rotation.x, wallLayerIndex);
        bool _canWallBeGripped = wallcol.Length > 0 && _isJumpAvailable == false;
        return _canWallBeGripped;
    }

    private Vector2 CalculateDesiredVelocity(float _direction, float _peakVelocityValue)
    {
        //GMTK
        Vector2 _desiredVelocity = new Vector2(_direction, 0) * _peakVelocityValue;
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
            case false:

                break;
        }

        //ground check for jumping, only when not in the air or is not gripped on wall
        if (!isGripped)
            //GroundCheck();
            //Jump
            if (isJumpActionTaken)
            {
            }
    }

    private void AdvancedMovementAgility()
    {
        //Advanced character mobility
        //-Wall jump

        //WallCheck();
        //playerAnimation.SetAnimationState("gripped", isGripped, playerAnimation.defaultAnimator);
    }

    private void WallGrip()
    {
        if (isGrippedActionTaken)
        {
            //rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
            //ChangeRigidbodyState(true, rigidbody2D);
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