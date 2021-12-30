using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator default_animator;
    public AnimatorOverrideController animatorcontroller;

    void Start()
    {
        default_animator.runtimeAnimatorController = animatorcontroller;
    }

    void Update()
    {
        SetAnimationState("int","movement",Input.GetAxisRaw("Horizontal").ToString(),default_animator);
    }

    private void LateUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
            transform.localEulerAngles = new Vector3(0, 0, 0);
        else if (Input.GetAxisRaw("Horizontal") < 0)
            transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    public void SetAnimationState(string parametertype, string parametername, string value,Animator _animator)
    {
        //float - Needs value
        //int - Needs value
        //bool - Needs value
        //trigger - Doesn't need value

        switch (parametertype.ToLower())
        {
            case "float":
                _animator.SetFloat(parametername,int.Parse(value));
                break;
            case "int":
                _animator.SetInteger(parametername, int.Parse(value));
                break;
            case "bool":
                _animator.SetBool(parametername,bool.Parse(value));
                break;
            case "trigger":
                _animator.SetTrigger(parametername);
                break;
        }
    }
}