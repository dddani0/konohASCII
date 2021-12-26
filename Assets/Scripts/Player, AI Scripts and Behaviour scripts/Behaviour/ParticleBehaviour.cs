using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticleBehaviour : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    Scene cScene;
    string cSName;
    GameObject player;

    private void Start()
    {
        cScene = SceneManager.GetActiveScene();
        cSName = cScene.name;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        ParticleOff();

        switch (cSName)
        {
            case "UzumASCIIAdventureNormal":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue != 0)
                {
                    transform.position = player.transform.position;
                }
                break;
            case "UzumASCIIAdventureHard":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue != 0)
                {
                    transform.position = player.transform.position;
                }
                break;
            case "UzumASCIIAdventureVeryHard":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue != 0)
                {
                    transform.position = player.transform.position;
                }
                break;
            case "UzumASCIIBoss":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue != 0)
                {
                    transform.position = player.transform.position;
                }
                break;
            case "UchihASCIIBoss":
                if (player.GetComponent<PlayerScriptUchihASCII>().jutsuVal != 0)
                {
                    transform.position = player.transform.position;
                }
                break;
            case "TrainingModeNaruto":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue != 0)
                {
                    transform.position = player.transform.position;
                }
                break;
            case "TrainingModeSasuke":
                if (player.GetComponent<PlayerScriptUchihASCII>().jutsuVal != 0)
                {
                    transform.position = player.transform.position;
                }
                break;
        }
    }

    public void ParticleOff()
    {
        switch (cSName)
        {
            case "UzumASCIIAdventureNormal":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue == 0)
                {
                    StartCoroutine(ParticleWaitSeconds());
                }
                break;
            case "UzumASCIIAdventureHard":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue == 0)
                {
                    StartCoroutine(ParticleWaitSeconds());
                }
                break;
            case "UzumASCIIAdventureVeryHard":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue == 0)
                {
                    StartCoroutine(ParticleWaitSeconds());
                }
                break;
            case "UzumASCIIBoss":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue == 0)
                {
                    StartCoroutine(ParticleWaitSeconds());
                }
                break;
            case "UchihASCIIBoss":
                if (player.GetComponent<PlayerScriptUchihASCII>().jutsuVal == 0)
                {
                    StartCoroutine(ParticleWaitSeconds());
                }
                break;
            case "TrainingModeNaruto":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue == 0)
                {
                    StartCoroutine(ParticleWaitSeconds());
                }
                break;
            case "TrainingModeSasuke":
                if (player.GetComponent<PlayerScriptUchihASCII>().jutsuVal == 0)
                {
                    StartCoroutine(ParticleWaitSeconds());
                }
                break;
            case "NarutoTutorial":
                if (player.GetComponent<PlayerScriptUzumASCII>().jutsuValue == 0)
                {
                    StartCoroutine(ParticleWaitSeconds());
                }
                break;
            case "SasukeTutorial":
                if (player.GetComponent<PlayerScriptUchihASCII>().jutsuVal == 0)
                {
                    StartCoroutine(ParticleWaitSeconds());
                }
                break;
        }
    }
    IEnumerator ParticleWaitSeconds()
    {
        GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(3);
        GetComponent<ParticleBehaviour>().enabled = false;
        Destroy(gameObject);
    }
}