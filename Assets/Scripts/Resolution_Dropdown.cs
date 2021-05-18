using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution_Dropdown : MonoBehaviour
{
    public GameObject canvas_Manager;

    public void Change_Resolution()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().Check_Dropdown_Value_Resolution();
        StartCoroutine(WaitForScreenChange());
    }
    private IEnumerator WaitForScreenChange()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Screen.SetResolution(canvas_Manager.GetComponent<Canvas_Manager>().resolution_Width, canvas_Manager.GetComponent<Canvas_Manager>().resolution_Height, canvas_Manager.GetComponent<Canvas_Manager>().full_Screen);
    }
    public void Change_Show_FPS_Toggle()
    {
        canvas_Manager.GetComponent<Canvas_Manager>().Check_Current_FPS_Toggle();
        canvas_Manager.GetComponent<Canvas_Manager>().fps_Text_UI.gameObject.SetActive(canvas_Manager.GetComponent<Canvas_Manager>().show_FPS);
    }

}
