using UnityEngine;
using UnityEngine.Serialization;

public class WeaponContainer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D weaponRigidbody;
    [Space]
    public WeaponSource weaponSourceSource;
    [Space] 
    public Sprite weaponSprite;
    public SpriteRenderer weaponSpriteRenderer;
    public Animator weaponAnimator;
    public AnimatorOverrideController weaponOverrideController;
    [Space] 
    private float weaponAirborneSpeed;
    [Space]
    [Tooltip("Determines the direction of weapon movement. 1 = right and -1 = left")]
    private int weaponMovementDirection = 1;

    private void Start()
    {
        Fetch_Rudimentary_Values();
    }

    private void FixedUpdate()
    {
        if (weaponSourceSource != null)
            if (weaponSourceSource.isRange)
                Throwable_Weapon(weaponSourceSource,weaponAirborneSpeed);
    }
    
    private void Update()
    {
        if (!weaponSourceSource) //Is weaponSource asigned?
            switch (weaponSourceSource.isRange)
            {case true: /*Throwable_Weapon();*/ break; case false: /* Melee_Weapon();*/ break;}
    }

    
    
    private void LateUpdate()
    {
        weaponSpriteRenderer.sprite = weaponSprite;
    }

    private void Fetch_Rudimentary_Values()
    {
        weaponSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        weaponAnimator = GetComponentInChildren<Animator>();
        weaponRigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void AssignNewWeapon(WeaponSource weaponSource, int _ismovingright)
    {
        weaponSourceSource = weaponSource;
        weaponAirborneSpeed = weaponSource.weaponSpeed;
        weaponSprite = weaponSource.weaponSprite;
        weaponMovementDirection = _ismovingright;
    }
    
    private void Throwable_Weapon(WeaponSource weaponSource, float _speed)
    {
        weaponRigidbody.velocity = new Vector2(Time.fixedDeltaTime * _speed * weaponMovementDirection, weaponRigidbody.velocity.y);
    }

    private void Melee_Weapon()
    {
        
    }
}