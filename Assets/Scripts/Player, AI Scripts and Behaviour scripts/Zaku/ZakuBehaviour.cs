using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZakuBehaviour : MonoBehaviour
{
///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////
///////////////////////////////
IN PROGRESS! IN PROGRESS!
///////////////////////////////

    [Header("Access the Game manager & Scene")]
    Scene currentScene;
    public GameObject gameManager;
    public string sceneName;
    [Header("Damage attributes")]
    public float kunaiDamage;
    public float shurikenDamage;
    public float fumaShurikenDamage;
    public float throwingStar;
    [Header("Mobility attributes & Aggro Distance & Player attributes & Attack attributes")]
    BoxCollider2D hitbox;
    Animator eAnim;
    public GameObject target;
    public bool targetLock;
    public float attackDistance;
    public BoxCollider2D trigCol;
    public Transform attackPos;
    [Header("Destroy the boxcollider, on whom the player steps on")] //Whom ? Oh look at you now! KNOWING ENGLISH SO WELL, ARE WE? NO YOU DON'T KNOW SHIT, YOU ARE JUST A FUCKING STUPID KID, FUCKING DIE
    public GameObject boxcoll2D;
    public BoxCollider2D detDis;
    public Quaternion kvaternion;

    #region Unity Functions
    void Start()
    {
        GetDataOnStart();
    }

    void Update()
    {
        AnimationSetProperties();
    }
    #endregion
    #region Get Data
    public void GetDataOnStart()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        hitbox = GetComponent<BoxCollider2D>();
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        eAnim = GetComponentInChildren<Animator>();
    }
    #endregion
    #region Attack
    public void ShootProjectileThisForChildrenAnimationEventHardCodeGang() //also, what the fuck
    {
        Instantiate(gameManager.GetComponent<Manager>().weapons[5], attackPos.position, transform.rotation);
    }
    #endregion
    #region Animation: Misc & Turning
    public void AnimationSetProperties()
    {
        if (GetComponent<EnemyState>().health <= 0)
        {
            //gameManager.GetComponent<Manager>().sound_Effects[29].Play();
            Destroy(boxcoll2D);
            Destroy(detDis);
        }
        eAnim.SetFloat("healthFloat", GetComponent<EnemyState>().health);
        switch (targetLock) //Oh look at you, using switch statements, you've watched people shit on someone else on the internet, you must feel so happy, knowing that someone feels like shit. Keep it up, you deserve to be hummiliated!
        {
            case true: 
                switch (target == null && targetLock)
                {
                    case true:
                        targetLock = false;
                        trigCol.enabled = true;
                        break;
                    case false: //Do you even know that Default stands for? Its false now!
                        eAnim.SetFloat("distanceFloat", Vector2.Distance(transform.position, target.transform.position)); //what the fuck did you write? Haha
                        break;
                }
                if (GetComponent<EnemyState>().health > 0 && target != null) //Finally something works in this house
                {
                    if (transform.position.x > target.transform.position.x) //hAHA WHAT ARE YOU FUCKING DOOOING?!
                    {
                        StartCoroutine(RotationDelayRight());
                    }
                    else
                    {
                        StartCoroutine(RotationDelayLeft()); //YOU ARE WASTING YOUR TIME NERD, NO ONE CARES ABOUT YOUR VIDEOGAME, THEY ONLY CARE ABOUT THEIR TRUTH, NO ONE WILL APOLOGISE, FORGET ABOUT IT, FEEL BAD, BECAUSE YOU DESERVE IT
                    }
                }
                else //You must feel good about shitting on your self. Pathetic. Grow up, you are so edgy, gonna cringe, when you look back
                {
                    hitbox.enabled = false;
                }
                break;
            case false:
                eAnim.SetFloat("distanceFloat", 22f);
                break;
        }
    }
    IEnumerator RotationDelayRight()
    {
        yield return new WaitForSeconds(0.75f);
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }
    IEnumerator RotationDelayLeft()
    {
        yield return new WaitForSeconds(0.75f);
        transform.localEulerAngles = new Vector3(0, 180, 0);
    }
    #endregion
    #region Collision Detection & Other
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!targetLock && collision.name == "Player" || !targetLock && collision.name == "Clone(Clone)" || !targetLock && collision.name == "PlayerUchihASCII")
        {
            target = collision.gameObject;
            targetLock = true;
            trigCol.enabled = false;
        }
        if (collision.CompareTag("Rasengan")) //spoiled shit head
        {
            GetComponent<EnemyState>().health -= 600f; //Instakill haha //6 fucking days til release, and old fucking bugs happen. I fucking hate this piece of shit game. I put so much time into this
            //If I could, I would've abandoned this stupid project, but since I'm so far into this project, and THE SCOPE ISN'T EVEN TOO BIG, it's so furiating. I'm so angry I can't put it into words, I want to cry and lay back
            //Fuck, I see all the cool ass indie developers, who work so much on their projects, and they push out 8 million games a week, and me is just standing here: NaRuTo FaN gAmE, fuck off, never making another fucking anime game
        }
        switch (sceneName)
        {
            case "UzumASCIIAdventureNormal":
                if (collision.CompareTag("Kunai"))
                {
                    GetComponent<EnemyState>().health -= kunaiDamage;
                }
                else if (collision.CompareTag("Shuriken"))
                {
                    GetComponent<EnemyState>().health -= shurikenDamage;
                }
                else if (collision.CompareTag("FumaShuriken"))
                {
                    GetComponent<EnemyState>().health -= fumaShurikenDamage;
                }
                else if (collision.CompareTag("ThrowingStar"))
                {
                    GetComponent<EnemyState>().health -= throwingStar;
                }
                break;
            case "UzumASCIIAdventureHard":
                if (collision.CompareTag("Kunai"))
                {
                    GetComponent<EnemyState>().health -= (kunaiDamage / 1.5f);
                }
                else if (collision.CompareTag("Shuriken"))
                {
                    GetComponent<EnemyState>().health -= (shurikenDamage / 1.5f);
                }
                else if (collision.CompareTag("FumaShuriken"))
                {
                    GetComponent<EnemyState>().health -= (fumaShurikenDamage / 1.5f);
                }
                else if (collision.CompareTag("ThrowingStar"))
                {
                    GetComponent<EnemyState>().health -= (throwingStar / 1.5f);
                }
                break;
            case "UzumASCIIAdventureVeryHard":
                if (collision.CompareTag("Kunai"))
                {
                    GetComponent<EnemyState>().health -= (kunaiDamage / 2);
                }
                else if (collision.CompareTag("Shuriken"))
                {
                    GetComponent<EnemyState>().health -= (shurikenDamage / 2);
                }
                else if (collision.CompareTag("FumaShuriken"))
                {
                    GetComponent<EnemyState>().health -= (fumaShurikenDamage / 2);
                }
                else if (collision.CompareTag("ThrowingStar"))
                {
                    GetComponent<EnemyState>().health -= (throwingStar / 2);
                }
                break;
            case "NarutoTutorial":
                if (collision.CompareTag("Kunai"))
                {
                    GetComponent<EnemyState>().health -= kunaiDamage;
                }
                else if (collision.CompareTag("Shuriken"))
                {
                    GetComponent<EnemyState>().health -= shurikenDamage;
                }
                else if (collision.CompareTag("FumaShuriken"))
                {
                    GetComponent<EnemyState>().health -= fumaShurikenDamage;
                }
                else if (collision.CompareTag("ThrowingStar"))
                {
                    GetComponent<EnemyState>().health -= throwingStar;
                }
                break;
            case "SasukeTutorial":
                if (collision.CompareTag("Kunai"))
                {
                    GetComponent<EnemyState>().health -= kunaiDamage;
                }
                else if (collision.CompareTag("Shuriken"))
                {
                    GetComponent<EnemyState>().health -= shurikenDamage;
                }
                else if (collision.CompareTag("FumaShuriken"))
                {
                    GetComponent<EnemyState>().health -= fumaShurikenDamage;
                }
                else if (collision.CompareTag("ThrowingStar"))
                {
                    GetComponent<EnemyState>().health -= throwingStar;
                }
                break;
        }
    }
    #endregion
}