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
        if (enemyBehavior.target)
            SpriteRotation();
    }

    private void SpriteRotation()
    {
        if (enemyBehavior.target.transform.position.x < enemyBehavior.gameObject.transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }
}