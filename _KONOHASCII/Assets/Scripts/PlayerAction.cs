using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Resources")] public GameObject gamemanager;
    public PlayerMovement playerMovement;
    public PlayerAnimation playerAnimation;

    [Space(20f)] public bool isBlocking;

    [Header("Weapon_Attribute")]
    [SerializeField]
    [Tooltip("Primary weapon is always close range weapon. Like Katana. If null, will attribute as fist attack")]
    public WeaponTemplate activePrimaryWeapon;

    [Space] public int fistDamage;
    [Space] public int kickDamage;
    [Space] public int attackRadius;
    [Space] public bool isCombo;
    [SerializeField] private bool canProceedWithCombo;
    public float punchAnimationTimeLeft;

    [Tooltip("Secondary weapon is always throwable. Like Shuriken")]
    public WeaponTemplate activeSecondaryWeaponTemplate;

    [SerializeField] private GameObject weaponContainer;

    [Space] public Transform[] weaponPosition;
    [Space] public int rangeWeaponAmmunition;
    [Space(20f)] [Header("Health")] public float maximumHealthPoints;
    [SerializeField] private float healthPoints;
    [Space(20f)] [Header("Chakra")] public int maximumChakra;
    [SerializeField] private int chakra;
    [Space(20f)] [Header("Brakes")] public bool isBusy;
    public bool isStaggered;
    public bool isFacingRight;
    [Space] public float maximumCastWeaponAngle; //Two values -x and x
    [SerializeField] private float castWeaponAngle;
    [Header("Visible Crosshair")] public GameObject crosshairGameObject;
    public Vector3 crosshairOffsetPosition;
    public float crosshairRadius;
    public float crosshairVisualSpeed;
    [SerializeField] private Sprite crosshairSprite;

    [Header("ChakraAttribute")] public float maximumTimeBtwChakraRegeneratingProcedure;
    [SerializeField] private float timeBtwChakraRegeneratingProcedure;
    [SerializeField] private bool hasChakraRegenerationCooldownPassed;
    [Space] public float maximumIntervalBtwChakraRegeneration;
    [SerializeField] private float intervalBtwChakraRegeneration;
    public int chakraRegenerationRate;

    [Space] [Tooltip("Shadow represents the player's y position relative to ground.")]
    public GameObject shadow;

    public Vector3 shadowPosition;
    [SerializeField] private float shadowPositionYOffset;
    [SerializeField] private float shadowPositionXOffset;

    [Space(20f)] [Header("Layermasks and Button mapping")]
    public LayerMask enemylayer;

    [Space] public KeyCode attackKeycode;
    public KeyCode rangeAttackKeycode;
    public KeyCode weaponSwapKeycode;
    public KeyCode blockKeyCode;

    private void Start()
    {
        FetchRudimentaryValues();
    }

    void Update()
    {
        if (isCombo)
        {
            isCombo = CheckComboState(); //Cancels Combo state, when returning to "Idle" animation state
            canProceedWithCombo = CheckComboFollowUpState();
            //Checks if the current punch animation is on it's last animation ("ThirdPunch")
        }

        shadow.transform.position = shadowPosition;
        isBlocking = CheckBlockState();
        isFacingRight = CheckObjectOrientation();
        CheckBusyBooleanStatement();
        PrimaryShortRangeAttack();
        RangeAttack(); //Signals expensive method invocation
        ChakraBlock();
        CrosshairDisplay();
    }

    private void LateUpdate()
    {
        shadowPosition = CalculateShadowPosition(transform);
    }

    private void FixedUpdate()
    {
        if (isCombo)
            punchAnimationTimeLeft -= Time.deltaTime;
    }

    private void FetchRudimentaryValues()
    {
        gamemanager = GameObject.FindGameObjectWithTag("Gamemanager");
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        GetComponent<PlayableCharacter>()
            .AssignNewPlayableCharacter(GetComponent<PlayableCharacter>().playableCharacter);
        chakra = maximumChakra;
        timeBtwChakraRegeneratingProcedure = maximumTimeBtwChakraRegeneratingProcedure;
        intervalBtwChakraRegeneration = maximumIntervalBtwChakraRegeneration;
        crosshairGameObject.transform.position = CalculateCrosshairPosition();
        crosshairGameObject.GetComponent<SpriteRenderer>().sprite = crosshairSprite;
        weaponContainer = gamemanager.GetComponent<Gamemanager>().weapon_container;
    }

    private void PrimaryShortRangeAttack()
    {
        switch (playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).IsName("fall") ||
                playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
        {
            case true:
                // if (Input.GetKey(attackKeycode) && !isBusy)
                // {
                //     playerAnimation.SetAnimationState("airAttack", playerAnimation.defaultAnimator);
                // }

                break;
            case false:
                switch (isCombo)
                {
                    case true:
                        switch (activePrimaryWeapon != null)
                        {
                            case true:
                                // if (Input.GetKey(attackKeycode) && canProceedWithCombo)
                                // {
                                //     playerAnimation.SetAnimationState("attack", playerAnimation.defaultAnimator);
                                //     punchAnimationTimeLeft =
                                //         playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
                                // }

                                break;
                            case false:
                                // if (Input.GetKey(attackKeycode) && canProceedWithCombo)
                                // {
                                //     playerAnimation.SetAnimationState("weapon_attack", playerAnimation.defaultAnimator);
                                //     punchAnimationTimeLeft =
                                //         playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
                                // }

                                break;
                        }

                        break;
                    case false:
                        switch (activePrimaryWeapon == null)
                        {
                            case true:
                                // if (Input.GetKey(attackKeycode) && !isBusy)
                                // {
                                //     playerAnimation.SetAnimationState("attack", playerAnimation.defaultAnimator);
                                //     isCombo = true;
                                //     punchAnimationTimeLeft =
                                //         playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
                                // }

                                break;
                            case false:
                                // if (Input.GetKey(attackKeycode) && !isBusy)
                                // {
                                //     playerAnimation.SetAnimationState("weapon_attack", playerAnimation.defaultAnimator);
                                //     isCombo = true;
                                //     punchAnimationTimeLeft =
                                //         playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
                                // }

                                break;
                        }

                        break;
                }

                break;
        }
    }

    private void RangeAttack()
    {
        //Creates instance of weapon prefab.
        //Modifies said instance from selected asset.

        //if (Input.GetKey(rangeAttackKeycode) && !isBusy &&
        //     !playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).IsName("wallgrip"))
        // {
        //     playerAnimation.SetAnimationState("range_attack", playerAnimation.defaultAnimator);
        //     GameObject _temporaryWeapon;
        //     switch (isFacingRight)
        //     {
        //         case true:
        //             _temporaryWeapon = Instantiate(weaponContainer,
        //                 weaponPosition[0].position, weaponPosition[0].rotation);
        //             _temporaryWeapon.GetComponent<SecondaryWeaponContainer>()
        //                 .AssignNewWeapon(activeSecondaryWeaponTemplate, CalculateWeaponAngle(), 1);
        //             break;
        //         case false:
        //             _temporaryWeapon = Instantiate(weaponContainer,
        //                 weaponPosition[1].position, weaponPosition[1].rotation);
        //             _temporaryWeapon.GetComponent<SecondaryWeaponContainer>()
        //                 .AssignNewWeapon(activeSecondaryWeaponTemplate, CalculateWeaponAngle(), -1);
        //             break;
        //     }
        // }
    }

    private void ChakraBlock()
    {
        //Indicates if chakra regeneration can begin
        //That can only happen, if the cooldown is completed.
        //This is so, that the player has to consider using chakra block
        hasChakraRegenerationCooldownPassed = CheckCanChakraRegenerate();

        playerAnimation.SetAnimationState("chakraValue", chakra, playerAnimation.defaultAnimator);
        playerAnimation.SetAnimationState("isBlocking", isBlocking, playerAnimation.defaultAnimator);
        switch (hasChakraRegenerationCooldownPassed)
        {
            case true:
                if (intervalBtwChakraRegeneration <= 0)
                {
                    chakra += chakraRegenerationRate;
                    intervalBtwChakraRegeneration = maximumIntervalBtwChakraRegeneration;
                }
                else
                    intervalBtwChakraRegeneration -= Time.deltaTime;

                break;
            case false:
                if (timeBtwChakraRegeneratingProcedure > 0)
                {
                    timeBtwChakraRegeneratingProcedure -= Time.deltaTime;
                }

                break;
        }
    }

    private void CrosshairDisplay()
    {
        castWeaponAngle = CalculateCrosshairYAngle(castWeaponAngle, false);
        crosshairGameObject.transform.position = CalculateCrosshairPosition();
    }

    private Vector3 CalculateCrosshairPosition()
    {
        castWeaponAngle = CalculateCrosshairYAngle(castWeaponAngle, false);
        float _verticalMouseInput = castWeaponAngle;
        bool _isFacingRight = isFacingRight;
        Vector3 _crosshairPosition = _isFacingRight
            ? new Vector3(transform.position.x + crosshairRadius + crosshairOffsetPosition.x,
                transform.position.y + _verticalMouseInput)
            : new Vector3(transform.position.x + crosshairRadius * -1 + crosshairOffsetPosition.x * -1,
                transform.position.y + _verticalMouseInput);

        return _crosshairPosition;
    }

    private Vector3 CalculateShadowPosition(Transform _shadowPosition)
    {
        Vector3 _shadowVerdictPosition = new Vector3(_shadowPosition.transform.position.x,
            CalculatePreShadowPositionWithRayCast(_shadowPosition).position.y + shadowPositionYOffset);

        return _shadowVerdictPosition;
    }

    private Transform CalculatePreShadowPositionWithRayCast(Transform _shadowArgumentPosition)
    {
        Transform _raycastStartPosition = _shadowArgumentPosition;
        Transform _shadowPosition = _raycastStartPosition;
        bool _isFacingRight = isFacingRight;
        RaycastHit2D _raycastHit2D = _isFacingRight
            ? Physics2D.Raycast(
                new Vector3(transform.position.x - shadowPositionXOffset, transform.position.y - shadowPositionYOffset),
                new Vector2(_raycastStartPosition.position.x - shadowPositionXOffset,
                    _raycastStartPosition.position.y - 150))
            : Physics2D.Raycast(
                new Vector3(transform.position.x + shadowPositionXOffset, transform.position.y - shadowPositionYOffset),
                new Vector2(_raycastStartPosition.position.x + shadowPositionXOffset,
                    _raycastStartPosition.position.y - 150));
        //hardcoded raycast lenght
        if (_raycastHit2D.collider)
            if (_raycastHit2D.collider.CompareTag("Untagged"))
                _shadowPosition = _raycastHit2D.collider.transform;
        return _shadowPosition;
    }

    private float CalculateWeaponAngle()
    {
        Transform weaponStartPositionTransform = isFacingRight ? weaponPosition[1] : weaponPosition[0];
        Vector3 weaponStartPosition = isFacingRight
            ? -weaponStartPositionTransform.right
            : weaponStartPositionTransform.right;

        Vector3 WeaponStartPositionCrosshairCurrentPositionVector =
            crosshairGameObject.transform.position - weaponStartPositionTransform.position;

        float hasReachedOtherSide =
            crosshairGameObject.transform.position.y < weaponStartPositionTransform.position.y ? -1 : 1;
        float _viewAngle = Vector2.Angle(weaponStartPosition, WeaponStartPositionCrosshairCurrentPositionVector) *
                           hasReachedOtherSide;

        return _viewAngle;
    }

    private float CalculateCrosshairYAngle(float _crosshairYAngle, bool _isInvert)
    {
        //Invert fuck shit
        _crosshairYAngle += _isInvert
            ? -playerMovement.mouseYAxisInput * crosshairVisualSpeed
            : playerMovement.mouseYAxisInput * crosshairVisualSpeed;
        _crosshairYAngle = Mathf.Clamp(_crosshairYAngle, -maximumCastWeaponAngle, maximumCastWeaponAngle);

        return _crosshairYAngle;
    }

    private bool CheckBlockState()
    {
        //var isPressingBlockKey = Input.GetKey(blockKeyCode);
        return false;
    }

    private bool CheckCanChakraRegenerate()
    {
        bool hasTimePassedToRegenerateChakra = timeBtwChakraRegeneratingProcedure <= 0;
        bool doesChakraNeedRegeneration = chakra < maximumChakra;
        return hasTimePassedToRegenerateChakra && doesChakraNeedRegeneration;
    }

    private bool CheckObjectOrientation()
    {
        return playerAnimation.gameObject.transform.localScale.x > 0;
    }

    private bool CheckComboState()
    {
        return !playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }

    private bool CheckComboFollowUpState()
    {
        //
        bool isLastAnimationState =
            playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).IsName("ThirdPunch");
        bool isIdleAnimationState = playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        bool hasPunchTimeLeft = punchAnimationTimeLeft < 0;
        bool hasComboFollowUpAllowed = playerAnimation.canFollowUpCombo;

        return !isLastAnimationState && !isIdleAnimationState && hasPunchTimeLeft && hasComboFollowUpAllowed;
    }

    private void CheckBusyBooleanStatement()
    {
        isBusy = playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Action");

        if (isBusy || isStaggered) //freeze if staggered or action is taking place
            playerMovement.ChangeRigidbodyState(true, false, playerMovement.rigidbody2D);
    }

    public void TakeInjury(int _damage)
    {
        switch (isBlocking)
        {
            case true:
                chakra--;
                break;
            case false:
                healthPoints -= _damage;
                break;
        }
    }

    public void TakeInjury(int _damage, bool _isTurnSuccessful)
    {
        switch (isBlocking)
        {
            case true:
                chakra--;
                break;
            case false:
                healthPoints -= _damage;
                break;
        }

        _isTurnSuccessful = true;
    }

    public void DepleteChakraWithRate(int _chakraDepletionRate)
    {
        chakra -= _chakraDepletionRate;
        //Resets chakra cooldowns timer, once chakra is consumed
        timeBtwChakraRegeneratingProcedure = maximumTimeBtwChakraRegeneratingProcedure;
    }

    public void AssingNewPrimaryWeapon(WeaponTemplate _primaryWeapon)
    {
        activePrimaryWeapon = _primaryWeapon;
        GameObject _uiGameObject = GameObject.FindGameObjectWithTag("UI");
        _uiGameObject.GetComponent<UIManager>().ReplacePrimaryWeaponUIIcon(activePrimaryWeapon.weaponSprite);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponPosition[0].position, attackRadius);
        Gizmos.DrawWireSphere(weaponPosition[1].position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            new Vector3(transform.position.x + shadowPositionXOffset, transform.position.y - shadowPositionYOffset),
            new Vector3(transform.position.x + shadowPositionXOffset, (transform.position.y - 150)));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //This is when the bottomcollider (lol) detects an enemy, when enabled
            //Used by AirAttack
            col.GetComponent<EnemyBehavior>().TakeInjury(kickDamage);
        }

        if (col.CompareTag("Weapon") && col.GetComponent<SecondaryWeaponContainer>())
        {
            switch (col.GetComponent<SecondaryWeaponContainer>().canBePickedUp)
            {
                case true:

                    break;
                case false:
                    healthPoints -= col.GetComponent<SecondaryWeaponContainer>().weaponDamage;
                    break;
            }
        }

        switch (activePrimaryWeapon != null)
        {
            case true:
                // if (col.CompareTag("PrimaryWeapon") && Input.GetKeyDown(weaponSwapKeycode))
                // {
                //     AssingNewPrimaryWeapon(col.GetComponent<PrimaryWeaponContainer>().primaryWeapon);
                //     Destroy(col.gameObject);
                // }

                break;
            case false:
                if (col.CompareTag("PrimaryWeapon"))
                {
                    AssingNewPrimaryWeapon(col.GetComponent<PrimaryWeaponContainer>().primaryWeapon);
                    Destroy(col.gameObject);
                }

                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PrimaryWeapon"))
        {
            //Set UI feedback disabled
        }
    }
}