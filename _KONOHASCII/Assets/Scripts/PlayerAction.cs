using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Resources")]
    public PlayerMovement playermovement;
    public PlayerAnimation playeranimation;
    [Space(20f)]
    [Header("Weapon_Attribute")]
    public WeaponSource currentWeaponSource;
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
    [Space(20f)] 
    [Header("Layermasks and Button mapping")]
    public LayerMask enemylayer;
    [Space]
    public KeyCode attackKeycode;
    public KeyCode rangeAttackKeycode; 

    void Update()
    {
        CheckBusyBooleanStatement();
        CQCAttack();
        RangeAttack();
    }

    private void CQCAttack()
    {
        if (Input.GetKey(attackKeycode) && !isBusy)
            playeranimation.SetAnimationState("attack",playeranimation.default_animator);
    }

    private void RangeAttack()
    {
        //Creates instance of weapon prefab.
        //Modifies said instance from selected asset.
        
        if (Input.GetKey(rangeAttackKeycode) && !isBusy && !playeranimation.default_animator.GetCurrentAnimatorStateInfo(0).IsName("wallgrip"))
        {
            playeranimation.SetAnimationState("range_attack",playeranimation.default_animator);
            GameObject t_weapon;
            switch (playeranimation.gameObject.transform.localScale.x > 0)
            {
                case true:
                    t_weapon = Instantiate(
                        GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>().weapon_container,
                        weaponPosition[0].position, weaponPosition[0].rotation);
                    t_weapon.GetComponent<WeaponContainer>().AssignNewWeapon(currentWeaponSource,1);
                    break;
                case false:
                    t_weapon = Instantiate(
                        GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>().weapon_container,
                        weaponPosition[1].position, weaponPosition[1].rotation);
                    t_weapon.GetComponent<WeaponContainer>().AssignNewWeapon(currentWeaponSource,-1);
                    break;
            }

            
        }
    }

    private void CheckBusyBooleanStatement()
    {
        isBusy = playeranimation.default_animator.GetCurrentAnimatorStateInfo(0).IsTag("Action");
        
        if (isBusy || isStaggered) //freeze if staggered or action is taking place
            playermovement.ChangeRigidbodyState(true,false,playermovement.rigidbody2D);
    }
}