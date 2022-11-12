using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class PauseGUI : MonoBehaviour
{
    public Gamemanager gamemanager;

    [Space] [Space, Tooltip("0=continue; 1=help; 2=options; 3=quit.")]
    public Button[] pauseButtons;

    public bool isGamePaused;

    [Space] public Animator pauseAnimator;

    private void Start()
    {
        gamemanager = GameObject.FindGameObjectWithTag("Gamemanager").GetComponent<Gamemanager>();
    }

    private void Update()
    {
        isGamePaused = fetchPauseMenuState();
    }

    private void LateUpdate()
    {
        PauseMenu(isGamePaused);
    }

    public void ResumeGame()
    {
        gamemanager.isGamePaused = false;
    }

    public void OpenHelpSubMenu()
    {
        pauseAnimator.SetBool("isHelpOpened", true);
    }

    public void CloseHelpSubMenu()
    {
        pauseAnimator.SetBool("isHelpOpened", false);
    }

    public void OpenSettingsSubMenu()
    {
        pauseAnimator.SetBool("isSettingsOpened", true);
    }

    public void CloseSettingsSubMenu()
    {
        pauseAnimator.SetBool("isSettingsOpened", false);
    }

    public void QuitMenu()
    {
    }

    public void PauseMenu(bool _isEnabled)
    {
        pauseAnimator.SetBool("isPaused", _isEnabled);
    }

    private bool fetchPauseMenuState()
    {
        bool _isPaused = gamemanager.isGamePaused;
        return _isPaused;
    }
}