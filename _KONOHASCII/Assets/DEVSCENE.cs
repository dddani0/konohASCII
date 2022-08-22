using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DEVSCENE : MonoBehaviour
{
    public Transform raycastpos;
    [Space] public float raycastLenght;
    public bool isGound;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        RaycastHit2D ledgeRay = Physics2D.Linecast(raycastpos.transform.position,
            new Vector2(raycastpos.transform.position.x, raycastpos.transform.position.y - raycastLenght));
        switch (ledgeRay.collider != null)
        {
            case true:
                isGound = true;
                break;
            case false:
                isGound = false;
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(raycastpos.transform.position,
            new Vector2(raycastpos.transform.position.x, raycastpos.transform.position.y - raycastLenght));
    }
}