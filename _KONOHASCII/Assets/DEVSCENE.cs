using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DEVSCENE : MonoBehaviour
{
    public float difference = 1;
    public GameObject checkpoint;
    public GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        checkpoint.transform.position = new Vector3(target.transform.position.x, checkpoint.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(checkpoint.transform.position,target.transform.position) < difference)
            checkpoint.transform.position = new Vector3(target.transform.position.x, checkpoint.transform.position.y);
    }

    private void FixedUpdate()
    {
        
    }

    private void OnDrawGizmosSelected()
    {

    }
}