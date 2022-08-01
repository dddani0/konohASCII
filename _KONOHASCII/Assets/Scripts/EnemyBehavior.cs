using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public EnemyTemplate enemyTemplate;
    public string enemyName;
    [Space]
    public int maximumHealth;
    [SerializeField] private int health;
    [Space] 
    [SerializeField] private bool isDetected;
    public bool isStationary;
    public int enemyLevel;
    // Start is called before the first frame update
    void Start()
    {
        FetchDataFromTemplate(enemyTemplate);
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
