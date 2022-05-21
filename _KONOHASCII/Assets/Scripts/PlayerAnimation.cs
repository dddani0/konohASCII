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
    [Space(20f)] [Header("Animator")] 
    public Animator default_animator;
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
            PlayerSpriteRotation(); // After jumping, the player refuses to remain to face the left area
    }

    private void PlayerSpriteRotation()
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (Input.GetAxis("Horizontal") < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FetchRudimentaryVariables()
    {
        default_animator = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerAction = GetComponentInParent<PlayerAction>();
        default_animator.runtimeAnimatorController = animator_controller;
    }

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
}