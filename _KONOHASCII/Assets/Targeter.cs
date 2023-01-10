using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Targeter : MonoBehaviour
{
    public float autoTargetMaximumCooldown;
    private float autoTargetCooldown;
    [Space] [Range(0, 25)] public float targetRange;

    [Space] public GameObject target;
    
    void Start()
    {
        FetchRudimentaryValues();
    }

    private void FixedUpdate()
    {
        AutoTarget();
    }

    private void AutoTarget()
    {
        autoTargetCooldown = FetchCooldown(autoTargetCooldown);
        if (autoTargetCooldown <= 0)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, targetRange);
            List<Collider2D> targetEntity = new List<Collider2D>();
            foreach (var col in cols)
            {
                if (col.tag.ToUpper().Contains("TARGET")) targetEntity.Add(col);
            }

            target = cols.Length > 1 ? cols[0].gameObject : FetchTarget(targetEntity);
        }
    }

    private void FetchRudimentaryValues()
    {
        autoTargetCooldown = autoTargetMaximumCooldown;
    }

    private float FetchCooldown(float _currentCooldown)
    {
        float _cooldown = _currentCooldown;
        if (_currentCooldown <= 0) return autoTargetMaximumCooldown;
        return _cooldown -= Time.deltaTime;
    }

    private GameObject FetchTarget(List<Collider2D> targets)
    {
        //From a list of porential targets, return the closest enemy, which'll be the closest target
        GameObject _closestTarget()
        {
            GameObject nearestTarget = targets[0].gameObject;
            int nearestTargetIndex = 0;
            float smallestTargetDifference = 0;
            for (int i = 0; i < targets.Count; i++)
            {
                var dif = Mathf.Abs((targets[i].transform.position - transform.position).magnitude);
                smallestTargetDifference = dif < smallestTargetDifference ? dif : smallestTargetDifference;
                nearestTargetIndex = dif < smallestTargetDifference ? i : nearestTargetIndex;
            }

            return targets[nearestTargetIndex].gameObject;
        }

        return _closestTarget();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRange);
    }
}