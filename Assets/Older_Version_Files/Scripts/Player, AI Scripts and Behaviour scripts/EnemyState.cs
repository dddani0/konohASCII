using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////
///////////////////////////////
//IN PROGRESS! IN PROGRESS!
///////////////////////////////

    [Header("Enemies health, easily accesseable with outer script")]
    public float health;

    public void TakeDamage(float damage)
    {
        switch (gameObject.name[0]) //Fuck you
        {
            case 'J':
               //GetComponent<JiroboBehaviour>().gameManager.GetComponent<Manager>().sound_Effects[23].Play();
                health -= damage;
                break;
            case 'Z':
                //GetComponent<ZakuBehaviour>().gameManager.GetComponent<Manager>().sound_Effects[23].Play();
                health -= damage;
                break;
            case 'Y':
                health -= damage;
                break;
        }

    }
}
