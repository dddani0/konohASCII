using System;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Serialization;

public class SecondaryWeaponContainer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D weaponRigidbody;
    [Space] public WeaponTemplate weapon;
    [Space] public Sprite weaponSprite;
    public SpriteRenderer weaponSpriteRenderer;
    public Animator weaponAnimator;
    public AnimatorOverrideController weaponOverrideController;
    [Space] private float weaponSpeed;
    [Space] public bool isAirborne = true;
    [Space] public int weaponDamage;

    [Space]
    //Determines the direction of weapon movement. 1 = right and -1 = left"
    private int weaponTurnOtherSideValue = 1;

    [Space] public bool canBePickedUp;

    [Tooltip("No assignment required")] private float weaponAngle;
    [Space] public float maximumCastCooldown;
    [SerializeField] private float castCooldown;
    [SerializeField] private BoxCollider2D bodyCollider;

    private void Start()
    {
        Fetch_Rudimentary_Values();
    }

    private void FixedUpdate()
    {
    }

    private void Update()
    {
        ManageWeapon(weaponSpeed);
    }


    private void LateUpdate()
    {
        weaponAnimator.SetBool("is_weapon_active", isAirborne);
        weaponSpriteRenderer.sprite = weaponSprite;
        canBePickedUp = CheckWeaponPickUpStatus();
    }

    private void Fetch_Rudimentary_Values()
    {
        bodyCollider = GetComponent<BoxCollider2D>();
        //bodyCollider.enabled = false;
        castCooldown = maximumCastCooldown;
        weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        weaponAnimator = GetComponentInChildren<Animator>();
        weaponRigidbody = GetComponent<Rigidbody2D>();
    }

    public void AssignNewWeapon(WeaponTemplate weaponTemplate, float _weaponAngle, int _ismovingright)
    {
        weapon = weaponTemplate;
        weaponSpeed = weaponTemplate.weaponSpeed;
        weaponSprite = weaponTemplate.weaponSprite;
        weaponTurnOtherSideValue = _ismovingright;
        weaponDamage = weaponTemplate.damage;
        weaponAngle = _weaponAngle;
        transform.localEulerAngles = new Vector3(0, 0, _weaponAngle);

        switch (weaponTemplate.weaponAnimatorController != null)
        {
            case true:
                weaponAnimator.runtimeAnimatorController = weaponTemplate.weaponAnimatorController;
                break;
            case false:
                print("No attached OverrideController attached to weapon source");
                break;
        }
    }

    private bool CheckWeaponPickUpStatus()
    {
        return !isAirborne;
    }

    private void ManageWeapon(float _speed)
    {
        bool CanMove()
        {
            return isAirborne;
        }

        void ManageWeaponCollider()
        {
            bool CanWeaponBePickedUp()
            {
                return !isAirborne;
            }

            bodyCollider.isTrigger = CanWeaponBePickedUp();
            canBePickedUp = CanWeaponBePickedUp();
        }

        if (!weapon) return;
        ManageWeaponCollider();
        transform.position += CanMove() ? transform.right * (_speed * Time.deltaTime) : transform.right * 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        bool DoesMatchObstacleLayer(int _layer)
        {
            return _layer == 11;
        }

        bool HasHitWall()
        {
            var o = col.gameObject;
            return DoesMatchObstacleLayer(o.layer) || DoesMatchObstacleLayer(o.layer);
        }

        if (HasHitWall())
        {
            isAirborne = false;
        }
    }
}