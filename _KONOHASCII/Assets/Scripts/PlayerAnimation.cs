using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerMovement player_movement;
    public PlayerActionAttribute playerActionAttribute;
    [Space(20f)]
    [Header("Animator")]
    public Animator default_animator;
    public AnimatorOverrideController animator_controller;

    void Start()
    {
        default_animator.runtimeAnimatorController = animator_controller;
    }

    void Update()
    {
        SetAnimationState("movement", int.Parse(Input.GetAxisRaw("Horizontal").ToString()),default_animator);
        SetAnimationState("ydelta",player_movement.rigidbody2D.velocity.y,default_animator);
    }

    private void LateUpdate()
    {
        if (!playerActionAttribute.isBusy && !player_movement.isGripped)
            PlayerSpriteRotation(); // After jumping, the player refuses to remain to face the left area
        else
            print("didn't run");
    }

    private void PlayerSpriteRotation()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
            transform.localEulerAngles = new Vector3(0, 0, 0);
        else if (Input.GetAxisRaw("Horizontal") < 0)
            transform.localEulerAngles = new Vector3(0, 180, 0);
    }


    public void SetAnimationState(string parametername, int integervalue, Animator _animator)
    {
        _animator.SetInteger(parametername,integervalue);
    }
    
    public void SetAnimationState(string parametername, Animator _animator)
    {
        _animator.SetTrigger(parametername);
    }
    
    public void SetAnimationState(string parametername, float floatvalue, Animator _animator)
    {
        _animator.SetFloat(parametername,floatvalue);
    }
    
    public void SetAnimationState(string parametername, bool booleanvalue, Animator _animator)
    {
        _animator.SetBool(parametername,booleanvalue);
    }
}