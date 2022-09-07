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
    
    public void SetAnimationState(string _parametername, int _integervalue, Animator _animator)
    {
        _animator.SetInteger(_parametername, _integervalue);
    }

    public void SetAnimationState(string _parametername, Animator _animator)
    {
        _animator.SetTrigger(_parametername);
    }

    public void SetAnimationState(string _parametername, float _floatvalue, Animator _animator)
    {
        _animator.SetFloat(_parametername, _floatvalue);
    }

    public void SetAnimationState(string _parametername, bool _booleanvalue, Animator _animator)
    {
        _animator.SetBool(_parametername, _booleanvalue);
    }
}