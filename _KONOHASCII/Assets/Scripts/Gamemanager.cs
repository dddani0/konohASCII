using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public GameObject weapon_container;
    [Space] public UIManager uimnager;
    [Space] public List<GameObject> entityList;

    private void Start()
    {
        FetchRudimentaryValues();
    }

    private void FetchRudimentaryValues()
    {
        entityList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }
}
