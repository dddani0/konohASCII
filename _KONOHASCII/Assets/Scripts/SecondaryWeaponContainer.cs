using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SecondaryWeaponContainer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D weaponRigidbody;
    [Space] public WeaponTemplate weaponTemplateTemplate;
    [Space] public Sprite weaponSprite;
    public SpriteRenderer weaponSpriteRenderer;
    public Animator weaponAnimator;
    public AnimatorOverrideController weaponOverrideController;
    [Space] private float weaponAirborneSpeed;
    [Space] public bool isAirborne = true;

    [Space] [Tooltip("Determines the direction of weapon movement. 1 = right and -1 = left")]
    private int weaponMovementDirection = 1;

    private void Start()
    {
        Fetch_Rudimentary_Values();
    }

    private void FixedUpdate()
    {
        if (weaponTemplateTemplate != null)
            if (weaponTemplateTemplate.isRange)
                Throwable_Weapon(weaponTemplateTemplate, weaponAirborneSpeed);
    }

    private void Update()
    {
        if (!weaponTemplateTemplate) //Is weaponSource asigned?
            switch (weaponTemplateTemplate.isRange)
            {
                case true: /*Throwable_Weapon();*/ break;
                case false: /* Melee_Weapon();*/ break;
            }
    }


    private void LateUpdate()
    {
        weaponAnimator.SetBool("is_weapon_active",isAirborne);
        weaponSpriteRenderer.sprite = weaponSprite;
    }

    private void Fetch_Rudimentary_Values()
    {
        weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        weaponAnimator = GetComponentInChildren<Animator>();
        weaponRigidbody = GetComponent<Rigidbody2D>();
    }

    public void AssignNewWeapon(WeaponTemplate weaponTemplate, int _ismovingright)
    {
        weaponTemplateTemplate = weaponTemplate;
        weaponAirborneSpeed = weaponTemplate.weaponSpeed;
        weaponSprite = weaponTemplate.weaponSprite;
        weaponMovementDirection = _ismovingright;
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
                weaponRigidbody.velocity = new Vector2(Time.fixedDeltaTime * _speed * weaponMovementDirection,
                    weaponRigidbody.velocity.y);

                break;
            case false:
                weaponRigidbody.velocity = new Vector2(weaponMovementDirection * 0, weaponRigidbody.velocity.y);

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