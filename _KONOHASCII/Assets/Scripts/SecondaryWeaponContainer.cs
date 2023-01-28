using System;
using UnityEngine;
using UnityEngine.Serialization;

internal class WeaponEffect
{
    public string effectName { get; set; }
    public bool instantEffect { get; set; }
    public int effectIndex { get; set; }

    string fetchEffectType(string _effectName)
    {
        switch (_effectName)
        {
            case "poison":
                return "poison";
                break;
        }

        return "no effect";
    }

    public WeaponEffect()
    {
    }
}

public class SecondaryWeaponContainer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D weaponRigidbody;
    [Space] public WeaponTemplate weapon;
    [Space] public Sprite weaponSprite;
    public SpriteRenderer weaponSpriteRenderer;
    public Animator weaponAnimator;
    public AnimatorOverrideController weaponOverrideController;
    [Space] private float _weaponSpeed;

    [FormerlySerializedAs("isAirborne")] [Space]
    public bool airborne = true;

    [Space] [Tooltip("Is manipulated with string?")]
    public bool canHome;

    [Space] public int weaponDamage;

    [Tooltip("0 = none, 1 = poison, 2 = bleed, 3 = explosion")]
    public int weaponEffectIndex;

    public float explosionRadius;
    public int explosionDamage;

    [Space]
    //Determines the direction of weapon movement. 1 = right and -1 = left"
    private int _weaponTurnOtherSideValue = 1;

    [Space] public bool canBePickedUp;

    [FormerlySerializedAs("isStuck")] [Tooltip("Stuck when weapon hits enemy.")]
    public bool stuck;

    private bool _canIncrementValue = true;

    [Tooltip("No assignment required")] private float _weaponAngle;
    [Space] public float maximumCastCooldown;
    [SerializeField] private float castCooldown;
    [SerializeField] private BoxCollider2D bodyCollider;

    private void Start()
    {
        Fetch_Rudimentary_Values();
    }

    private void Update()
    {
        ManageWeapon(_weaponSpeed);
    }


    private void LateUpdate()
    {
        weaponAnimator.SetBool("is_weapon_active", airborne);
        weaponSpriteRenderer.sprite = weaponSprite;
        canBePickedUp = CheckWeaponPickUpStatus();
    }

    private void Fetch_Rudimentary_Values()
    {
        bodyCollider = GetComponent<BoxCollider2D>();
        castCooldown = maximumCastCooldown;
        weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        weaponAnimator = GetComponentInChildren<Animator>();
        weaponRigidbody = GetComponent<Rigidbody2D>();
    }

    public void AssignNewWeapon(WeaponTemplate weaponTemplate, float weaponAngle, int isMovingRight)
    {
        bool HasWeaponTemplate()
        {
            return weaponTemplate.weaponAnimatorController;
        }

        bool HasWeaponEffect()
        {
            return weaponTemplate.bleed || weaponTemplate.poison || weaponTemplate.explosion;
        }

        weapon = weaponTemplate;
        _weaponSpeed = weaponTemplate.weaponSpeed;
        _weaponTurnOtherSideValue = isMovingRight;
        weaponDamage = weaponTemplate.damage;
        this._weaponAngle = weaponAngle;
        weaponSprite = weaponTemplate.weaponSprite;
        transform.localEulerAngles = new Vector3(0, 0, weaponAngle);
        // 0 = none, 1 = poison, 2 = bleed, 3 = explosion
        if (HasWeaponEffect())
        {
            if (weaponTemplate.poison) weaponEffectIndex = 1;
            else if (weaponTemplate.bleed) weaponEffectIndex = 2;
            else weaponEffectIndex = 3;
        }
        else
            weaponEffectIndex = 0;

        weaponAnimator.runtimeAnimatorController = null; //reset controller
        switch (HasWeaponTemplate())
        {
            case true:
                weaponOverrideController = weaponTemplate.weaponAnimatorController;
                weaponAnimator.runtimeAnimatorController = weaponOverrideController;
                break;
            case false:
                weaponSprite = weaponTemplate.weaponSprite;
                print("No attached OverrideController attached to weapon source");
                break;
        }
    }

    public void IncreaseWeaponAmmunition(PlayerAction _playerAction)
    {
        //Used when the weapon is picked up
        if (_canIncrementValue)
        {
            _playerAction.secondaryWeaponAmmunition++;
            _canIncrementValue = false;
        }

        Destroy(this);
    }

    public void DismantleWeapon()
    {
        //Upon enemy hit
        stuck = true;
    }

    public void ManipulateWeapon(GameObject target)
    {
        //Only "sasuke" can call this

        bool CanBeManipulated()
        {
            return airborne;
        }

        bool HasTarget()
        {
            return target;
        }

        float HoneAngle()
        {
            bool IsTargetBelow()
            {
                return target.transform.position.y < transform.position.y;
            }

            Vector3 TargetDirection()
            {
                return (target.transform.position - transform.position).normalized;
            }

            Vector3 ThisDirection()
            {
                return IsTargetBelow() ? -transform.up : transform.up;
            }

            return Vector2.Angle(ThisDirection(), TargetDirection());
        }

        if (!HasTarget()) return;
        transform.localEulerAngles = new Vector3(0, 0, HoneAngle());
    }

    private bool CheckWeaponPickUpStatus()
    {
        return !airborne && !stuck;
    }

    private bool HasWeaponAnimation()
    {
        return weaponOverrideController;
    }

    private bool ShouldPlayBaseAnimation()
    {
        return airborne;
    }

    private void ManageWeapon(float speed)
    {
        bool CanMove()
        {
            return airborne;
        }

        void ManageWeaponCollider()
        {
            bool CanWeaponBePickedUp()
            {
                return !airborne && !stuck;
            }

            bodyCollider.enabled = CanWeaponBePickedUp();
            canBePickedUp = CanWeaponBePickedUp();
        }

        if (!weapon) return;
        ManageWeaponCollider();
        transform.position += CanMove() ? transform.right * (speed * Time.deltaTime) : transform.right * 0;
        if (!HasWeaponAnimation()) return;
        weaponAnimator.SetBool("isWeaponAirborne", ShouldPlayBaseAnimation());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        bool DoesMatchObstacleLayer(int layer, int targetLayer)
        {
            return layer == targetLayer;
        }

        bool CanDamage(Component _col)
        {
            return _col.GetComponent<EnemyBehavior>() || _col.GetComponent<PlayerAction>();
        }

        void DealDamageToEntity(Component _col, int damage)
        {
            if (_col.GetComponent<EnemyBehavior>())
                _col.GetComponent<EnemyBehavior>().DealDamage(damage);
            if (_col.GetComponent<PlayerAction>())
                _col.GetComponent<PlayerAction>().TakeInjury(damage);
        }

        bool HasHitWall()
        {
            var o = col.gameObject;
            return DoesMatchObstacleLayer(o.layer, 11) || DoesMatchObstacleLayer(o.layer, 10);
        }

        if (!HasHitWall()) return;
        airborne = false;
        if (weaponEffectIndex != 3) return;
        //make effect
        var hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var VARIABLE in hits)
        {
            if (CanDamage(VARIABLE))
                DealDamageToEntity(VARIABLE, explosionDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}