using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy/new Enemy Template", order = 1)]
public class EnemyTemplate : ScriptableObject
{
    public string enemyName;
    public int maximumHealth;
    public bool isStationary;

    [Space] [Tooltip("Only throwable weapons!")]
    public WeaponTemplate weapon;

    [Tooltip("Level 1 = Primitive AI | level 2 = advanced | level 3 = true shinobi")] [Range(1, 3)]
    public int enemyComplexityLevel;
}