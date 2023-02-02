using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PlayableCharacterMovement : MonoBehaviour
{
    /// <summary>
    /// Based on GMTK method.
    /// "Platformer Toolkit": https://gmtk.itch.io/platformer-toolkit
    /// "Behind the code": https://gmtk.itch.io/platformer-toolkit/devlog/395523/behind-the-code
    /// I highly suggest, you check it out for yourself!
    /// If you have further questions you can reach me out below!
    /// https://linktr.ee/devley0
    ///
    /// Ground variables -> "NOWALL_..."
    /// Wall variables -> "WALL_..."
    /// </summary>
    /* -->>Unaccesseble members<<-- */
    private Rigidbody2D _rigidbody;

    private PlayerMovementInput _playerMovementInput;

    [Header("BUTTON INPUTS")]
    /* -->>Button inputs<<-- */
    //TODO: remove [serializedField] once it's working, with header.
    //TODO: rename private members respectfully to the guidelines.
    [SerializeField]
    private bool hasJumped;

    [SerializeField] private bool isHoldingJump;
    [SerializeField] private bool wallgrabButton;
    [SerializeField] private bool pickUpButton;
    private float _xDirection, _yDirection;

    /* -->>Condition occurences<<-- */
    private bool _canJump;

    //Decides if is on wall or not.
    private bool _isStandingOnWall;
    private bool _canMoveOnWall;

    [Header("GENERAL MOVEMENT PRINCIPLES")]
    public float xMovementAxisInput;

    public float yMovementAxisInput;

    [Space] public LayerMask groundLayer;
    public Transform groundCheckPosition;
    [Range(0, 15f)] public float groundCheckRadius;
    [Space] public LayerMask wallLayer;
    private Transform wallCheckPosition => transform;
    [Range(0, 15f)] public float wallCheckRadius;

    [Header("NON-WALL MOVEMENT PRINCIPLES")]
    /* -->>Non-wall Movement principles<<-- */
    [SerializeField]
    private Vector2 currentVelocity;
    private float _currentAcceleration;
    private float _currentTurnSpeed;
    private float _currentDeceleration;
    private Vector2 _desiredVelocity;
    private float _maxProgressSpeed;
    public float peakMovementSpeed;
    private Vector2 _newGravity;
    [Space] 
    public float jumpSpeed;
    public float jumpHeight;
    public float timeToReachJumpHeighPeak;
    public float jumpCutOff;
    public float gravityMultiplier;
    public float downwardMovementMultiplier;
    [Space]
    //accelerator elements:
    public float nowall_maxGroundAcceleration; //<-- 30

    public float nowall_maxAirAcceleration; //<-- 25

    //Turn elements
    public float nowall_maxGroundTurnSpeed; //<-- 45

    public float nowall_maxAirTurnSpeed; //10

    //deceleration elements:
    public float nowall_maxGroundDeceleration; //<-- 40
    public float nowall_maxAirDeceleration; //<-- 15

    [Header("WALL MOVEMENT PRINCIPLES")]
    /* -->>Wall Movement principles<<-- */
    //accelerator elements:
    public float WALL_maxGroundAcceleration;

    //Turn elements
    private float WALL_maxTurnSpeed;

    //deceleration elements:
    public float WALL_maxGroundDeceleration;
    [Space, Range(0,15f)] public float maximumTimeBtwWallAttempts;
    private float _timeBtwWallAttempts;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PrefaceMovementStub();
    }

    private void FixedUpdate()
    {
        ManageMovement();
    }

    #region Movement

    private void PrefaceMovementStub()
    {
        //Only separated to be called in default update.
        CalculateDesiredVelocity(xMovementAxisInput, peakMovementSpeed);
        CheckMovementOccurences();
        PrefaceJumpAchievement();
    }

    private void CalculateDesiredVelocity(float input, float peakSpeed)
    {
        _desiredVelocity = new Vector2(input, 0) * peakSpeed;
    }

    private void CheckMovementOccurences()
    {
        _canJump = FetchGroundStatus();
    }

    private void PrefaceJumpAchievement()
    {
        _newGravity = new Vector2(0,
            (-2 * jumpHeight)
            /
            (timeToReachJumpHeighPeak * timeToReachJumpHeighPeak));
        _rigidbody.gravityScale = (_newGravity.y / Physics2D.gravity.y) * gravityMultiplier;
    }

    private bool FetchGroundStatus()
    {
        var collisions = Physics2D.OverlapCircleAll(
            groundCheckPosition.position,
            groundCheckRadius, groundLayer);
        return collisions.Length > 0 && !_isStandingOnWall;
    }
    
    private void MatchPlayerToWall()
    {
        bool HasDetectedWall()
        {
            var detectedCols = new Collider2D[10];
            return Physics2D.OverlapCircleNonAlloc(
                wallCheckPosition.position, 
                wallCheckRadius, 
                detectedCols,wallLayer) > 0;
        }
        
        bool IsAttachedToWall() => _isStandingOnWall;

        bool IsWallOnRightSide(Transform wallPosition) => wallPosition.position.x > transform.position.x;

        bool IsNextToMoreWalls(Collider2D[] cols) => cols.Length > 1;

        bool HasAttemptedToAttachToWall() => !IsAttachedToWall() && wallgrabButton;

        Collider2D[] Walls() => Physics2D.OverlapCircleAll(
            transform.position, 
            wallCheckRadius,
            wallLayer);

        Collider2D GetClosestWall(Collider2D[] cols)
        {
            Vector3 position;
            return (cols[0].transform.position - (position = transform.position)).magnitude <
                   (cols[1].transform.position - position).magnitude
                ? cols[0]
                : cols[1];
        }

        if (!HasAttemptedToAttachToWall()) return;
        
        if (!FetchGroundStatus() && HasDetectedWall())
        {
            var isOnRightWall = IsNextToMoreWalls(Walls())
                ? GetClosestWall(Walls())
                : IsWallOnRightSide(Walls()[0].transform);
                    
            _isStandingOnWall = true;

            switch (isOnRightWall)
            {
                case true:
                    transform.localEulerAngles = new Vector3(0, 0, 90);
                    _rigidbody.transform.position += new Vector3(-5, 0, 0); //-5 => offset
                    break;
                case false:
                    transform.localEulerAngles = new Vector3(0, 0, -90);
                    _rigidbody.transform.position += new Vector3(5, 0, 0); //5 => offset
                    //graphics local scale: new Vector3(-1, 1, 1);
                    break;
            }
        }
        
    }

    private void ManageMovement()
    {
        bool MoveGround() => !_isStandingOnWall;

        bool IsOnGround() => _canJump;
        
        _currentAcceleration = IsOnGround() ?
            nowall_maxGroundAcceleration :
            nowall_maxAirAcceleration;
            
        _currentDeceleration = IsOnGround() ? 
            nowall_maxGroundDeceleration :
            nowall_maxAirAcceleration;
        
        _currentTurnSpeed = IsOnGround() ?
            nowall_maxGroundTurnSpeed :
            nowall_maxAirTurnSpeed;

        switch (MoveGround())
        {
            case true:
                MakeGroundMovement();
                MakeJumpMovement();
                MatchPlayerToWall();
                break;
            case false:
                MakeWallMovement();
                break;
        }

        _rigidbody.velocity = currentVelocity;
    }

    private void MakeGroundMovement()
    {
        switch (_xDirection != 0)
        {
            case true:
                _maxProgressSpeed =
                    Math.Abs(Mathf.Sign(_xDirection) - Mathf.Sign(_rigidbody.velocity.x)) > 0
                        ? _currentTurnSpeed * Time.deltaTime
                        : _currentAcceleration * Time.deltaTime;
                break;
            case false:
                _maxProgressSpeed = _currentDeceleration * Time.deltaTime;
                break;
        }

        currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, _desiredVelocity.x, _maxProgressSpeed);
    }

    private void MakeJumpMovement()
    {
        switch (_canJump && hasJumped)
        {
            case true:
                jumpSpeed = Mathf.Sqrt(
                    -2f * Physics2D.gravity.y * _rigidbody.gravityScale * jumpHeight);
                if (currentVelocity.y > 0)
                    jumpSpeed = Mathf.Max(jumpSpeed - currentVelocity.y, 0f);
                else
                    jumpSpeed += Mathf.Abs(_rigidbody.velocity.y);
                currentVelocity.y += jumpSpeed;
                hasJumped = false;
                break;
            case false:
                hasJumped = false;
                currentVelocity.y = _rigidbody.velocity.y;
                break;
        }

        isHoldingJump = false;

        if (_rigidbody.velocity.y > 0.01f)
        {
            if (isHoldingJump && hasJumped)
                gravityMultiplier = 1;
            else
                gravityMultiplier = jumpCutOff;
        }
        else if (_rigidbody.velocity.y < -0.01f)
            gravityMultiplier = downwardMovementMultiplier;
        else
            gravityMultiplier = 1;
    }

    private void MakeWallMovement()
    {
    }

    #endregion

    #region inputs

    public void InputMovement(InputAction.CallbackContext context)
    {
        float ButtonInputHelper(InputAction.CallbackContext _context) 
            => _context.ReadValue<Vector2>().x;
        xMovementAxisInput = ButtonInputHelper(context);
    }

    public void InputJump(InputAction.CallbackContext context)
    {
        void ButtonInputHelper (InputAction.CallbackContext _context)
        {
            if (_context.started)
            {
                isHoldingJump = true;
                hasJumped = true;
            }
            if (_context.canceled) isHoldingJump = false;
        }
        ButtonInputHelper(context);
    }

    #endregion
    #region default stub

    private void OnEnable()
    {
        _playerMovementInput = new PlayerMovementInput();
        _playerMovementInput.Enable();
    }

    private void OnDisable()
    {
        _playerMovementInput.Disable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheckPosition.position, groundCheckRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheckPosition.position, wallCheckRadius);
    }

    #endregion
}
