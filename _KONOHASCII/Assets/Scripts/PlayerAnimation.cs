using UnityEngine;
using UnityEngine.Serialization;

//Prerequisites components:
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAction))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerAction playerAction;
    [Space(20f)] [Header("Animator")] public Animator default_animator;
    public AnimatorOverrideController animator_controller;

    void Start()
    {
        FetchRudimentaryVariables();
    }

    void Update()
    {
        SetAnimationState("movement", int.Parse(Input.GetAxisRaw("Horizontal").ToString()), default_animator);
        SetAnimationState("ydelta", playerMovement.rigidbody2D.velocity.y, default_animator);
    }

    private void LateUpdate()
    {
        if (!playerAction.isBusy && !playerMovement.isGripped)
            PlayerSpriteRotation(); 
    }

    private void PlayerSpriteRotation()
    {
        //Rotates sprite based on the horizontal input value
        //thus: greater than 0 = right; and less than 0 = left;
        if (Input.GetAxis("Horizontal") > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (Input.GetAxis("Horizontal") < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FetchRudimentaryVariables()
    {
        //Calls once on scene load, which fetches the important values from the gameobjects
        default_animator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerAction = GetComponentInParent<PlayerAction>();
        default_animator.runtimeAnimatorController = animator_controller;
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
        //Animation event, which is called everytime the player 
        //Takes damage if raycast detects an object with EnemyBehavior.cs script
        //Tagging is not necessary, if it can access the above-mentioned script
        Transform _attackPosition = null;
        int _damage;
        float _attackRadius = playerAction.attackRadius;
        if (playerAction.activePrimaryWeaponTemplate != null)
            _damage = playerAction.activePrimaryWeaponTemplate.damage;
        else
            _damage = playerAction.fistDamage;
        switch (playerAction.isFacingRight)
        {
            case true:
                _attackPosition = playerAction.weaponPosition[0];
                break;
            case false:
                _attackPosition = playerAction.weaponPosition[1];
                break;
        }
        Collider2D hit = Physics2D.OverlapCircle(_attackPosition.position, _attackRadius);
        if (hit.GetComponent<EnemyBehavior>() != null)
            hit.GetComponent<EnemyBehavior>().TakeInjury(_damage);
    }
}