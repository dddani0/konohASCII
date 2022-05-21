using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyStatistics : MonoBehaviour
{
    public float health_maximum;
    [SerializeField] private float health_current;
    
    private void HealthCheck(float _healthpoints)
    {
        if (_healthpoints <= 0)
            Destroy(gameObject);
    }

    private void HealthCheck(float _healthpoints,float _timer)
    {
        if (_healthpoints <= 0 && _timer <= 0)
            Destroy(gameObject);
        else
            _timer -= Time.deltaTime;
    }
    
    private void DisplayHealthBarAtPosition(Vector2 _position, Vector2 _position_offset, GameObject _healthbar, float _heatlth_minimum, float _health_maxium)
    {
        //With shader material, so no UI
        _healthbar.transform.position = (_position + _position_offset);
    }

    public void TakeDamage(float damage)
    {
        health_current -= damage;
    }

    public void TakeDamage(float damage, float multiplier)
    {
        health_current-= (damage * multiplier);
    }
}
