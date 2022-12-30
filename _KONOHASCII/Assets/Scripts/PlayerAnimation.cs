using System;
using UnityEngine;

//Prerequisites components:
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAction))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerAction playerAction;
    [Header("Animator")] public Animator defaultAnimator;
    public bool canFollowUpCombo;

    void Start()
    {
        FetchRudimentaryVariables();
    }

    void Update()
    {
        SetAnimationState("movement", int.TryParse(playerMovement.xMovementAxisInput.ToString(), out int discardNumber),
            defaultAnimator);
        SetAnimationState("ydelta", playerMovement.rigidbody2D.velocity.y, defaultAnimator);
        SetAnimationState("nextFrameDeadline", playerAction.punchAnimationTimeLeft, defaultAnimator);
    }

    private void LateUpdate()
    {
        if (!playerAction.isBusy)
            PlayerSpriteRotation();
    }

    private void PlayerSpriteRotation()
    {
        float _movementInput = !playerMovement.isGripped
            ? playerMovement.xMovementAxisInput
            : playerMovement.yMovementAxisInput = Math.Abs(playerAction.transform.rotation.y - 90) < 0.001f
                ? playerMovement.yMovementAxisInput * -1f
                : playerMovement.yMovementAxisInput * 1f; //flip direction to match
        //Rotates sprite based on the horizontal input value
        //thus: greater than 0 = right; and less than 0 = left;
        if (_movementInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (_movementInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FetchRudimentaryVariables()
    {
        //Calls once on scene load, which fetches the important values from the gameobjects
        defaultAnimator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerAction = GetComponentInParent<PlayerAction>();
    }

    #region Set this.player animation properties

    public void SetAnimationState(string parametername, int integervalue, Animator _animator)
    {
        _animator.SetInteger(parametername, integervalue);
    }

    public void SetAnimationState(string parametername, Animator _animator)
    {
        _animator.SetTrigger(parametername);
    }

    public void SetAnimationState(string parametername, float floatvalue, Animator _animator)
    {
        _animator.SetFloat(parametername, floatvalue);
    }

    public void SetAnimationState(string parametername, bool booleanvalue, Animator _animator)
    {
        _animator.SetBool(parametername, booleanvalue);
    }

    #endregion

    public void ACALLANIMATIONEVENTInflictDamage()
    {
        print("inflict damage called");
        //Animation event
        //Takes damage if raycast detects an object with EnemyBehavior.cs script attached
        //Tagging is not necessary, if it can access the above-mentioned script
        Transform _attackPosition = null;
        int _damage;
        float _attackRadius = playerAction.attackRadius;
        bool _isPrimaryWeaponEquipped = playerAction.activePrimaryWeapon;
        if (_isPrimaryWeaponEquipped)
            _damage = playerAction.activePrimaryWeapon.damage;
        else
            _damage = playerAction.fistDamage;
        switch (defaultAnimator.GetCurrentAnimatorStateInfo(0).IsName("fall") ||
                defaultAnimator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
        {
            case true:
                playerAction.weaponPosition[2].GetComponent<BoxCollider2D>().enabled = true;
                break;
            case false:
                switch (playerAction.isFacingRight)
                {
                    case true:
                        _attackPosition = playerAction.weaponPosition[0];
                        break;
                    case false:
                        _attackPosition = playerAction.weaponPosition[1];
                        break;
                }

                break;
        }


        Collider2D hit = Physics2D.OverlapCircle(_attackPosition.position, _attackRadius);
        if (hit.GetComponent<EnemyBehavior>() != null)
            hit.GetComponent<EnemyBehavior>().TakeInjury(_damage);
    }

    public void ACALLANIMATIONEVENTResetAirAttackCollider()
    {
        playerAction.weaponPosition[2].GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ACALLANIMATIONEVENTSignalComboFollowUp()
    {
        //Signals the playeraction script, that the player can follow up the combo.
        canFollowUpCombo = true;
    }

    public void CALLANIMATIONEVENTSignalComboOff()
    {
        //Tells the playeraction script, if the player cannot proceed with combo
        canFollowUpCombo = false;
    }
}