using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public EnemyBehavior enemyBehavior;
    public EnemyMovement enemyMovement;
    [Space] public Animator defaultAnimator;
    public AnimatorOverrideController animatorController;

    void Start()
    {
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
}