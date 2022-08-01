using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Resources")] public GameObject gamemanager;
    public PlayerMovement playermovement;
    public PlayerAnimation playeranimation;
    [Space(20f)]
    [Header("Weapon_Attribute")]
    [Tooltip("Primary weapon is always close range weapon. Like Katana. If null, will attribute as fist attack")]
    public WeaponTemplate activePrimaryWeaponTemplate;
    [Space]
    public int fistDamage;
    [Space] 
    public int attackRadius;
    [Tooltip("Secondary weapon is always throwable. Like Shuriken")]
    public WeaponTemplate activeSecondaryWeaponTemplate;
    [Space] 
    public Transform[] weaponPosition;
    [Space] 
    public int rangeWeaponAmmunition;
    [Space(20f)] 
    [Header("Health")] 
    public float maximumHealthPoints = 750;
    [SerializeField] private float healthPoints;
    [Space(20f)] 
    [Header("Chakra")] 
    public float maximumChakra = 12;
    [SerializeField] private float chakra;
    [Space(20f)] 
    [Header("Brakes")] 
    public bool isBusy;
    public bool isStaggered;
    public bool isFacingRight;
    [Space(20f)] 
    [Header("Layermasks and Button mapping")]
    public LayerMask enemylayer;
    [Space]
    public KeyCode attackKeycode;
    public KeyCode rangeAttackKeycode;

    private void Start()
    {
        //AssingNewPrimaryWeapon(activePrimaryWeaponTemplate);
    }

    void Update()
    {
        isFacingRight = CheckObjectOrientation();
        CheckBusyBooleanStatement();
        PrimaryShortRangeAttack();
        RangeAttack();
    }

    private void PrimaryShortRangeAttack()
    {
        switch (activePrimaryWeaponTemplate == null)
        {
            case true:
                if (Input.GetKey(attackKeycode) && !isBusy)
                    playeranimation.SetAnimationState("attack",playeranimation.default_animator);
                break;
            case false:
                if (Input.GetKey(attackKeycode) && !isBusy)
                    playeranimation.SetAnimationState("weapon_attack",playeranimation.default_animator);
                break;
        }
    }

    private void RangeAttack()
    {
        //Creates instance of weapon prefab.
        //Modifies said instance from selected asset.
        
        if (Input.GetKey(rangeAttackKeycode) && !isBusy && !playeranimation.default_animator.GetCurrentAnimatorStateInfo(0).IsName("wallgrip"))
        {
            playeranimation.SetAnimationState("range_attack",playeranimation.default_animator);
            GameObject t_weapon;
            switch (isFacingRight)
            {
                case true:
                    t_weapon = Instantiate(
                        GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>().weapon_container,
                        weaponPosition[0].position, weaponPosition[0].rotation);
                    t_weapon.GetComponent<SecondaryWeaponContainer>().AssignNewWeapon(activeSecondaryWeaponTemplate,1);
                    break;
                case false:
                    t_weapon = Instantiate(
                        GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>().weapon_container,
                        weaponPosition[1].position, weaponPosition[1].rotation);
                    t_weapon.GetComponent<SecondaryWeaponContainer>().AssignNewWeapon(activeSecondaryWeaponTemplate,-1);
                    break;
            }

            
        }
    }

    private bool CheckObjectOrientation()
    {
        return playeranimation.gameObject.transform.localScale.x > 0;
    }
    
    private void CheckBusyBooleanStatement()
    {
        isBusy = playeranimation.default_animator.GetCurrentAnimatorStateInfo(0).IsTag("Action");
        
        if (isBusy || isStaggered) //freeze if staggered or action is taking place
            playermovement.ChangeRigidbodyState(true,false,playermovement.rigidbody2D);
    }

    private void AssingNewPrimaryWeapon(WeaponTemplate _primaryWeapon)
    {
        gamemanager.GetComponent<Gamemanager>().uimanager.primaryWeaponIcon.sprite =
            _primaryWeapon.weaponSprite;
        gamemanager.GetComponent<Gamemanager>().uimanager.secondaryWeaponIcon.sprite =
            _primaryWeapon.weaponSprite;
        gamemanager.GetComponent<Gamemanager>().uimanager.primaryWeaponIcon.SetNativeSize();
        gamemanager.GetComponent<Gamemanager>().uimanager.secondaryWeaponIcon.SetNativeSize();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponPosition[0].position,attackRadius);
    }
}