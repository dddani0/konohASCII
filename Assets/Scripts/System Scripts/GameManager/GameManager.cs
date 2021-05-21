using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////
///////////////////////////////
IN PROGRESS! IN PROGRESS!
///////////////////////////////

    //Get current scene, current scene name
    Scene currentScene;
    string sceneName;

    [Header("Game manager")]
    public GameObject gameManager;
    [Header("Preference Selection GameObject Access")]
    public GameObject preference_Selection;
    [Header("Canvas Manager")]
    public GameObject canvas_Manager;

    //Load: Loading into another scene
    //Enter: Enter a menu
    //Exit: Exit from a scene

    public void Load_Scene_Adventure_Mode() 
    {
        SceneManager.LoadScene(2);
    }
    public void Exit_Adventure_Mode()
    {

        SceneManager.LoadScene(1);
    }
    public void Continue_Adventure_Mode()
    {
        gameManager.GetComponent<Manager>().Unpause();
    }
    public void Load_Challenge_Mode_Menu_Selection()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().main_Menu_Panel.SetActive(false);
        canvas_Manager.GetComponent<Canvas_Manager>().challenge_Mode_Menu.SetActive(true);
        for (int i = 0; i < canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts.Length; i++)
        {
            canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts[i].SetActive(false);
        }
    }
    public void Exit_Challenge_Mode_Menu_Selection()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().main_Menu_Panel.SetActive(true);
        canvas_Manager.GetComponent<Canvas_Manager>().challenge_Mode_Menu.SetActive(false);
        for (int i = 0; i < canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts.Length; i++)
        {
            canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts[i].SetActive(true);
        }
    }
    public void Load_Training_Mode_Menu_Selection()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().main_Menu_Panel.SetActive(false);
        canvas_Manager.GetComponent<Canvas_Manager>().training_Mode_Menu.SetActive(true);
        for (int i = 0; i < canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts.Length; i++)
        {
            canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts[i].SetActive(false);
        }
    }
    public void Exit_Training_Mode_Menu_Selection()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().main_Menu_Panel.SetActive(true);
        canvas_Manager.GetComponent<Canvas_Manager>().training_Mode_Menu.SetActive(false);
        for (int i = 0; i < canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts.Length; i++)
        {
            canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts[i].SetActive(true);
        }
    }
    public void Load_Information_Menu()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().information_Menu.SetActive(true);
        canvas_Manager.GetComponent<Canvas_Manager>().main_Menu_Panel.SetActive(false);
        for (int i = 0; i < canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts.Length; i++)
        {
            canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts[i].SetActive(false);
        }
    }
    public void Exit_Information_Menu()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().information_Menu.SetActive(false);
        canvas_Manager.GetComponent<Canvas_Manager>().main_Menu_Panel.SetActive(true);
        for (int i = 0; i < canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts.Length; i++)
        {
            canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts[i].SetActive(true);
        }
    }
    public void Enter_Settings_Menu()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().Show_Current_Settings_Configuration();
        canvas_Manager.GetComponent<Canvas_Manager>().settings_Menu.SetActive(true);
        canvas_Manager.GetComponent<Canvas_Manager>().main_Menu_Panel.SetActive(false);

        for (int i = 0; i < canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts.Length; i++)
        {
            canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts[i].SetActive(false);
        }
    }
    public void Exit_Settings_Menu()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().settings_Menu.SetActive(false);
        canvas_Manager.GetComponent<Canvas_Manager>().main_Menu_Panel.SetActive(true);
        for (int i = 0; i < canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts.Length; i++)
        {
            canvas_Manager.GetComponent<Canvas_Manager>().sub_Texts[i].SetActive(true);
        }
    }
    public void Enter_Collectible_Menu()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().settings_Menu.SetActive(false);
        canvas_Manager.GetComponent<Canvas_Manager>().collectible_Menu.SetActive(true);
    }
    public void Exit_Collectible_Menu()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().settings_Menu.SetActive(true);
        canvas_Manager.GetComponent<Canvas_Manager>().collectible_Menu.SetActive(false);
    }
    public void Enter_Save_Management_Menu()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().settings_Menu.SetActive(false);
        canvas_Manager.GetComponent<Canvas_Manager>().save_Management_Menu.SetActive(true);
    }
    public void Exit_Save_Management_Menu()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().settings_Menu.SetActive(true);
        canvas_Manager.GetComponent<Canvas_Manager>().save_Management_Menu.SetActive(false);
    }
    public void Exit_Game()
    {
        Application.Quit();
    }
    public void Change_Weapon_Preference()
    {
        for (int i = 0; i < canvas_Manager.GetComponent<Canvas_Manager>().weapon_Select_UI_Element.Length; i++)
        {
            if (canvas_Manager.GetComponent<Canvas_Manager>().weapon_Select_UI_Element[i].GetComponent<Button_Select>().isSelected)
            {
                canvas_Manager.GetComponent<Canvas_Manager>().weapon_Select_UI_Element[i].GetComponent<Button_Select>().isSelected = false;
                break;
            } 
        }
        this.gameObject.GetComponent<Button_Select>().isSelected = true;
        for (int n = 0; n < canvas_Manager.GetComponent<Canvas_Manager>().weapon_Select_UI_Element.Length; n++)
        {
            if (canvas_Manager.GetComponent<Canvas_Manager>().weapon_Select_UI_Element[n].name == this.gameObject.name)
            {
                gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().weapon = n;
                canvas_Manager.GetComponent<Canvas_Manager>().Set_Preference_Index_New_Position(n, 0);;
                break;
            }
        }
    }
    public void Change_Difficulty_Preference()
    {
        for (int i = 0; i < canvas_Manager.GetComponent<Canvas_Manager>().weapon_Select_UI_Element.Length; i++)
        {
            if (canvas_Manager.GetComponent<Canvas_Manager>().difficulty_Select_UI_Element[i].GetComponent<Button_Select>().isSelected)
            {
                canvas_Manager.GetComponent<Canvas_Manager>().difficulty_Select_UI_Element[i].GetComponent<Button_Select>().isSelected = false;
                break;
            }
        }
        this.gameObject.GetComponent<Button_Select>().isSelected = true;
        for (int n = 0; n < canvas_Manager.GetComponent<Canvas_Manager>().difficulty_Select_UI_Element.Length; n++)
        {
            if (canvas_Manager.GetComponent<Canvas_Manager>().difficulty_Select_UI_Element[n].name == this.gameObject.name)
            {
                gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().difficulty = n;
                canvas_Manager.GetComponent<Canvas_Manager>().Set_Preference_Index_New_Position(n + 4, 1); ;
                break;
            }
        }
    }
    public void Load_Naruto_Adventure_Scene()
    {
        if (gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().difficulty == 0) //Normal - GeninASCII
        {
            SceneManager.LoadScene(3);
        }
        else if (gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().difficulty == 1) //Hard - JouninASCII
        {
            SceneManager.LoadScene(4);
        }
        else if (gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().difficulty == 2) //Very Hard - KagASCII
        {
            SceneManager.LoadScene(5);
        }
    }
    public void Exit_Ingame()
    {
        SceneManager.LoadScene(1);
    }

    private void Start()
    {
        //DataAndStuff();
    }
    //#region GetData
    //public void DataAndStuff()
    //{
    //    currentScene = SceneManager.GetActiveScene();
    //    sceneName = currentScene.name;
    //    gameManager = GameObject.FindGameObjectWithTag("GameManager");
    //    if (sceneName == "UzumASCIIAdventure" || sceneName == "UzumASCIIAdventureSummary")
    //    {
    //        preferenceSelection = GameObject.FindGameObjectWithTag("PreferenceStorage");
    //    }
    //    if (sceneName == "UzumASCIIAdventure") //Try with boss scenes!
    //    {
    //        selectionScroll = GameObject.FindGameObjectWithTag("SelectionMenuLoadout");
    //    }
    //    if (sceneName == "MainMenu")
    //    {
    //        menuScroll = GameObject.FindGameObjectWithTag("MainMenu");
    //    }
    //    if (sceneName == "Information")
    //    {
    //        informationBehav = GameObject.FindGameObjectWithTag("InformationScroll");
    //    }
    //}
    //#endregion
    //#region Button Interactions
    //public void HighLight()
    //{
    //    gameManager.GetComponent<Manager>().sound_Effects[0].Play();
    //}
    //public void MyWebsite()
    //{
    //    Application.OpenURL("https://devmarley.itch.io/"); //My website :) shut the fuck up
    //}
    //public void information()
    //{
    //    StartCoroutine(InformationMenu());
    //}
    //public void BackToMainMenuFromInformation()
    //{
    //    StartCoroutine(BackToMenu());
    //}
    //public void BackToMainMenuFromPreferenceSelection()
    //{
    //    StartCoroutine(BackToMainMenu());
    //}
    //public void StartUchihASCIIBoss()
    //{
    //    StartCoroutine(StartUchihASCIIBossAfterSelection());
    //}
    //public void StartUzumASCIIAdventure()
    //{
    //    StartCoroutine(StartAdventureMode());
    //}
    //public void UzumASCIIAdventure()
    //{
    //    StartCoroutine(UzumASCIIADventureDelay());
    //}
    //public void UchihASCIISelection()
    //{
    //    StartCoroutine(UchihASCIIDelay());
    //}
    //public void UzumASCIIBossSelection()
    //{
    //    StartCoroutine(UzumASCIIDelay());
    //}
    //public void TrainingScroll()
    //{
    //    menuScroll.GetComponent<MainMenuBehaviour>().Scroll_Transition_Out();
    //    tScroll.GetComponent<TrainingScrollBehaviour>().ScrollOn();
    //    //StartCoroutine(TrainingScrollScroll()); //pff
    //}
    //public void LoadToTrainUzumASCII()
    //{
    //    StartCoroutine(TrainingLoadUzumASCII());
    //}
    //public void LoadToTrainUchihASCII()
    //{
    //    StartCoroutine(TrainingLoadUchihASCII());
    //}
    //public void TrainingScrollOff()
    //{
    //    StartCoroutine(TrainingScrollScrollBack());
    //}
    //public void BackToMenuFromIngame()
    //{
    //    StartCoroutine(BackToMainMenuDelay());
    //}
    //public void ContinueGame()
    //{
    //    Time.timeScale = 1.0f;
    //    pScroll.GetComponent<Animator>().SetFloat("pauseValue", 0);
    //}
    //public void RetryFromSummary() //
    //{
    //    gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().score = 0;
    //    gameManager.GetComponent<Manager>().SaveCurrentStateOrLoadCurrentState();
    //    StartCoroutine(RetryFromSummaryDelay());
    //}
    //public void ExitFromSummary()
    //{
    //    gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().score = 0;
    //    gameManager.GetComponent<Manager>().SaveCurrentStateOrLoadCurrentState();
    //    StartCoroutine(ExitFromSummaryDelay());
    //}
    //public void TrainingGroundLoadScene()
    //{
    //    StartCoroutine(LoadTrainingGroundDel());
    //}
    //public void StartUzumASCIIBoss()
    //{
    //    StartCoroutine(StartUzumASCIIBossFromSelection());
    //}
    //public void MedallionScrollExit()
    //{
    //    StartCoroutine(DelayMedallionScroll());
    //}
    //public void MedallionScrollEnter()
    //{
    //    StartCoroutine(DelayToMedallionScene());
    //}
    //public void MenuFromSaveSystem()
    //{
    //    StartCoroutine(DelayToMenuFromSaveSystem());
    //}
    //public void SaveWipe()
    //{
    //    gameManager.GetComponent<Manager>().DeleteCurrentSave();
    //    StartCoroutine(DelayToWipe());
    //}
    //public void EraseProgression()
    //{
    //    StartCoroutine(EraseProgressionDelay());
    //}

    //public void LoadUchihASCIITrainSceneGame() //ok?
    //{
    //    StartCoroutine(LoadTrainingGroundDelUch());
    //}
    //public void QuitGame()
    //{
    //    gameManager.GetComponent<Manager>().SaveCurrentStateOrLoadCurrentState();
    //    StartCoroutine(QuitGameDelay());
    //}
    //public void LoadSasukeTutorial()
    //{
    //    StartCoroutine(LoadTutSasDel());
    //}
    //public void LoadNarutoTutorial()
    //{
    //    StartCoroutine(LoadTutNar());
    //}
    //#endregion
    //#region Button IEnumerators 
    //IEnumerator LoadTutSasDel() 
    //{
    //    tScroll.GetComponent<TrainingScrollBehaviour>().ScrollBack();
    //    yield return new WaitForSeconds(0.5f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(19);
    //}
    //IEnumerator LoadTutNar()
    //{
    //    tScroll.GetComponent<TrainingScrollBehaviour>().ScrollBack();
    //    yield return new WaitForSeconds(0.1f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(18);
    //}
    //IEnumerator LoadTrainingGroundDel() //Gay rights
    //{
    //    selectionScroll.GetComponent<PreferenceSelectionScrollBehaviour>().ScrollBackToMainMenu();
    //    yield return new WaitForSeconds(0.5f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(12);
    //} 
    //IEnumerator TrainingLoadUzumASCII()
    //{
    //    tScroll.GetComponent<TrainingScrollBehaviour>().ScrollBack();
    //    yield return new WaitForSeconds(0.1f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(15);
    //}
    //IEnumerator LoadTrainingGroundDelUch()
    //{
    //    selectionScroll.GetComponent<PreferenceSelectionScrollBehaviour>().ScrollBackToMainMenu();
    //    yield return new WaitForSeconds(0.5f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(17);
    //}
    //IEnumerator TrainingLoadUchihASCII()
    //{
    //    tScroll.GetComponent<TrainingScrollBehaviour>().ScrollBack();
    //    yield return new WaitForSeconds(0.1f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(16);
    //}

    //IEnumerator QuitGameDelay()
    //{
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    Application.Quit();
    //}
    //IEnumerator EraseProgressionDelay()
    //{
    //    menuScroll.GetComponent<MainMenuBehaviour>().Scroll_Transition_Out();
    //    yield return new WaitForSeconds(0.25f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(14);
    //}

    //IEnumerator DelayToWipe()
    //{
    //    sScroll.GetComponent<Animator>().SetTrigger("trigger"); //Again, with one of the IEnums ; Reusable variable, SAVE THE TREES!
    //    yield return new WaitForSeconds(1.5f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(1);
    //}
    //IEnumerator DelayToMenuFromSaveSystem()
    //{
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(1);
    //}
    //IEnumerator DelayToMedallionScene()
    //{
    //    menuScroll.GetComponent<MainMenuBehaviour>().Scroll_Transition_Out();
    //    yield return new WaitForSeconds(0.25f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(11);
    //}

    //IEnumerator DelayMedallionScroll()
    //{
    //    sScroll.GetComponent<Animator>().SetTrigger("trig"); //Don't want to declare another variable ; So I use an undeclared, existing one!
    //    yield return new WaitForSeconds(0.25f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(1);
    //}
    //IEnumerator StartUzumASCIIBossFromSelection()
    //{
    //    selectionScroll.GetComponent<PreferenceSelectionScrollBehaviour>().ScrollBackToMainMenu();
    //    yield return new WaitForSeconds(0.5f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(8);
    //}

    ////Delay for buttons, otherwise it doesn't work, dumb shit right ? //Whats 
    //IEnumerator RetryFromSummaryDelay()
    //{
    //    sScroll.GetComponent<Animator>().SetTrigger("scroll");
    //    yield return new WaitForSeconds(0.5f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    if (preferenceSelection.GetComponent<PreferenceStorage>().bossValues == 0)
    //    {
    //        if (preferenceSelection.GetComponent<PreferenceStorage>().difficulty == 0)
    //        {
    //            SceneManager.LoadScene(3);
    //        }
    //        else if (preferenceSelection.GetComponent<PreferenceStorage>().difficulty == 1)
    //        {
    //            SceneManager.LoadScene(4);
    //        }
    //        else if (preferenceSelection.GetComponent<PreferenceStorage>().difficulty == 2)
    //        {
    //            SceneManager.LoadScene(5);
    //        }
    //    }else if (preferenceSelection.GetComponent<PreferenceStorage>().bossValues == 1 || preferenceSelection.GetComponent<PreferenceStorage>().bossValues == 3) //UchihASCII
    //    {
    //        SceneManager.LoadScene(10);
    //        preferenceSelection.GetComponent<PreferenceStorage>().bossValues = 1;
    //    }else if (preferenceSelection.GetComponent<PreferenceStorage>().bossValues == 2 || preferenceSelection.GetComponent<PreferenceStorage>().bossValues == 4) //UzumASCII
    //    {
    //        SceneManager.LoadScene(8);
    //        preferenceSelection.GetComponent<PreferenceStorage>().bossValues = 2;
    //    }

    //}
    //IEnumerator ExitFromSummaryDelay()
    //{
    //    sScroll.GetComponent<Animator>().SetTrigger("scroll");
    //    yield return new WaitForSeconds(0.75f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(1);
    //}
    //IEnumerator StartUchihASCIIBossAfterSelection() //UchihASCII LoadoutSelect -> Ingame boss fight UchihASCII
    //{
    //    selectionScroll.GetComponent<PreferenceSelectionScrollBehaviour>().ScrollBackToMainMenu();
    //    yield return new WaitForSeconds(0.25f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(10);
    //}
    //IEnumerator BackToMainMenuDelay() //ingame -> Main menu
    //{
    //    pScroll.GetComponent<Animator>().SetFloat("pauseValue", 0);
    //    yield return new WaitForSeconds(0.05f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.05f);
    //    Time.timeScale = 1.0f;// uhm ok?
    //    SceneManager.LoadScene(1);
    //    Time.timeScale = 1.0f;
    //}
    //IEnumerator InformationMenu() //Main menu -> Information scene
    //{
    //    menuScroll.GetComponent<MainMenuBehaviour>().Scroll_Transition_Out();
    //    yield return new WaitForSeconds(0.25f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(13);
    //}
    //IEnumerator TrainingScrollScrollBack() //Training Mode -> Back to menu ; This backs of the training menu, and activates menu button
    //{
    //    tScroll.GetComponent<TrainingScrollBehaviour>().ScrollBack();
    //    yield return new WaitForSeconds(0.1f);
    //    menuScroll.GetComponent<MainMenuBehaviour>().Scroll_Transition_In();
    //}
    //IEnumerator UzumASCIIDelay() //Menu -> Neji bossfight/ UzumASCII loadout select
    //{
    //    menuScroll.GetComponent<MainMenuBehaviour>().Scroll_Transition_Out();
    //    yield return new WaitForSeconds(0.25f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(7);
    //}
    //IEnumerator UzumASCIIADventureDelay() //Menu -> Loadout select for endless mode
    //{
    //    menuScroll.GetComponent<MainMenuBehaviour>().Scroll_Transition_Out();
    //    yield return new WaitForSeconds(0.25f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(2);
    //}
    //IEnumerator UchihASCIIDelay() //Menu -> UchihASCII loadout select
    //{
    //    menuScroll.GetComponent<MainMenuBehaviour>().Scroll_Transition_Out();
    //    yield return new WaitForSeconds(0.25f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(9);
    //}
    //IEnumerator BackToMainMenu() //-> To main menu
    //{
    //    selectionScroll.GetComponent<PreferenceSelectionScrollBehaviour>().ScrollBackToMainMenu();
    //    yield return new WaitForSeconds(0.5f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(1);
    //}
    //IEnumerator BackToMenu() //Information Menu -> Main menu
    //{
    //    informationBehav.GetComponent<InformationScrollBehavior>().ScrollBack();
    //    yield return new WaitForSeconds(0.3f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    SceneManager.LoadScene(1);
    //}
    //IEnumerator StartAdventureMode() //Adventure loadout select -> ingame
    //{
    //    selectionScroll.GetComponent<PreferenceSelectionScrollBehaviour>().ScrollBackToMainMenu();
    //    yield return new WaitForSeconds(0.5f);
    //    gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
    //    yield return new WaitForSeconds(0.5f);
    //    if (preferenceSelection.GetComponent<PreferenceStorage>().difficulty == 0) //Normal - GeninASCII
    //    {
    //        SceneManager.LoadScene(3);
    //    }
    //    else if (preferenceSelection.GetComponent<PreferenceStorage>().difficulty == 1) //Hard - JouninASCII
    //    {
    //        SceneManager.LoadScene(4);
    //    }
    //    else if (preferenceSelection.GetComponent<PreferenceStorage>().difficulty == 2) //Very Hard - KagASCII
    //    {
    //        SceneManager.LoadScene(5);
    //    }
    //}
    //#endregion
}