using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileCreatorAndDestroyer : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    public GameObject gameManager;
    [Header("Hitboxes, to contain the player or interact with the player")]
    public BoxCollider2D[] hitbox;
    [Header("Adventure mode, inst. tiles randomly")]
    public GameObject tileStorage;
    [Header("Adventure mode, Transform for the random Tiles")]
    public Transform instPost;
    [Header("Tutorial, Enemies to defeat before letting the player progress")]
    public GameObject[] enemies;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (gameObject.name == "CheckPointer" && gameObject.name != "TutEnd")
        {
            hitbox[0].enabled = true;
            hitbox[1].enabled = false;
        }
        else if (gameObject.name != "RTFINORMAL" && gameObject.name != "RTFNORMAL" && gameObject.name != "RTONORMAL" && gameObject.name != "RTSNORMAL" && gameObject.name != "RTTHNORMAL" && gameObject.name != "RTTNORMAL" && gameObject.name != "CheckPointer" && gameObject.name != "EnemyChecker" && gameObject.name != "TutEnd")
        {
            tileStorage = GameObject.FindGameObjectWithTag("TileSystem");
            for (int i = 0; i < (hitbox.Length - 1); i++)
            {
                hitbox[i].enabled = false;
            }
        }else if (gameObject.name == "EnemyChecker" && gameObject.name != "TutEnd")
        {
            GetComponentInParent<Animator>().SetTrigger("triggerFall");
            hitbox[0].enabled = true;
            hitbox[1].enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) //it's okay, doesn't hinder performance, only runs once
    {
        if (collision.CompareTag("Player") && gameObject.name == "TileCreator")
        {
            Destroy(gameObject);
            Instantiate(tileStorage.GetComponent<InfiniteTileSystem>().tiles[Random.Range(0, tileStorage.GetComponent<InfiniteTileSystem>().tiles.Length)], instPost.transform.position, Quaternion.identity); 
        }
        else if (collision.CompareTag("Player") && gameObject.name == "TileDestroyer")
        {
            GetComponentInParent<Animator>().SetTrigger("triggerFall");
            for (int i = 0; i < (hitbox.Length - 1); i++)
            {
                hitbox[i].enabled = true;
            }
            hitbox[2].enabled = false;
        }
        else if (collision.CompareTag("Player") && gameObject.name == "CheckPointer")
        {
            GetComponentInParent<Animator>().SetTrigger("triggerFall");
            hitbox[1].enabled = true;
            hitbox[0].enabled = false;
        }
        else if (collision.CompareTag("Wall") && gameObject.name == "RTFINORMAL(Clone)" || collision.CompareTag("Wall") && gameObject.name == "RTFNORMAL(Clone)" || collision.CompareTag("Wall") && gameObject.name == "RTONORMAL(Clone)" || collision.CompareTag("Wall") && gameObject.name == "RTSNORMAL(Clone)" || collision.CompareTag("Wall") && gameObject.name == "RTTNORMAL(Clone)" || collision.CompareTag("Wall") && gameObject.name == "RTTHNORMAL(Clone)")
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player") && enemies[0].GetComponent<EnemyState>().health <= 0 && enemies[1].GetComponent<EnemyState>().health <= 0 && gameObject.name == "EnemyChecker")
        {
            GetComponentInParent<Animator>().SetTrigger("triggerRise");
            hitbox[1].enabled = false;
            hitbox[0].enabled = false;
        }
        else if (collision.CompareTag("Player") && gameObject.name == "TutEnd")
        {
            StartCoroutine(DelayMenu());
            IEnumerator DelayMenu()
            {
                gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
                yield return new WaitForSeconds(0.5f);
                SceneManager.LoadScene(1);
            }
        }
    }
}