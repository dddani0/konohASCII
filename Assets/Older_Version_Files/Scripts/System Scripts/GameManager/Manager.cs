using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////
///////////////////////////////
//IN PROGRESS! IN PROGRESS!
///////////////////////////////

    /// <summary>
    /// Optional resolutions:
    /// 1 1920 x 1080
    /// 2 1768 x 992
    /// 3 1600 x 1024
    /// 4 1440 x 900
    /// 5 1366 x 768
    /// 6 1360 x 768
    /// 7 1280 x 768
    /// 8 1176 x 664
    /// 9 1024 x 768
    /// 10 800 x 600
    ///  
    /// Selectable Ninja weapons (S & N)
    /// 0 = Kunai 
    /// 1 = Shuriken
    /// 2 = Fuma Shuriken
    /// 3 = Throwing Star
    /// </summary>

    [Header("Currently loaded scene")]
    Scene current_Loaded_Scene;
    public string current_Loaded_Scene_Name;
    [Header("Instances")]
    [Tooltip("Available players and AI characters are stored here")]public List<GameObject> instances;
    [Header("Ninja Ingame Weapons")]
    [Tooltip("0 = Kunai ; 1 = Shuriken ; 2 = Fuma Shuriken; 3 = Throwing Star ; 4 = Rasengan")]public GameObject[] weapons;
    [Header("Prefabs for accessing & Prefabs")]
    public GameObject[] particle_effects;
    [Header("SoundEffect Storage as Array")]
    public AudioSource[] sound_Effects;
    [Header("Use transition, to change between scenes")]
    public GameObject transition; //maybe change to "Transition manager"
    [Header("Ninja Weapon array(4), use for setting weapons active ; FOR LOADOUTSELECTION")]
    public List<GameObject> ninja_UI_Weapons;
    [Range(0, 3)] public byte selected_Weapon_Number;
    [Header("Difficulty array(4), use for setting difficulty")]
    public List<GameObject> difficulty_UI_Element;
    [Range(0, 2)] public byte selected_Difficulty_Number;
    [Header("Preference Storage, this stores chosen weapon and difficulty")]
    public GameObject preference_Storage_GameObject;
    [Header("InGame Pause GameObject")]
    public GameObject pause_Menu;
    [Space]
    public bool pause;
    [Space]
    public List<Vector2> reserved_Velocity;
    [Header("Jutsu Scroll UI Element")]
    public GameObject jutsu_Scroll_UI;
    [Header("Value Displays for Adventure Summary")] //Maybe outdated
    public Text[] value_Text_Display;
    [Header("Timer attributes (only for boss and training scenes")]
    public Text timer_UI_Display;
    public float timer;
    float start_Timer = 1;
    [Header("UI Clone indicator elements")]
    public GameObject[] clone_Indicator_UI_Elements;
    [Header("Position for the weapons for UzumASCII boss")]
    public Transform[] weaponSpots;
    [Header("Medallion stash")]
    public float medallion_Counter;

    #region Unity Functions
    private void Awake()
    {
        CheckTheCurrentScene();
    }

    private void Start()
    {
        switch (current_Loaded_Scene_Name)
        {
            case "Intro":
                Application.targetFrameRate = 60; //Default settings Need to check, if an fps has been already set!
                QualitySettings.vSyncCount = 0;
                break;
            case "MainMenu":
                GameObject gameobject_Temp = GameObject.FindGameObjectWithTag("PreferenceStorage");
                if (gameobject_Temp == null)
                {
                    Instantiate(preference_Storage_GameObject);
                }
                break;
        }
        if (preference_Storage_GameObject == null)
            preference_Storage_GameObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //Time.timeScale = 1.0f; //Reset if time is issue
        #region Checks if Current scene is loadout select
        //switch (current_Loaded_Scene_Name)
        //{
        //    case "Intro":
        //        Application.targetFrameRate = 30; //Default settings Need to check, if an fps has been already set!
        //        QualitySettings.vSyncCount = 0;
        //        break;
        //    case "UzumASCIIAdventure":
        //        ninja_UI_Weapons.AddRange(GameObject.FindGameObjectsWithTag("NinjaWeapons")); //Collects UI weapon elements
        //        difficulty_UI_Element.AddRange(GameObject.FindGameObjectsWithTag("Difficulty")); //Collects UI Difficulty elements
        //        for (int i = 0; i < ninja_UI_Weapons.Count; i++)
        //        {
        //            ninja_UI_Weapons[i].SetActive(false); //Deactivates every weapon UI
        //        }
        //        ninja_UI_Weapons[selected_Weapon_Number].SetActive(true);
        //        for (int i = 0; i < difficulty_UI_Element.Count; i++)
        //        {
        //            difficulty_UI_Element[i].SetActive(false); //Deactivates every difficulty UI
        //        }
        //        difficulty_UI_Element[selected_Difficulty_Number].SetActive(true);
        //        Instantiate(preference_Storage_GameObject);
        //        break;
        //    case "LoadoutSelectUzumASCIIBoss": //Player vs Sasuke: Loadout select scene
        //        ninja_UI_Weapons.AddRange(GameObject.FindGameObjectsWithTag("NinjaWeapons"));
        //        difficulty_UI_Element.AddRange(GameObject.FindGameObjectsWithTag("Difficulty")); //Why is
        //        Instantiate(preference_Storage_GameObject);
        //        for (int i = 0; i < ninja_UI_Weapons.Count; i++)
        //        {
        //            ninja_UI_Weapons[i].SetActive(false);
        //        }
        //        ninja_UI_Weapons[selected_Weapon_Number].SetActive(true);
        //        break;
        //    case "LoadoutSelectUchihASCIIBoss": //Player vs Naruto: Loadout Select scene
        //        ninja_UI_Weapons.AddRange(GameObject.FindGameObjectsWithTag("NinjaWeapons"));
        //        difficulty_UI_Element.AddRange(GameObject.FindGameObjectsWithTag("Difficulty")); //this necessary?
        //        Instantiate(preference_Storage_GameObject);
        //        for (int i = 0; i < ninja_UI_Weapons.Count; i++)
        //        {
        //            ninja_UI_Weapons[i].SetActive(false);
        //        }
        //        ninja_UI_Weapons[selected_Weapon_Number].SetActive(true);
        //        break;
        //    case "LoadoutSelectUzumASCIITrainingGround":
        //        ninja_UI_Weapons.AddRange(GameObject.FindGameObjectsWithTag("NinjaWeapons"));
        //        difficulty_UI_Element.AddRange(GameObject.FindGameObjectsWithTag("Difficulty"));
        //        Instantiate(preference_Storage_GameObject);
        //        for (int i = 0; i < ninja_UI_Weapons.Count; i++)
        //        {
        //            ninja_UI_Weapons[i].SetActive(false);
        //        }
        //        ninja_UI_Weapons[selected_Weapon_Number].SetActive(true);
        //        break;
        //    case "LoadoutSelectUchihASCIITrainingGround":
        //        ninja_UI_Weapons.AddRange(GameObject.FindGameObjectsWithTag("NinjaWeapons"));
        //        difficulty_UI_Element.AddRange(GameObject.FindGameObjectsWithTag("Difficulty"));
        //        Instantiate(preference_Storage_GameObject);
        //        for (int i = 0; i < ninja_UI_Weapons.Count; i++)
        //        {
        //            ninja_UI_Weapons[i].SetActive(false);
        //        }
        //        ninja_UI_Weapons[selected_Weapon_Number].SetActive(true);
        //        break;
        //    case "UzumASCIIAdventureNormal":
        //        pause_Menu = GameObject.FindGameObjectWithTag("Pause");
        //        preference_Storage_GameObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        break;
        //    case "UzumASCIIAdventureHard":
        //        pause_Menu = GameObject.FindGameObjectWithTag("Pause");
        //        preference_Storage_GameObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        break;
        //    case "UzumASCIIAdventureVeryHard":
        //        pause_Menu = GameObject.FindGameObjectWithTag("Pause");
        //        preference_Storage_GameObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        break;
        //    case "UzumASCIIBoss":
        //        preference_Storage_GameObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        break;
        //    case "UchihASCIIBoss":
        //        preference_Storage_GameObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        break;
        //    case "UzumASCIIAdventureSummary":
        //        preference_Storage_GameObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        DisplayValuesAsText();
        //        break;
        //    case "MedallionCollection":
        //        Instantiate(preference_Storage_GameObject);
        //        preference_Storage_GameObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        DisplayValuesAsText();
        //        break;
        //    case "TrainingModeNaruto":
        //        pause_Menu = GameObject.FindGameObjectWithTag("Pause");
        //        preference_Storage_GameObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        break;
        //    case "TrainingModeSasuke":
        //        pause_Menu = GameObject.FindGameObjectWithTag("Pause");
        //        preference_Storage_GameObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        break;
        //    case "NarutoTutorial":
        //        pause_Menu = GameObject.FindGameObjectWithTag("Pause");
        //        //prefStorage = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        break;
        //    case "SasukeTutorial":
        //        pause_Menu = GameObject.FindGameObjectWithTag("Pause");
        //        //prefStorage = GameObject.FindGameObjectWithTag("PreferenceStorage");
        //        break;
        //}
        #endregion
        //SaveCurrentStateOrLoadCurrentState();
    }

    private void Update()
    {
        #region Reset, Pause and Timer
        //check if it contains
        //if (current_Loaded_Scene_Name == "UzumASCIIAdventureNormal" || current_Loaded_Scene_Name == "UzumASCIIAdventureHard" || current_Loaded_Scene_Name == "UzumASCIIAdventureVeryHard" || current_Loaded_Scene_Name == "UchihASCIIBoss")
        switch (current_Loaded_Scene_Name) //Def. not copy pasted kek
        {
            case "UzumASCIIAdventureNormalDifficulty":
                PauseGame();
                ResetScene(); //Hehehe what the fuck
                break;
            case "UzumASCIIAdventureHardDifficulty":
                PauseGame();
                ResetScene(); //Hehehe what the fuck
                break;
            case "UzumASCIIAdventureVeryHardDifficulty":
                PauseGame();
                ResetScene(); //Hehehe what the fuck
                break;
            case "UchihASCIIBoss":
                Timer(); //Timer 'n' shit
                PauseGame();
                ResetScene(); //Hehehe what the fuck
                break;
            case "UzumASCIIBoss":
                Timer(); //Timer 'n' shit
                PauseGame();
                ResetScene(); //Hehehe what the fuck
                break;
            case "TrainingModeNaruto":
                Timer(); //Timer 'n' shit
                PauseGame();
                ResetScene(); //Hehehe what the fuck
                break;
            case "TrainingModeSasuke":
                Timer(); //Timer 'n' shit
                PauseGame();
                ResetScene(); //Hehehe what the fuck
                break;
            case "NarutoTutorial":
                Timer(); //Timer 'n' shit
                PauseGame();
                ResetScene(); //Hehehe what the fuck
                break;
            case "SasukeTutorial":
                Timer(); //Timer 'n' shit
                PauseGame();
                ResetScene(); //Hehehe what the fuck
                break;
        }

        #endregion
    }

    private void LateUpdate()
    {
        CheckTheCurrentScene(); //Basically, run this function, on every new scene. Whats the solution ? Late update baby <3 Performance ? buh, who gives a shit, nobody's gonna play this game, so who cares ? :)
    }
    #endregion
    #region Timer
    public void Timer()
    {
        timer_UI_Display.text = timer.ToString();
        if (!pause)
        {
            if (start_Timer <= 0)
            {
                timer++;
                start_Timer = 1;
            }
            else
            {
                start_Timer -= Time.deltaTime;
            }
        }
    }
    #endregion
    #region Check Scene
    public void CheckTheCurrentScene()
    {
        current_Loaded_Scene = SceneManager.GetActiveScene();
        current_Loaded_Scene_Name = current_Loaded_Scene.name;
    }
    #endregion
    #region Pause, restart and Die Menu
    public void PauseGame()
    {
        if (!pause && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            //pause_Menu.GetComponent<Animator>().SetFloat("pauseValue", 1);
            //Time.timeScale = 0.1f;
        }
        else if (pause && Input.GetKeyDown(KeyCode.Escape))
        {
            Unpause();
            //Time.timeScale = 1.0f;
            //pause_Menu.GetComponent<Animator>().SetFloat("pauseValue", 0);

        }
    }

    public void Pause()
    {
        pause_Menu.SetActive(true);
        pause = true;
        for (int i = 0; i < instances.Count; i++)
        {
            if (instances[i].GetComponentInChildren<Animator>() != null && instances[i].gameObject.tag != "WPickUp")
                instances[i].GetComponentInChildren<Animator>().speed = 0;
            if (instances[i].GetComponent<Rigidbody2D>() != null)
            {
                reserved_Velocity.Add(instances[i].GetComponent<Rigidbody2D>().velocity);
                instances[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                instances[i].GetComponent<Rigidbody2D>().isKinematic = true;
            }

        }
    }

    public void Unpause()
    {
        pause_Menu.SetActive(false);
        pause = false;
        for (int i = 0; i < instances.Count; i++)
        {
            if (instances[i].GetComponentInChildren<Animator>() != null && instances[i].gameObject.tag != "WPickUp")
                instances[i].GetComponentInChildren<Animator>().speed = 1;
            if (instances[i].GetComponent<Rigidbody2D>() != null)
            {
                instances[i].GetComponent<Rigidbody2D>().isKinematic = false;
                instances[i].GetComponent<Rigidbody2D>().velocity = reserved_Velocity[0];
                reserved_Velocity.RemoveAt(0);
            }

        }
    }
    public void ResetScene()
    {
        if (Input.GetKeyDown(KeyCode.R) && current_Loaded_Scene_Name != "NarutoTutorial" && current_Loaded_Scene_Name != "SasukeTutorial")
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }
    }
    #endregion
    #region Display values after Summary
    public void DisplayValuesAsText()
    {
        switch (current_Loaded_Scene_Name == "MedallionCollection")
        {
            case true:
                if (current_Loaded_Scene_Name == "MedallionCollection" && PlayerPrefs.HasKey("Player Medallion"))
                {
                    value_Text_Display[0].text = PlayerPrefs.GetFloat("Player Medallion", medallion_Counter).ToString();
                }
                else if (current_Loaded_Scene_Name == "MedallionCollection" && !PlayerPrefs.HasKey("Player Medallion"))
                {
                    value_Text_Display[0].text = "0".ToString();
                }
                break;

            case false:
                value_Text_Display[1].text = preference_Storage_GameObject.GetComponent<PreferenceStorage>().score.ToString();
                value_Text_Display[0].text = preference_Storage_GameObject.GetComponent<PreferenceStorage>().highscore.ToString(); //PlayerPrefs.GetFloat("Player Highscore", prefStorage.GetComponent<PreferenceStorage>().highscore).ToString();
                value_Text_Display[2].text = preference_Storage_GameObject.GetComponent<PreferenceStorage>().medallion.ToString();
                value_Text_Display[3].text = preference_Storage_GameObject.GetComponent<PreferenceStorage>().chakraConsumed.ToString();
                for (int i = 5; i < value_Text_Display.Length; i++) //uhum, looks janky as fuck
                {
                    value_Text_Display[i].text = preference_Storage_GameObject.GetComponent<PreferenceStorage>().timer.ToString();
                }
                switch (preference_Storage_GameObject.GetComponent<PreferenceStorage>().difficulty)
                {
                    case 0:
                        value_Text_Display[4].text = "Normal".ToString();
                        break;
                    case 1:
                        value_Text_Display[4].text = "Hard".ToString();
                        break;
                    case 2:
                        value_Text_Display[4].text = "Very Hard".ToString();
                        break;
                }
                break;
        }
    }
    #endregion
    #region Save Variables AKA Save System
    /* Save system
     * 
     * The game saves on every Summary scene
     * 
     * What does it save?
     * 
     * It saves the amount of medallions
     * It saves the highscore amount
     * 
     * thats it
     */ 
    //public void SaveCurrentStateOrLoadCurrentState()
    //{ //wtf looks scary
    //    if (current_Loaded_Scene_Name == "UzumASCIIAdventureSummary" && preference_Storage_GameObject.GetComponent<PreferenceStorage>().bossValues == 0 && PlayerPrefs.HasKey("Player Highscore")) //Get Save Data if Save Data exists
    //    {
    //        Debug.Log("Save Data exists, and I'm getting the data");
    //        //PlayerPrefs.GetFloat("Player Highscore", prefStorage.GetComponent<PreferenceStorage>().highscore);
    //        //PlayerPrefs.GetFloat("Player Medallion", prefStorage.GetComponent<PreferenceStorage>().medallion);
    //        value_Text_Display[0].text = PlayerPrefs.GetFloat("Player Highscore", preference_Storage_GameObject.GetComponent<PreferenceStorage>().highscore).ToString();
    //        value_Text_Display[2].text = preference_Storage_GameObject.GetComponent<PreferenceStorage>().medallion.ToString();
    //        medallion_Counter += preference_Storage_GameObject.GetComponent<PreferenceStorage>().medallion;
    //        PlayerPrefs.SetFloat("Player Medallion", medallion_Counter);
    //        preference_Storage_GameObject.GetComponent<PreferenceStorage>().medallion = 0;
    //        PlayerPrefs.Save();
    //    }
    //    else if (current_Loaded_Scene_Name == "UzumASCIIAdventureSummary" && preference_Storage_GameObject.GetComponent<PreferenceStorage>().bossValues == 0 && !PlayerPrefs.HasKey("Player Highscore")) //Not? okay, save the current
    //    {
    //        Debug.Log("No Save Data, Creating one");
    //        PlayerPrefs.SetFloat("Player Highscore", preference_Storage_GameObject.GetComponent<PreferenceStorage>().highscore);
    //        PlayerPrefs.SetFloat("Player Medallion", medallion_Counter);
    //        medallion_Counter += preference_Storage_GameObject.GetComponent<PreferenceStorage>().medallion;
    //        preference_Storage_GameObject.GetComponent<PreferenceStorage>().medallion = 0;
    //        PlayerPrefs.Save();
    //    }
    //    else if (current_Loaded_Scene_Name == "MainMenu" && PlayerPrefs.HasKey("Highscore")) //Menu and check if saveData Exists
    //    {
    //        Debug.Log("Menu and Savedata exist");
    //        PlayerPrefs.GetFloat("Player Highscore", preference_Storage_GameObject.GetComponent<PreferenceStorage>().highscore);
    //        PlayerPrefs.GetFloat("Player Medallion", medallion_Counter);
    //    }
    //}

    public void DeleteCurrentSave()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion
}

