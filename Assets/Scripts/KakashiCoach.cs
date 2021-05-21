using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KakashiCoach : MonoBehaviour
{

///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    //Typing script, Input Sentence, and it types it out
    [Header("Access Gamemanager")]
    public GameObject gameManager;
    [Header("Sentence input attributes")]
    public string inputSentence;
    public string outputSentence;
    [Space]
    public TextMesh text;
    [Space]
    public float sentenceRowLenght;
    [Header("Trigger for sentence to output")]
    public CircleCollider2D triggerCol;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    public void OutputSentence()
    {
        StartCoroutine(TypeIENUM());
        IEnumerator TypeIENUM()
        {
            for (int i = 0; i < inputSentence.Length; i++)
            {
                gameManager.GetComponent<Manager>().sound_Effects[21].Play();
                if (i > sentenceRowLenght && inputSentence[i] == ' ' || i > sentenceRowLenght && inputSentence[i] == ',' || i > sentenceRowLenght && inputSentence[i] == '.' || i > sentenceRowLenght && inputSentence[i] == '!' || i > sentenceRowLenght && inputSentence[i] == '?')
                {
                    outputSentence += inputSentence[i];
                    outputSentence += "\n";
                    sentenceRowLenght += sentenceRowLenght;
                }
                else
                {
                    outputSentence += inputSentence[i];
                }
                text.text = outputSentence.ToString();
                yield return new WaitForSeconds(0.05f); //Nice scripting
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OutputSentence();
            triggerCol.enabled = false;
        }
    }
}
