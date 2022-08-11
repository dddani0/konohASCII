using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Resources")] public GameObject gamemanager;
    public PlayerMovement playerMovement;
    public PlayerAnimation playerAnimation;

    [Space(20f)] [Header("Weapon_Attribute")] [SerializeField]
    private bool weaponSwapped;

    [Tooltip("Primary weapon is always close range weapon. Like Katana. If null, will attribute as fist attack")]
    public WeaponTemplate activePrimaryWeapon;

    [Space] public int fistDamage;
    [Space] public int attackRadius;
    [Space] public bool isCombo;
    [SerializeField] private bool canProceedWithCombo;
    public float punchAnimationTimeLeft;

    [Tooltip("Secondary weapon is always throwable. Like Shuriken")]
    public WeaponTemplate activeSecondaryWeaponTemplate;

    [Space] public Transform[] weaponPosition;
    [Space] public int rangeWeaponAmmunition;
    [Space(20f)] [Header("Health")] public float maximumHealthPoints;
    [SerializeField] private float healthPoints;
    [Space(20f)] [Header("Chakra")] public float maximumChakra;
    [SerializeField] private float chakra;
    [Space(20f)] [Header("Brakes")] public bool isBusy;
    public bool isStaggered;
    public bool isFacingRight;

    [Space(20f)] [Header("Layermasks and Button mapping")]
    public LayerMask enemylayer;

    [Space] public KeyCode attackKeycode;
    public KeyCode rangeAttackKeycode;
    public KeyCode weaponSwapKeycode;

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

        isFacingRight = CheckObjectOrientation();
        CheckBusyBooleanStatement();
        PrimaryShortRangeAttack();
        RangeAttack();
    }

    private void LateUpdate()
    {
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
    }

    private void PrimaryShortRangeAttack()
    {
        switch (isCombo)
        {
            case true:
                switch (activePrimaryWeapon == null)
                {
                    case true:
                        if (Input.GetKey(attackKeycode) && canProceedWithCombo)
                        {
                            playerAnimation.SetAnimationState("attack", playerAnimation.defaultAnimator);
                            punchAnimationTimeLeft =
                                playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
                        }

                        break;
                    case false:
                        if (Input.GetKey(attackKeycode) && canProceedWithCombo)
                        {
                            playerAnimation.SetAnimationState("weapon_attack", playerAnimation.defaultAnimator);
                            punchAnimationTimeLeft =
                                playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
                        }

                        break;
                }

                break;
            case false:
                switch (activePrimaryWeapon == null)
                {
                    case true:
                        if (Input.GetKey(attackKeycode) && !isBusy)
                        {
                            playerAnimation.SetAnimationState("attack", playerAnimation.defaultAnimator);
                            isCombo = true;
                            punchAnimationTimeLeft =
                                playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
                        }

                        break;
                    case false:
                        if (Input.GetKey(attackKeycode) && !isBusy)
                        {
                            playerAnimation.SetAnimationState("weapon_attack", playerAnimation.defaultAnimator);
                            isCombo = true;
                            punchAnimationTimeLeft =
                                playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
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

        if (Input.GetKey(rangeAttackKeycode) && !isBusy &&
            !playerAnimation.defaultAnimator.GetCurrentAnimatorStateInfo(0).IsName("wallgrip"))
        {
            playerAnimation.SetAnimationState("range_attack", playerAnimation.defaultAnimator);
            GameObject t_weapon;
            switch (isFacingRight)
            {
                case true:
                    t_weapon = Instantiate(
                        GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>().weapon_container,
                        weaponPosition[0].position, weaponPosition[0].rotation);
                    t_weapon.GetComponent<SecondaryWeaponContainer>().AssignNewWeapon(activeSecondaryWeaponTemplate, 1);
                    break;
                case false:
                    t_weapon = Instantiate(
                        GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>().weapon_container,
                        weaponPosition[1].position, weaponPosition[1].rotation);
                    t_weapon.GetComponent<SecondaryWeaponContainer>()
                        .AssignNewWeapon(activeSecondaryWeaponTemplate, -1);
                    break;
            }
        }
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

    public void AssingNewPrimaryWeapon(WeaponTemplate _primaryWeapon)
    {
        activePrimaryWeapon = _primaryWeapon;
        GameObject _uiGameObject = GameObject.Find("UI");
        _uiGameObject.GetComponent<UIManager>().ReplacePrimaryWeaponUIIcon(activePrimaryWeapon.weaponSprite);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponPosition[0].position, attackRadius);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (activePrimaryWeapon != null)
        {
            case true:
                if (col.CompareTag("PrimaryWeapon") && Input.GetKeyDown(weaponSwapKeycode))
                {
                    AssingNewPrimaryWeapon(col.GetComponent<PrimaryWeaponContainer>().primaryWeapon);
                    Destroy(col.gameObject);
                }

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