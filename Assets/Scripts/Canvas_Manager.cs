using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Canvas_Manager : MonoBehaviour
{
///////////////////////////////
IN PROGRESS! IN PROGRESS!
///////////////////////////////

    [Header("Game Manager")]
    public GameObject game_Manager;

    //Main Menu Variables

    [Header("Main Menu buttons")]
    [Tooltip("Buttons")] public GameObject main_Menu_Panel;
    [Header("Challenge Mode Menu")]
    [Tooltip("Challenge menu mode")]public GameObject challenge_Mode_Menu;
    [Header("Training Mode Menu")]
    public GameObject training_Mode_Menu;
    [Header("Information Mode Menu")]
    public GameObject information_Menu;
    [Header("Settings Menu & attributes")]
    public GameObject settings_Menu;
    [Space]
    public Slider fps_Slider;
    [Space]
    public Dropdown resolution_Dropdown;
    [Space]
    public Toggle show_FPS_Toggle;
    [Space]
    public Toggle fullscreen_Toggle;
    [Space]
    public bool show_FPS;
    [Space]
    public bool full_Screen;
    [Space]
    public int resolution_Width;
    public int resolution_Height;
    [Space]
    public Text fps_Text_UI;
    [Space]
    public Text version_Text_UI;
    [Header("Collectible Menu")]
    public GameObject collectible_Menu;
    [Header("Save Management Menu")]
    public GameObject save_Management_Menu;
    [Header("Menu sub-texts")]
    [Tooltip("Non essential texts from the Menu")]public GameObject[] sub_Texts;

    //Adventure Loadout Variables

    [Header("Weapon UI Selection")]
    public GameObject[] weapon_Select_UI_Element;
    [Header("Difficulty UI Selection")]
    public GameObject[] difficulty_Select_UI_Element;
    [Header("Selected Index UI Element")]
    public GameObject[] preference_Selected_Index_Identifier_UI_Element;
    [Space]
    public GameObject[] preference_Selected_Index_Identifier_UI_Element_Positions;

    // Start is called before the first frame update
    void Start()
    {
        switch (game_Manager.GetComponent<Manager>().current_Loaded_Scene_Name)
        {
            case "MainMenu":
                Check_Current_FPS_Toggle();
                main_Menu_Panel.SetActive(true);
                challenge_Mode_Menu.SetActive(false);
                training_Mode_Menu.SetActive(false);
                information_Menu.SetActive(false);
                settings_Menu.SetActive(false);
                collectible_Menu.SetActive(false);
                save_Management_Menu.SetActive(false);
                resolution_Height = Screen.height;
                resolution_Width = Screen.width;
                //
                version_Text_UI.text = ("ver " + Application.version).ToString();
                break;
            case "UzumASCIIAdventure":
                Debug.Log("UzumASCIIADVENTURE");
                for (int i = 1; i < weapon_Select_UI_Element.Length; i++)
                {
                    weapon_Select_UI_Element[i].GetComponent<Button_Select>().isSelected = false;
                }

                for (int i = 1; i < difficulty_Select_UI_Element.Length; i++)
                {
                    difficulty_Select_UI_Element[i].GetComponent<Button_Select>().isSelected = false;
                }

                Debug.Log(weapon_Select_UI_Element[0].GetComponent<Text>().rectTransform.position);

                preference_Selected_Index_Identifier_UI_Element[0].GetComponent<Text>().rectTransform.position = preference_Selected_Index_Identifier_UI_Element_Positions[0].GetComponent<RectTransform>().position; //0 ; 1 ; 2 ; 3
                preference_Selected_Index_Identifier_UI_Element[1].GetComponent<Text>().rectTransform.position = preference_Selected_Index_Identifier_UI_Element_Positions[4].GetComponent<RectTransform>().position; //4 ; 5 ; 6 ;

                Debug.Log(weapon_Select_UI_Element[0].GetComponent<Text>().rectTransform.position);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Show_FPS_Number();
    }

    public void Set_Preference_Index_New_Position(int new_Pos, int index)
    {
        preference_Selected_Index_Identifier_UI_Element[index].GetComponent<Text>().rectTransform.position = preference_Selected_Index_Identifier_UI_Element_Positions[new_Pos].GetComponent<RectTransform>().position;
    }

    public void Show_FPS_Number()
    {
        if (show_FPS)
            fps_Text_UI.text = ("FPS " + Application.targetFrameRate).ToString();
    }

    public void Change_Fps_Value_Slider()
    {
        switch (fps_Slider.value)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;
            case 1:
                Application.targetFrameRate = 60;
                break;
        }
    }

    public void Check_Current_FPS_Toggle()
    {
        show_FPS = show_FPS_Toggle.isOn;
    }

    public void Check_Dropdown_Value_Resolution()
    {
        full_Screen = fullscreen_Toggle.isOn;
        resolution_Height = Screen.height;
        //resolution_Width = Screen.width;

        switch (resolution_Dropdown.value)
        {
            case 0: //1920 x 1080
                resolution_Width = 1920;
                resolution_Height = 1080;
                break;
            case 1: //
                resolution_Width = 1768;
                resolution_Height = 992;
                break;
            case 2:
                resolution_Width = 1600;
                resolution_Height = 1024;
                break;
            case 3:
                resolution_Width = 1440;
                resolution_Height = 900;
                break;
            case 4:
                resolution_Width = 1366;
                resolution_Height = 768;
                break;
            case 5:
                resolution_Width = 1280;
                resolution_Height = 768;
                break;
            case 6:
                resolution_Width = 1176;
                resolution_Height = 664;
                break;
            case 7:
                resolution_Width = 1024;
                resolution_Height = 768;
                break;
            case 8:
                resolution_Width = 800;
                resolution_Height = 600;
                break;
        }
    }

    public void Show_Current_Settings_Configuration()
    {
        for (int i = 0; i < resolution_Dropdown.options.Count; i++)
        {
            if (resolution_Dropdown.options[i].text == (resolution_Width + " x " + resolution_Height).ToString())
            {
                resolution_Dropdown.captionText.text = resolution_Dropdown.options[i].text;
            }
        }

        Check_Dropdown_Value_Resolution();

        switch (Application.targetFrameRate)
        {
            case 30:
                fps_Slider.value = 0;
                break;
            case 60:
                fps_Slider.value = 1;
                break;
        }

        //Temporary Default Values in the settings menu

        //Default = later player sets it up

        fullscreen_Toggle.isOn = Screen.fullScreen;

        show_FPS_Toggle.isOn = false;

        //fps_Inputfield.text = "60".ToString();
    }
}
