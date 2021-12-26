using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreferenceStorage : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////
///////////////////////////////
//IN PROGRESS! IN PROGRESS!
///////////////////////////////

    [Header("Currently loaded Scene")]
    Scene curScene;
    public string sceneName;
    [Header("Gamemanager Access")]
    public GameObject gameManager;
    [Header("Preference options")]
    [Range(0, 3)] public int weapon;
    [Range(0, 2)] public int difficulty;
    [Range(0, 2)] public int bossValues;
    [Header("Achievement attributes")]
    public float score;
    public float highscore;
    [Space]
    public float chakraConsumed;
    public float weaponsThrown;
    public float medallion;
    public float currentMedalion;
    [Space]
    public float timer;
    [Header("fe")]
    public float resolution_Width;
    public float resolution_Height;
    [Space]
    public bool full_Screen;
    [Space]
    public bool show_FPS;
    [Space]
    public float target_fps;

    #region Unity Functions
    private void Start()
    {
        GetCurrentScene();
        CheckSceneForBossValues();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        resolution_Height = Screen.currentResolution.height;
        resolution_Width = Screen.currentResolution.width;
    }
    private void Update()
    {
        GetCurrentScene();
        DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate()
    {
        GetCurrentScene();
        SetHighScore();
    }
    #endregion
    #region Set Highscore
    public void SetHighScore() //ffs use in late update
    {
        if (score > highscore)
        {
            highscore = score;
        }
    }
    #endregion
    #region Get Current Scene
    public void GetCurrentScene()
    {
        curScene = SceneManager.GetActiveScene();
        sceneName = curScene.name;
    }
    #endregion
    #region Get Scene Value For Summary
    public void CheckSceneForBossValues()
    {
        if (sceneName == "LoadoutSelectUzumASCIIBoss") //2 is for Naruto Vs Neji
        {
            bossValues = 2;
        }
        else if (sceneName == "LoadoutSelectUchihASCIIBoss") //1 is for Sasuke Vs Naruto
        {
            bossValues = 1;
        }
        else if (sceneName == "UzumASCIIAdventureNormal" || sceneName == "UzumASCIIAdventureHard" || sceneName == "UzumASCIIAdventureVeryHard")
        {
            bossValues = 0;
        }
    }
    #endregion
}