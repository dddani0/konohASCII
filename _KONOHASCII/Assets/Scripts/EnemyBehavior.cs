using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    //Basic enemy Behavior
    //Can be stationary, or non-stationary.
    //Read more about the behavior, here:
    //https://github.com/marloss/konohASCII/issues/4

    [Header("Resources")] public EnemyTemplate enemyTemplate;
    [Space] public EnemyAnimation enemyAnimation;
    [Space] public Gamemanager gamemanager;
    [Space] public WeaponTemplate secondaryWeapon;
    [Space] public string enemyName;
    [Space] public int maximumHealth;
    [SerializeField] private int health;
    [Space] [SerializeField] private bool isEnemyDetected;
    public bool isStationary;
    public int enemyLevel;
    public float movementSpeed;
    [Space] public bool isObjectInMotion;
    [Space] public Rigidbody2D enemyRigidbody2D;

    [Header("Enemy Behavior type variables")]
    private RaycastHit2D hitcol;

    [Tooltip("Interval for checking for enemy.")] [Space]
    public float maximumTimeBtwEnemyChecks;

    [SerializeField] private float timeBtwEnemyChecks;

    [Tooltip("Check circle is casted in these points.")]
    public Transform[] detectionPoints;

    [Tooltip("Damage and weapon casts are initiated at attack points.")]
    public Transform[] attackPoints;

    [Tooltip("Crosshair represents where the ninja weapon can be thrown.")] [Space]
    public GameObject targetCrosshairGameObject;

    [Tooltip("Maximum height, you can aim with the crosshair.")]
    public int crosshairMaximumHeight;

    public Vector3 crosshairOffset;

    [Tooltip("Detection state while the target stays inside the radius.")] [Space]
    public float targetMaximumDistance;

    [SerializeField] private float detectionPointSize;

    [Tooltip("All detected targets are assigned to the list, before sorting out the main target.")] [Space]
    public List<GameObject> targetList;

    [Tooltip("Enemy will solely focus on the main target. Harder bosses automatically switch out")]
    public GameObject mainTarget;

    [Space] public bool isFacingRight;

    [Tooltip("Drone marks the destination for the enemy")] [Space(10f)]
    public GameObject drone;

    public float droneHeightCastMagnitude;
    public Transform[] droneRaycastPosition;

    [Tooltip("Full patrol magnitude is x * 2. The course, which enemy completes.")] [Space]
    public int patrolMagnitude;

    [Tooltip("The maximum distance, when the patrol turns around.")]
    public float PatrolHaltDistance;

    public Vector3 patrolStartposition;
    [SerializeField] private bool hasEnemyNotFinishedPatrol;
    [Space] public float maximumWaitingTime;
    [SerializeField] private float waitingTime;
    private bool hasOneIterationStarted;

    private int currentHighestDamage; //Highest damage done to this behavior
    private GameObject highestDamageParticipiant; //The object, which caused the highest damage

    void Start()
    {
        FetchRudimentaryValues();
        FetchDataFromTemplate(enemyTemplate);
    }

    void Update()
    {
        SetAnimationProperties();
        isFacingRight = CheckObjectOrientation();
        switch (isStationary)
        {
            case true:
                StationaryEnemy();
                break;
            case false:
                PatrollingEnemy();
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

    private void PatrollingEnemy()
    {
        //See the list of non-stationary basic enemies at the issue
        // https://github.com/marloss/konohASCII/issues/4
        switch (isEnemyDetected)
        {
            case true:
                if (!mainTarget)
                    mainTarget = DetermineMainTarget();
                targetCrosshairGameObject.transform.position = DetermineTargetCrosshair();
                drone.transform.position = DetermineDronePosition();
                CastWeapon();
                break;
            case false:
                Patrol();
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

    private void Patrol()
    {
        Vector2 destination;
        if (waitingTime <= 0)
        {
            isObjectInMotion = true;
            
            if (hasOneIterationStarted)
            {
                enemyAnimation.transform.localScale = isFacingRight ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
                isFacingRight = CheckObjectOrientation();
                hasOneIterationStarted = false;
            }

            //This is also inverted, reason: line 231
            destination = isFacingRight
                ? new Vector2(patrolStartposition.x + patrolMagnitude, patrolStartposition.y)
                : new Vector2(patrolStartposition.x - patrolMagnitude, transform.position.y);
            hasEnemyNotFinishedPatrol = CheckPatrolDestinationCompletion(destination, PatrolHaltDistance);
            switch (hasEnemyNotFinishedPatrol)
            {
                case true:
                    transform.position =
                        Vector2.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
                    break;
                case false:
                    //This is inverted, because the enemy pauses the way it moved before
                    waitingTime = maximumWaitingTime;
                    hasOneIterationStarted = true;
                    isObjectInMotion = false;
                    break;
            }
        }
        else
            waitingTime -= Time.deltaTime;
        
    }

    private void SetAnimationProperties()
    {
        enemyAnimation.SetAnimationState("isInMotion",isObjectInMotion,enemyAnimation.defaultAnimator);
    }

    private bool CheckLedgeRayCast()
    {
        Transform _raycastPosition = isFacingRight
            ? droneRaycastPosition[0]
            : droneRaycastPosition[1];

        RaycastHit2D _ledgeRay = Physics2D.Linecast(_raycastPosition.position,
            new Vector2(_raycastPosition.position.x, _raycastPosition.transform.position.y - droneHeightCastMagnitude));
        bool _isOnTheLedge = _ledgeRay.collider != null;
        return _isOnTheLedge;
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

    private Vector3 DetermineDronePosition()
    {
        bool _isDetected = CheckDetection();
        Vector3 _dronePosition = _isDetected
            ? new Vector3(mainTarget.transform.position.x, droneHeightCastMagnitude)
            : new Vector3(0, 0);
        return _dronePosition;
    }

    private bool CheckDetection()
    {
        bool isMainTargetDetected = mainTarget;
        return isMainTargetDetected;
    }

    private bool CheckPatrolDestinationCompletion(Vector2 _destination, float _maximumDistanceBetweenDestination)
    {
        bool _hasPatrolNotCompletedRoute =
            Vector2.Distance(_destination, transform.position) > _maximumDistanceBetweenDestination &&
            CheckLedgeRayCast();
        return _hasPatrolNotCompletedRoute;
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
        movementSpeed = _enemyTemplate.movementSpeed;
        enemyAnimation.animatorController = _enemyTemplate.overrideController;
    }

    private void FetchRudimentaryValues()
    {
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        gamemanager = GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>();
        timeBtwEnemyChecks = maximumTimeBtwEnemyChecks;
        drone = GameObject.FindGameObjectWithTag("Drone");
        patrolStartposition = transform.position;
        enemyAnimation.defaultAnimator.runtimeAnimatorController = enemyAnimation.animatorController;
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
        Gizmos.DrawLine(droneRaycastPosition[0].transform.position,
            new Vector3(droneRaycastPosition[0].position.x,
                droneRaycastPosition[1].position.y - droneHeightCastMagnitude));
        Gizmos.DrawLine(droneRaycastPosition[1].transform.position,
            new Vector3(droneRaycastPosition[1].position.x,
                droneRaycastPosition[1].position.y - droneHeightCastMagnitude));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(patrolStartposition.x - patrolMagnitude, transform.position.y),
            new Vector2(patrolStartposition.x + patrolMagnitude, transform.position.y));
    }
}