using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Resources")] public EnemyTemplate enemyTemplate;
    [Space] public EnemyMovement enemyMovement;
    public EnemyAnimation enemyAnimation;
    [Space] public Gamemanager gamemanager;
    [Space] public WeaponTemplate secondaryWeapon;
    [Space] public string enemyName;
    [Space] public int maximumHealth;
    [SerializeField] private int health;
    [Space] [SerializeField] private bool isEnemyDetected;
    public bool isStationary;
    public int enemyLevel;
    [Space] public Rigidbody2D enemyRigidbody2D;

    [Header("Enemy Behavior type variables")]
    private RaycastHit2D hitcol;

    public float maximumTimeBtwEnemyChecks;
    [SerializeField] private float timeBtwEnemyChecks;
    public Transform[] detectionPoints;
    public Transform[] attackPoints;
    [Space] public GameObject targetCrosshairGameObject;
    public int crosshairMaximumHeight;
    public Vector3 crosshairOffset;
    [Space] public float deTargetMaximumDistance;
    [SerializeField] private float detectionPointSize;
    [Space] public List<GameObject> targetList;
    public GameObject mainTarget;
    [Space] public bool isFacingRight;
    [Space(10)] [SerializeField] private bool isMainTargetWithinRadius;

    void Start()
    {
        FetchDataFromTemplate(enemyTemplate);
        FetchRudimentaryValues();
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
        isEnemyDetected = CheckDetection();
    }

    private void FixedUpdate()
    {
        DetectEnemy();
    }

    private void NonStationaryEnemy()
    {
        //See the list of non-stationary basic enemies at the issue
        // https://github.com/marloss/konohASCII/issues/4
        switch (isEnemyDetected)
        {
            case true:
                if (!mainTarget)
                    mainTarget = DetermineMainTarget();
                targetCrosshairGameObject.transform.position = DetermineTargetCrosshair();
                CastWeapon();
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
        switch (isEnemyDetected)
        {
            case true:
                if (timeBtwEnemyChecks <= 0)
                {
                    print("checked");
                    hitcol = new RaycastHit2D();
                    switch (isFacingRight)
                    {
                        case true: //Facing Right
                            hitcol = Physics2D.CircleCast(detectionPoints[1].transform.position, detectionPointSize,
                                detectionPoints[1].right);
                            break;
                        case false: //Facing left
                            hitcol = Physics2D.CircleCast(detectionPoints[0].transform.position, detectionPointSize,
                                -detectionPoints[0].right);
                            break;
                    }
                }
                else
                {
                    timeBtwEnemyChecks -= Time.deltaTime;
                }

                break;
            case false:
                //Raycasts without cooldown
                hitcol = new RaycastHit2D();
                switch (isFacingRight)
                {
                    case true: //Facing Right
                        hitcol = Physics2D.CircleCast(detectionPoints[1].transform.position, detectionPointSize,
                            detectionPoints[1].right);
                        break;
                    case false: //Facing left
                        hitcol = Physics2D.CircleCast(detectionPoints[0].transform.position, detectionPointSize,
                            -detectionPoints[0].right);
                        break;
                }

                break;
        }

        if (hitcol.collider.gameObject.name.Contains("Player") && !targetList.Contains(hitcol.collider.gameObject))
        {
            print($"{hitcol.collider.gameObject.name} detected");
            targetList.Add(hitcol.collider.gameObject);
            //Only temporarly, to check for enemy
            mainTarget = hitcol.collider.gameObject;
        }
    }

    private void CastWeapon()
    {
        GameObject _temporaryWeapon = null;
        switch (isFacingRight)
        {
            case true:
                _temporaryWeapon = Instantiate(gamemanager.GetComponent<Gamemanager>().weapon_container,
                    attackPoints[1].position, attackPoints[1].rotation);
                _temporaryWeapon.GetComponent<SecondaryWeaponContainer>()
                    .AssignNewWeapon(secondaryWeapon, CalculateWeaponAngle(), 1);
                break;
            case false:
                _temporaryWeapon = Instantiate(gamemanager.GetComponent<Gamemanager>().weapon_container,
                    attackPoints[0].position, attackPoints[0].rotation);
                _temporaryWeapon.GetComponent<SecondaryWeaponContainer>()
                    .AssignNewWeapon(secondaryWeapon, CalculateWeaponAngle(), -1);
                break;
        }
    }

    private GameObject DetermineMainTarget()
    {
        GameObject _mainTarget = null;
        switch (targetList.Count > 1)
        {
            case true:

                break;
            case false:

                mainTarget = targetList[0];
                break;
        }

        return _mainTarget;
    }

    private float CalculateWeaponAngle()
    {
        Transform weaponStartPositionTransform = isFacingRight ? attackPoints[1] : attackPoints[0];
        Vector3 weaponStartPosition =
            isFacingRight ? -weaponStartPositionTransform.right : weaponStartPositionTransform.right;

        Vector3 WeaponStartPositionCrosshairCurrentPositionVector =
            targetCrosshairGameObject.transform.position - weaponStartPosition;

        float hasReachedOtherSide = targetCrosshairGameObject.transform.position.y < weaponStartPosition.y ? -1 : 1;

        float _viewAngle = Vector2.Angle(weaponStartPosition, WeaponStartPositionCrosshairCurrentPositionVector) *
                           hasReachedOtherSide;

        return _viewAngle;
    }

    private Vector3 DetermineTargetCrosshair()
    {
        float _targetCrosshairYPosition = Mathf.Clamp(mainTarget.transform.position.y, -crosshairMaximumHeight,
            crosshairMaximumHeight);
        Vector3 _crosshairPosition = isFacingRight
                ? new Vector3((transform.position.x + crosshairOffset.x) * -1,
                    _targetCrosshairYPosition)
                : new Vector3((transform.position.x + crosshairOffset.x),
            _targetCrosshairYPosition);
        return _crosshairPosition;
    }

    private bool CheckDetection()
    {
        bool isMainTargetDetected = mainTarget;
        return isMainTargetDetected;
    }

    private bool CheckObjectOrientation()
    {
        return enemyAnimation.gameObject.transform.localScale.x < 0;
    }

    public void TakeInjury(int _damage)
    {
        health -= _damage;
        print($"old hp {health + _damage} | new hp {health}");
    }

    public void TakeInjury(int _damage, int _knockbackForce)
    {
        health -= _damage;
        // switch (isFacingRight)
        // {
        //     case true:
        //         
        //         break;
        //     case false:
        //         
        //         break;
        // }
    }

    private void FetchDataFromTemplate(EnemyTemplate _enemyTemplate)
    {
        enemyName = _enemyTemplate.enemyName;
        maximumHealth = _enemyTemplate.maximumHealth;
        health = maximumHealth;
        isStationary = _enemyTemplate.isStationary;
        enemyLevel = _enemyTemplate.enemyComplexityLevel;
        secondaryWeapon = _enemyTemplate.weapon;
    }

    private void FetchRudimentaryValues()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        gamemanager = GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>();
        timeBtwEnemyChecks = maximumTimeBtwEnemyChecks;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<SecondaryWeaponContainer>())
        {
            //Damages the enemy
            health -= col.GetComponent<SecondaryWeaponContainer>().weaponDamage;
            //Visual implementation:
            //The weapon, which hits the enemy, stays on the enemy.
            //Makes it more dramatic.
            SecondaryWeaponContainer temporaryWeapon = col.gameObject.GetComponent<SecondaryWeaponContainer>();
            temporaryWeapon.transform.SetParent(enemyAnimation.gameObject.transform);
            temporaryWeapon.isAirborne = false;
            temporaryWeapon.GetComponent<Collider2D>().enabled = false;
            temporaryWeapon.GetComponent<Rigidbody2D>().simulated = false;
            temporaryWeapon.weaponAnimator.enabled = false;
            temporaryWeapon.enabled = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(detectionPoints[0].position, detectionPointSize);
    }
}