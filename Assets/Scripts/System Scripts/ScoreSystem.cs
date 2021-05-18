using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    [Header("Access the Preference GameObject")]
    public GameObject preferenceG;
    [Header("Display Attributes for Adventure Mode X Difficulty")]
    public Text score;

    private void Start()
    {
        preferenceG = GameObject.FindGameObjectWithTag("PreferenceStorage");
    }

    private void Update()
    {
        SetPoint();
    }

    public void SetPoint()
    {
        score.text = preferenceG.GetComponent<PreferenceStorage>().score.ToString();
    }
}
