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
    [Space] private float weaponAirborneSpeed;
    [Space] public bool isAirborne = true;
    [Space] public int weaponDamage;

    [Space]
    //Determines the direction of weapon movement. 1 = right and -1 = left"
    private int weaponTurnOtherSideValue = 1;

    [Tooltip("No assignment required")] public float weaponAngle;

    private void Start()
    {
        Fetch_Rudimentary_Values();
    }

    private void FixedUpdate()
    {
    }

    private void Update()
    {
        if (weapon)
        {
            Throwable_Weapon(weapon, weaponAirborneSpeed);
        }
    }


    private void LateUpdate()
    {
        weaponAnimator.SetBool("is_weapon_active", isAirborne);
        weaponSpriteRenderer.sprite = weaponSprite;
    }

    private void Fetch_Rudimentary_Values()
    {
        weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        weaponAnimator = GetComponentInChildren<Animator>();
        weaponRigidbody = GetComponent<Rigidbody2D>();
    }

    public void AssignNewWeapon(WeaponTemplate weaponTemplate, float _weaponAngle, int _ismovingright)
    {
        weapon = weaponTemplate;
        weaponAirborneSpeed = weaponTemplate.weaponSpeed;
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

    private void Throwable_Weapon(WeaponTemplate weaponTemplate, float _speed)
    {
        switch (isAirborne)
        {
            case true:
                transform.position += transform.right * _speed *Time.deltaTime;
                break;
            case false:
                weaponRigidbody.velocity = new Vector2(weaponTurnOtherSideValue * 0, weaponRigidbody.velocity.y);

                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            isAirborne = false;
        }
    }
}