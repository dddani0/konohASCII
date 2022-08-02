using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public EnemyTemplate enemyTemplate;
    [Space] 
    public EnemyMovement enemyMovement;
    public EnemyAnimation enemyAnimation;
    [Space]
    public string enemyName;
    [Space]
    public int maximumHealth;
    [SerializeField] private int health;
    [Space] 
    [SerializeField] private bool isEnemyDetected;
    public bool isStationary;
    public int enemyLevel;
    [Header("Enemy Behavior type variables")]
    [SerializeField] private Transform[] detectionPoints;
    [SerializeField] private float detectionPointSize;
    [Space] 
    public GameObject target;
    [Space] 
    [SerializeField] private bool isFacingRight;
    
    void Start()
    {
        FetchDataFromTemplate(enemyTemplate);
    }
    
    void Update()
    {
        isFacingRight = CheckObjectOrientation();
        switch (isStationary)
        {
            case true:
                StationaryEnemy();
                break;
            case false:
                NonStationaryEnemy();
                break;
        }
    }

    private void LateUpdate()
    {
        
    }

    private void FixedUpdate()
    {
        DetectEnemy();
    }

    private void NonStationaryEnemy()
    {
        switch (isEnemyDetected)
        {
            case true:

                break;
            case false:
                
                break;
        }
    }
    
    private void StationaryEnemy()
    {
        
    }

    private void DetectEnemy()
    {
        if (!isEnemyDetected)
        {
            RaycastHit2D hitcol = new RaycastHit2D();
            switch (isFacingRight)
            {
                case true: //Facing Right
                    hitcol = Physics2D.CircleCast(detectionPoints[1].transform.position, detectionPointSize,detectionPoints[1].right);
                    break;
                case false: //Facing left
                    hitcol = Physics2D.CircleCast(detectionPoints[0].transform.position, detectionPointSize,-detectionPoints[0].right);
                    break;
            }
            if (hitcol.collider.gameObject.name == "Player")
            {
                print($"{hitcol.collider.gameObject.name} detected");
                target = hitcol.collider.gameObject;
                isEnemyDetected = true;
            }
        }
    }

    private bool CheckObjectOrientation()
    {
        return gameObject.transform.localScale.x < 0;
    }
    
    public void TakeInjury(int _damage)
    {
        health -= _damage;
        print($"old hp {health + _damage} | new hp {health}");
    }
    private void FetchDataFromTemplate(EnemyTemplate _enemyTemplate)
    {
        enemyName = _enemyTemplate.enemyName;
        maximumHealth = _enemyTemplate.maximumHealth;
        health = maximumHealth;
        isStationary = _enemyTemplate.isStationary;
        enemyLevel = _enemyTemplate.enemyComplexityLevel;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<SecondaryWeaponContainer>() != null)
            health -= col.GetComponent<SecondaryWeaponContainer>().weaponDamage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(detectionPoints[0].position,detectionPointSize);
    }
}
