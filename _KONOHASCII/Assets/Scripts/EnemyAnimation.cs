using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public EnemyBehavior enemyBehavior;
    [Space] public Animator defaultAnimator;
    public AnimatorOverrideController animatorController;

    void Start()
    {
        FetchRudimentaryValues();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        if (enemyBehavior.targetList.Count > 0)
            SpriteRotation();
    }

    private void SpriteRotation()
    {
        if (enemyBehavior.mainTarget.transform.position.x < enemyBehavior.gameObject.transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void FetchRudimentaryValues()
    {
        defaultAnimator = GetComponent<Animator>();
        enemyBehavior = GetComponentInParent<EnemyBehavior>();
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