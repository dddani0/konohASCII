using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Gamemanager : MonoBehaviour
{
    [HideInInspector] public float timer;
    public PlayerAction playerEntity;
    public GameObject weaponEntity;

    [FormerlySerializedAs("uimnager")] [Space] [Tooltip("NEVER ASSIGN PREFAB FROM INSPECTOR!")]
    public UIManager uiManager;

    [Tooltip("NEVER ASSIGN PREFAB FROM INSPECTOR!")]
    public PauseGUI pauseManager;

    [Space] public bool isGamePaused;

    private void Start()
    {
        FetchRudimentaryValues();
        playerEntity.FetchRudimentaryValues();
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void LateUpdate()
    {
    }

    private void FetchRudimentaryValues()
    {
        playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAction>();
        pauseManager = GameObject.FindGameObjectWithTag("PauseUI").GetComponent<PauseGUI>();
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
    }
}