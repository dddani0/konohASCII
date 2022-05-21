using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Weapon", order = 1)]
public class WeaponSource : ScriptableObject
{
    /// <summary>
    /// Manual creation for new weapons.
    /// </summary>
    
    [Tooltip("Weapon name starts with capital letter, like 'Kunai'.")]
    public string weaponName;
    public Sprite weaponSprite;
    [Tooltip("Animation override controller for diverse animations")]
    public AnimatorOverrideController weaponAnimatorController;
    [Space] 
    public int damage;
    [Space] 
    public bool isRange;
    public float weaponSpeed;
    [Space] 
    public bool isWeaponFlying;
}