/* Sort this out thanks future dani
 *         switch (currentSceneName == "MedallionCollection")
        {
            case true:
                if (currentSceneName == "MedallionCollection" && PlayerPrefs.HasKey("Player Medallion"))
                {
                    Debug.Log("okééééé");
                    vTextDisp[0].text = PlayerPrefs.GetFloat("Player Medallion", prefStorage.GetComponent<PreferenceStorage>().currentMedalion).ToString();
                }
                else if (currentSceneName == "MedallionCollection" && !PlayerPrefs.HasKey("Player Medallion"))
                {
                    vTextDisp[0].text = "0".ToString();
                }
                break;
            case false:
                vTextDisp[1].text = prefStorage.GetComponent<PreferenceStorage>().score.ToString();
                vTextDisp[0].text = prefStorage.GetComponent<PreferenceStorage>().highscore.ToString();//PlayerPrefs.GetFloat("Player Highscore", prefStorage.GetComponent<PreferenceStorage>().highscore).ToString();
                vTextDisp[2].text = PlayerPrefs.GetFloat("Player Medallion", prefStorage.GetComponent<PreferenceStorage>().medallion).ToString();
                vTextDisp[3].text = prefStorage.GetComponent<PreferenceStorage>().chakraConsumed.ToString();
                switch (prefStorage.GetComponent<PreferenceStorage>().difficulty)
                {
                    case 0:
                        vTextDisp[4].text = "Normal".ToString();
                        break;
                    case 1:
                        vTextDisp[4].text = "Hard".ToString();
                        break;
                    case 2:
                        vTextDisp[4].text = "Very Hard".ToString();
                        break;
                }
                break;
        }

        if (currentSceneName == "UzumASCIIAdventureSummary" && prefStorage.GetComponent<PreferenceStorage>().bossValues == 0 && PlayerPrefs.HasKey("Player Highscore")) //Get Save Data if Save Data exists
        {
            Debug.Log("Save Data exists, Getting the data");
            vTextDisp[0].text = PlayerPrefs.GetFloat("Player Highscore", prefStorage.GetComponent<PreferenceStorage>().highscore).ToString(); //Highscore display
            vTextDisp[2].text = prefStorage.GetComponent<PreferenceStorage>().medallion.ToString();                                           //Display This run's medallion
            currentMedallion += prefStorage.GetComponent<PreferenceStorage>().medallion;                                                      //Add current medallion to the stash (I KNOW BAD VAR NAME)
            prefStorage.GetComponent<PreferenceStorage>().medallion = 0;                                                                      //Reset value
            PlayerPrefs.SetFloat("Player Medallion", currentMedallion);
            PlayerPrefs.Save();
        }
        else if (currentSceneName == "UzumASCIIAdventureSummary" && prefStorage.GetComponent<PreferenceStorage>().bossValues == 0 && !PlayerPrefs.HasKey("Player Highscore")) //Not? okay, save the current
        {
            Debug.Log("No Save Data, Creating one");
            PlayerPrefs.SetFloat("Player Highscore", prefStorage.GetComponent<PreferenceStorage>().highscore); //Create highscore save
            currentMedallion += prefStorage.GetComponent<PreferenceStorage>().medallion;
            PlayerPrefs.SetFloat("Player Medallion", currentMedallion);
            PlayerPrefs.Save();
            prefStorage.GetComponent<PreferenceStorage>().medallion = 0;
        }
        else if (currentSceneName == "MainMenu" && PlayerPrefs.HasKey("Highscore")) //Menu and check if saveData Exists
        {
            Debug.Log("Menu and Savedata exist");
            PlayerPrefs.GetFloat("Player Highscore", prefStorage.GetComponent<PreferenceStorage>().highscore);
            PlayerPrefs.GetFloat("Player Medallion", prefStorage.GetComponent<PreferenceStorage>().currentMedalion);
        }
 */
