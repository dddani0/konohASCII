using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
    [Header("Button inputs")] private PlayerMovementInput playerMovementInput;

    [Space] public float mouseYAxisInput;
    public float maximumPauseGameCooldown;
    private float currentPauseGameCooldown;

    [FormerlySerializedAs("movementAxisInput")]
    public float xMovementAxisInput;

    public float yMovementAxisInput;

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

    [FormerlySerializedAs("canJumpFromWall")]
    public bool hasLandedOnWall = false;

    [FormerlySerializedAs("isGrounded")] public bool isStandingOnGround;
    public bool isStandingOnWall;

    [FormerlySerializedAs("isOnRightWall")]
    public bool isOnTheRightWall;

    public bool canMoveOnWall;
    [Range(1, 3)] public float wallRaycastXOffset;
    [Range(1, 3)] public float wallRaycastYOffset;
    public Transform groundCheckPosition;

    [Tooltip("Speed, which the player will progress.")]
    public float maximumSpeed;

    //GMTK
    [Space, Tooltip("The maximum speed, the player can reach")]
    public float peakMovementSpeed;

    [Tooltip("The direction, which the player faces.")]
    private float xDirection;

    //Used for standing on the wall.
    private float yDirection;

    private Vector2 desiredVelocity;

    [Tooltip("Maximum speed, which the player can achieve on the ground.")]
    public float maximumLateralGroundAcceleration; //30

    private float maximumVerticalGroundAcceleration;

    [Tooltip("Maximum value, which the player will decelerates on the ground.")]
    public float maximumLateralGroundDeceleration; //40

    private float maximumVerticalGroundDeceleration;

    [Tooltip("Maximum speed, which the player can achieve in the air")]
    public float maximumAirLateralAcceleration; //25

    private float maximumAirVerticalAcceleration;

    [Tooltip("Maximum speed, which the player will turn on the ground")]
    public float maximumGroundTurnSpeed; //45

    private float maximumVerticalTurnSpeed;

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

    [Space, Range(1, 3f)] public float wallGripOffset;
    [Range(1f, 5f)] public float wallcheck_width_size;
    [Space] public bool canGrip;
    public bool isGripped = false;
    private bool isGrippedActionTaken;
    private bool isJumpActionTaken;
    public Transform wallcheck_position;
    [Space] public float switchWallStateCooldown;
    private float currentSwitchStanceCooldown;

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
        //Calculated the velocity, which is desired to reach at peak speed.
        AchieveMovementCalculations();

        //checks if the player is on the ground, or is on the wall
        canJump = FetchGroundInformation() || FetchWallJumpCondition();

        //debug purposes
        hasLandedOnWall = FetchWallJumpCondition();

        isStandingOnGround = FetchGroundInformation();
        isStandingOnWall = FetchWallState();

        canGrip = FetchWallInformation(wallcheck_position, wallcheck_width_size, wallcheck_height_size,
            wall_layer, canJump);

        PrefaceHorizontalJumpAchievement();
        isGrippedActionTaken = FetchGripInput();
        AssignAnimationVariables();
        if (isStandingOnWall) canMoveOnWall = FetchMoveStateOnWall();
        currentPauseGameCooldown = fetchPauseGameCooldown();
    }

    private void FixedUpdate()
    {
        AchieveMovementPhysics();
    }

    private void AchieveMovementPhysics()
    {
        AchieveRigidbody2D();
        switch (!isStandingOnWall)
        {
            case true:
                AchieveGroundLateralMovement();
                break;
            case false:
                AchieveWallLateralMovement();
                break;
        }

        AchieveJumpMovement();
        SwitchGroundField();
    }

    private void AssignAnimationVariables()
    {
        float _currentVelocity = !isGripped ? currentVelocity.x : currentVelocity.y;
        float _currentDirection = !isGripped ? math.abs(_currentVelocity) > 0.9 ? xDirection : 0 :
            canMoveOnWall ? yDirection : 0f;


        switch (int.TryParse(xDirection.ToString(), out int discardNumber))
        {
            case true:
                playerAnimation.SetAnimationState("direction", int.Parse(_currentDirection.ToString()),
                    playerAnimation.defaultAnimator);
                break;
        }

        playerAnimation.SetAnimationState("currentVelocity", math.abs(_currentVelocity),
            playerAnimation.defaultAnimator);
        playerAnimation.SetAnimationState("hasReachedPeakVelocity",
            math.abs(_currentVelocity) == 1f * peakMovementSpeed, playerAnimation.defaultAnimator);
        playerAnimation.SetAnimationState("onGround", isStandingOnGround, playerAnimation.defaultAnimator);
        playerAnimation.SetAnimationState("hasLandedOnwall", hasLandedOnWall, playerAnimation.defaultAnimator);
        playerAnimation.SetAnimationState("isFallingFromGround", !hasLandedOnWall && !isStandingOnGround,
            playerAnimation.defaultAnimator);
    }

    private void AchieveRigidbody2D()
    {
        currentVelocity = rigidbody2D.velocity;
    }

    private void AchieveMovementCalculations()
    {
        desiredVelocity = !isStandingOnWall
            ? CalculateDesiredVelocity(xDirection, peakMovementSpeed)
            : CalculateDesiredVelocity(yDirection, peakMovementSpeed);

        switch (int.TryParse(xDirection.ToString(), out int discardNumber))
        {
            case true:
                isPlayerInMotion = !isStandingOnWall
                    ? CheckPlayerMotion(int.Parse(xDirection.ToString()))
                    : CheckPlayerMotion(int.Parse(yDirection.ToString()));
                break;
        }
    }

    private void AchieveWallLateralMovement()
    {
        //Called when standing on the wall.
        //GMTK
        switch (playerAction.isBusy)
        {
            case true:
                break;
            case false:
                currentAcceleration = maximumVerticalGroundAcceleration;
                currentDeceleration = maximumVerticalGroundDeceleration;
                currentTurnSpeed = maximumVerticalTurnSpeed;

                switch (yDirection != 0)
                {
                    case true:
                        //Checks whether the player direction and the velocity direction matches
                        switch (Mathf.Sign(yDirection) != Mathf.Sign(rigidbody2D.velocity.y))
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

        currentVelocity.y = canMoveOnWall ? Mathf.MoveTowards(currentVelocity.y, desiredVelocity.y, maximumSpeed) : 0;
        rigidbody2D.velocity = currentVelocity;
    }


    #region Ground movement

    private void AchieveGroundLateralMovement()
    {
        //Called when standing on the ground.
        //GMTK
        switch (playerAction.isBusy)
        {
            case true:
                break;
            case false:
                currentAcceleration =
                    isStandingOnGround ? maximumLateralGroundAcceleration : maximumAirLateralAcceleration;
                currentDeceleration = isStandingOnGround ? maximumLateralGroundDeceleration : maximumAirDeceleration;
                currentTurnSpeed = isStandingOnGround ? maximumGroundTurnSpeed : maximumAirTurnSpeed;

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
        switch (isJumpExecuted && !playerAction.isBusy && canJump)
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

    private void PrefaceHorizontalJumpAchievement()
    {
        //GMTK
        //Calculations to make jump gravity smooth
        newGravity = new Vector2(0,
            (-2 * jumpHeight) / (timeToReachJumpHeightPeak * timeToReachJumpHeightPeak));
        rigidbody2D.gravityScale =
            !isGripped
                ? (newGravity.y / Physics2D.gravity.y) * gravityMultiplier
                : math.round(math.abs(1 * (yDirection)));
    }

    #endregion

    #region Button input

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
        xMovementAxisInput = context.ReadValue<Vector2>().x;
        yMovementAxisInput = context.ReadValue<Vector2>().y;
        xDirection = xMovementAxisInput;
        yDirection = yMovementAxisInput;
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
            playerAction.gamemanager.GetComponent<Gamemanager>().isGamePaused =
                !playerAction.gamemanager.GetComponent<Gamemanager>().isGamePaused && currentPauseGameCooldown <= 0;
    }

    public void FetchWallGripInput(InputAction.CallbackContext context)
    {
        if (context.started)
            isWallGrabPressed = true;
        else if (context.canceled)
            isWallGrabPressed = false;
    }

    #endregion

    private float fetchCurrentStanceCooldown()
    {
        float _currentCooldown = currentSwitchStanceCooldown -= Time.deltaTime;
        return _currentCooldown;
    }

    private float fetchPauseGameCooldown()
    {
        float _currentCooldown = currentPauseGameCooldown -= Time.deltaTime;
        return _currentCooldown;
    }


    private bool FetchWallRaycastStatus()
    {
        //Ugly code :D
        // RaycastHit2D ray = isOnRightWall //On right wall
        //     ? Physics2D.Raycast(playerAction.isFacingRight
        //             ? new Vector2(groundCheckPosition.position.x,
        //                 groundCheckPosition.position.y + wallRaycastYOffset) //on right wall, facing up
        //             : new Vector2(groundCheckPosition.position.x,
        //                 groundCheckPosition.position.y - wallRaycastYOffset), //On right wall, facing down
        //         playerAction.isFacingRight
        //             ? new Vector2(groundCheckPosition.position.x + wallRaycastXOffset,
        //                 groundCheckPosition.position.y + wallRaycastYOffset)
        //             : new Vector2(groundCheckPosition.position.x + wallRaycastXOffset,
        //                 groundCheckPosition.position.y - wallRaycastYOffset))
        //     : Physics2D.Raycast( //On left wall
        //         playerAction.isFacingRight
        //             ? new Vector2(groundCheckPosition.position.x,
        //                 groundCheckPosition.position.y + wallRaycastYOffset) //on right wall, facing up
        //             : new Vector2(groundCheckPosition.position.x,
        //                 groundCheckPosition.position.y - wallRaycastYOffset), //on right wall, facing down
        //         playerAction.isFacingRight
        //             ? new Vector2(groundCheckPosition.position.x - wallRaycastXOffset,
        //                 groundCheckPosition.position.y + wallRaycastYOffset)
        //             : new Vector2(groundCheckPosition.position.x - wallRaycastXOffset,
        //                 groundCheckPosition.position.y - wallRaycastYOffset));
        RaycastHit2D ray = Physics2D.Raycast(Vector2.zero, Vector2.zero); //Default value

        switch (isOnTheRightWall)
        {
            case true:
                switch (playerAction.isFacingRight)
                {
                    case true: //Z = 90 (right wall) and facing up (facing right)
                        ray = Physics2D.Raycast(new Vector2(groundCheckPosition.position.x,
                                groundCheckPosition.position.y + wallRaycastYOffset),
                            -groundCheckPosition.up * wallRaycastXOffset);
                        break;
                    case false: //Z = 90 (right wall) and facing down (facing left)
                        ray = Physics2D.Raycast(new Vector2(groundCheckPosition.position.x,
                                groundCheckPosition.position.y - wallRaycastYOffset),
                            -groundCheckPosition.up * wallRaycastXOffset);
                        break;
                }

                break;
            case false:
                switch (playerAction.isFacingRight)
                {
                    case true: //Z = -90 (left wall) and facing up (facing right)
                        ray = Physics2D.Raycast(new Vector2(groundCheckPosition.position.x,
                                groundCheckPosition.position.y - wallRaycastYOffset),
                            -groundCheckPosition.up * wallRaycastXOffset);
                        break;
                    case false:
                        ray = Physics2D.Raycast(new Vector2(groundCheckPosition.position.x,
                                groundCheckPosition.position.y + wallRaycastYOffset),
                            -groundCheckPosition.up * wallRaycastXOffset);
                        break;
                }

                break;
        }

        bool _isOnGround = false;
        if (ray.collider != null) _isOnGround = ray.collider.gameObject.name.ToLower().Contains("wall");

        return _isOnGround;
    }

    private bool FetchMoveStateOnWall()
    {
        bool _canMoveOnWall = FetchWallRaycastStatus();
        return _canMoveOnWall;
    }

    private void FetchRudimentaryVariables()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        playerAction = GetComponentInChildren<PlayerAction>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //Assign same values to vertical values
        maximumVerticalGroundAcceleration = maximumLateralGroundAcceleration;
        maximumVerticalGroundDeceleration = 100;
        maximumVerticalTurnSpeed = 100;
        maximumAirVerticalAcceleration = maximumAirLateralAcceleration;
        currentSwitchStanceCooldown = switchWallStateCooldown;
    }

    private void ChangeWallStance(bool _affectGravity)
    {
        isStandingOnWall = _affectGravity;
        ChangeRigidbodyState(_affectGravity, _affectGravity, rigidbody2D);
        isGripped = _affectGravity;
    }

    private bool FetchGripInput()
    {
        bool _isGrippedActionTaken = !playerAction.isBusy && canGrip && isWallGrabPressed;
        return _isGrippedActionTaken;
    }

    private bool CheckPlayerMotion(int _motionValue)
    {
        bool _motion = math.abs(_motionValue) == 1;
        return _motion;
    }

    private bool FetchGroundInformation()
    {
        //Checks jump condition
        Collider2D[] col = Physics2D.OverlapCircleAll(groundCheckPosition.position, groundCheckRadius, ground_layer);
        bool _isTouchingCollisionObject = col.Length > 0 && !isGripped;
        return _isTouchingCollisionObject;
    }

    private bool FetchWallJumpCondition()
    {
        bool _isStandingOnWallCanJump = isGripped;
        return _isStandingOnWallCanJump;
    }

    private bool FetchWallInformation(Transform _wallCheckTransform, float _WallCheckWidth, float _WallCheckHeight,
        int wallLayerIndex, bool _isJumpAvailable)
    {
        Collider2D[] wallcol = Physics2D.OverlapBoxAll(_wallCheckTransform.position,
            new Vector2(_WallCheckWidth, _WallCheckHeight),
            _wallCheckTransform.rotation.x, wallLayerIndex);
        bool _canWallBeGripped = wallcol.Length > 0 && _isJumpAvailable == false;
        return _canWallBeGripped;
    }

    private bool FetchWallState()
    {
        //is on ground?
        bool _isOnWall = isGripped && !isStandingOnGround;
        return _isOnWall;
    }

    private Vector2 CalculateDesiredVelocity(float _direction, float _peakVelocityValue)
    {
        //GMTK
        Vector2 _desiredVelocity = Vector2.zero;
        switch (isStandingOnWall)
        {
            case true:
                _desiredVelocity = new Vector2(0, _direction) * _peakVelocityValue;
                break;
            case false:
                _desiredVelocity = new Vector2(_direction, 0) * _peakVelocityValue;
                break;
        }

        return _desiredVelocity;
    }

    private void SwitchGroundField()
    {
        switch (!isStandingOnWall) //Is not standing on wall?
        {
            case true:
                if (isStandingOnGround)
                {
                    ChangeWallStance(false);
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                    currentSwitchStanceCooldown = fetchCurrentStanceCooldown();
                    // Collect wall information, set disable brakes.

                    //Collect touching wall.
                    Collider2D[] wallcol = Physics2D.OverlapBoxAll(wallcheck_position.position,
                        new Vector2(wallcheck_width_size, wallcheck_height_size),
                        wallcheck_position.rotation.x, wall_layer);

                    Transform wallpos = null;

                    //'Which wall am I touching? Right or left?'
                    if (wallcol.Length > 0)
                        wallpos = wallcol[0].transform;
                }

                if (isGrippedActionTaken)
                {
                    //Reset the flag
                    canGrip = false;

                    //Set Grip flag true
                    isGripped = true;

                    //Freeze x position
                    ChangeRigidbodyState(true, false, rigidbody2D);
                    currentSwitchStanceCooldown = switchWallStateCooldown;
                    // Collect wall information, set enable brakes.

                    //Collect touching wall.
                    Collider2D[] wallcol = Physics2D.OverlapBoxAll(wallcheck_position.position,
                        new Vector2(wallcheck_width_size, wallcheck_height_size),
                        wallcheck_position.rotation.x, wall_layer);

                    Transform wallpos = null;

                    //'Which wall am I touching? Right or left?'
                    if (wallcol.Length > 0)
                        wallpos = wallcol[0].transform;

                    if (!wallpos) return;

                    switch (wallpos.position.x > gameObject.transform.position.x)
                    {
                        case true:
                            isOnTheRightWall = true;
                            transform.localEulerAngles = new Vector3(0, 0, 90);
                            rigidbody2D.transform.position += new Vector3(-wallGripOffset, 0, 0);
                            break;
                        case false:
                            isOnTheRightWall = false;
                            transform.localEulerAngles = new Vector3(0, 0, -90);
                            rigidbody2D.transform.position += new Vector3(wallGripOffset, 0, 0);
                            playerAnimation.transform.localScale = new Vector3(-1, 1, 1);
                            break;
                    }
                }

                break;
            case false:
                currentSwitchStanceCooldown = fetchCurrentStanceCooldown();
                if (isWallGrabPressed && !playerAction.isBusy && currentSwitchStanceCooldown <= 0)
                {
                    ChangeWallStance(false);
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                    currentSwitchStanceCooldown = switchWallStateCooldown;
                }

                break;
        }
    }

    public void ChangeRigidbodyState(bool freeze, Rigidbody2D _rigidbody)
    {
        //Freeze or unfreeze whole rigidbody
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
        //Freeze a specific axis on the rigidbody.
        bool _isotheraxisfrozen = false;
        switch (_freeze_x)
        {
            case true:
                _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
                break;
            case false:
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