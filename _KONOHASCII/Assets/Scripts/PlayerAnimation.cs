using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimation : MonoBehaviour
{
    [FormerlySerializedAs("playerMovement")] [Header("Resources")] 
    public PlayerMovement playermovement;
    public PlayerActionAttribute playerActionAttribute;
    [Space(20f)]
    [Header("Animator")]
    public Animator default_animator;
    public AnimatorOverrideController animatorcontroller;

    void Start()
    {
        default_animator.runtimeAnimatorController = animatorcontroller;
    }

    void Update()
    {
        SetAnimationState("movement", int.Parse(Input.GetAxisRaw("Horizontal").ToString()),default_animator);
        SetAnimationState("ydelta",playermovement.rigidbody2D.velocity.y,default_animator);
    }

    private void LateUpdate()
    {
        if (!playerActionAttribute.isBusy && !playermovement.isGripped)
            PlayerSpriteRotation();
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