﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneModeShaderManager : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////
    public void DisableObject()
    {
        GetComponent<Animator>().SetTrigger("BTDef");
    }
    public void ResetObject()
    {
        GetComponent<Animator>().ResetTrigger("BTDef");
        GetComponent<Animator>().ResetTrigger("ena");
    }
}
