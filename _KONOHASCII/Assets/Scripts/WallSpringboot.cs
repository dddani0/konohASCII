using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpringboot : MonoBehaviour
{
    //The script manages a brake system, which won't let the player walk off the wall.
    //There has to be a delay, so they won't be able to get stuck on the brakes
    //The brake is essentially just a collider which activates once on the wall.
    //This kind of mechanic won't work with a thin layered wall.
    
    public BoxCollider2D[] wallBrake;

    public void EnableBrakes()
    {
        StartCoroutine(switchBrakeState(true,0.3f));
    }

    public void DisableBrakes()
    {
        StartCoroutine(switchBrakeState(false,0f));
    }

    IEnumerator switchBrakeState(bool _state, float _delayDelta)
    {
        yield return new WaitForSeconds(_delayDelta);
        for (int i = 0; i < wallBrake.Length; i++)
        {
            wallBrake[i].enabled = _state;
        }
    }
}