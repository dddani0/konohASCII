using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThrowableBehaviour : MonoBehaviour
{
    /// <summary>
    /// Prefabs, which has this script attached to it:
    /// -Kunai 
    /// -Shuriken
    /// -Giant Shuriken
    /// -Throwing Star
    /// 
    /// -Rasengan
    /// -Whrilwind Projectile
    /// </summary>

    //No need -> Delete when re writing the script
    Scene curScene;
    public string curSceneName;
    [Header("Gamemanager access")]
    public GameObject gManager;
    Animator wAnim;
    Rigidbody2D wBody;
    [Header("Weapon attributes")]
    public float speed;
    [Space]
    [Tooltip("For weapons which spin")]public AudioSource spinSFX;
    [Space]
    public LayerMask[] layers;

    void Start()
    {
        GetData();
    }

    public void GetData()
    {
        curScene = SceneManager.GetActiveScene();
        curSceneName = curScene.name;
        gManager = GameObject.FindGameObjectWithTag("GameManager");
        wBody = GetComponent<Rigidbody2D>();
        if (!gameObject.name.Contains("Kunai"))
        {
            wAnim = GetComponentInChildren<Animator>();
        }
        if (gameObject.name == "Shuriken(Clone)" || gameObject.name == "FumaShuriken(Clone)" || gameObject.name == "ThrowingStar(Clone)")
        {
            spinSFX = GetComponent<AudioSource>();
        }
        gManager.GetComponent<Manager>().instances.Add(this.gameObject);
    }

    void Update()
    {
        Throwable_Action();
    }

    public void Throwable_Action()
    {
        switch (gameObject.name)
        {
            case "Rasengan(Clone)": //Move after "intro" animation
                if (!wAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion") && wAnim.speed != 0)
                {
                    wBody.velocity = transform.right * speed;
                }
                else
                {
                    wBody.velocity = transform.right * 0;
                }
                break;
            case "Kunai(Clone)": //Kunai
                if (!gManager.GetComponent<Manager>().pause)
                    wBody.velocity = transform.right * speed;
                else
                    wBody.velocity = new Vector2(0, 0);
                //if (gameObject.tag != "WPickUp")
                //{
                //    transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
                //}
                break;
            case "Shuriken(Clone)": //Shuriken
                wBody.velocity = transform.right * speed * wAnim.speed;
                //if (gameObject.tag != "WPickUp")
                //{
                //    transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
                //}
                break;
            case "FumaShuriken(Clone)": //Fuma Shuriken
                wBody.velocity = transform.right * speed * wAnim.speed;
                //if (gameObject.tag != "WPickUp")
                //{
                //    transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
                //}
                break;
            case "ThrowingStar(Clone)": //Throwing Star
                wBody.velocity = transform.right * speed * wAnim.speed;
                //if (gameObject.tag != "WPickUp")
                //{
                //    transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
                //}
                break;
            case "WhirlwindProjectile(Clone)":
                wBody.velocity = -transform.right * speed * wAnim.speed;
                //StartCoroutine(WhirlwindProjectileScaler());
                //Destroy(gameObject, 3.5f);
                break;
            case "CloneKunai(Clone)":
                //if (gameObject.tag != "WPickUp")
                //{
                //    transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
                //}
                break;
            case "CloneShuriken(Clone)":
                //if (gameObject.tag != "WPickUp")
                //{
                //    transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
                //}
                break;
            case "CloneFumaShuriken(Clone)":
                //if (gameObject.tag != "WPickUp")
                //{
                //    transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
                //}
                break;
            case "CloneThrowingStar(Clone)":
                //if (gameObject.tag != "WPickUp")
                //{
                //    transform.position = new Vector2(transform.position.x, transform.position.y - 0.01f);
                //}
                break;
        }
    }

    public void DestroyFromChild()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //░█──░█ ─█▀▀█ ░█─── ░█─── 　 ▀█▀ ░█▀▄▀█ ░█▀▀█ ─█▀▀█ ░█▀▀█ ▀▀█▀▀ 
        //░█░█░█ ░█▄▄█ ░█─── ░█─── 　 ░█─ ░█░█░█ ░█▄▄█ ░█▄▄█ ░█─── ─░█── 
        //░█▄▀▄█ ░█─░█ ░█▄▄█ ░█▄▄█ 　 ▄█▄ ░█──░█ ░█─── ░█─░█ ░█▄▄█ ─░█──
        //Kunai impact on wall
        if (gameObject.tag.Contains("Kunai") && collision.CompareTag("Wall")) 
        {
            //gManager.GetComponent<Manager>().sound_Effects[1].Play();
            speed = 0;
            gameObject.tag = "WPickUp";
            //Instantiate(gManager.GetComponent<Manager>().particle_effects[3], transform.position, Quaternion.identity);
        }
        //Other than: Kunai, Rasengan, Whirlwind impact on wall
        else if (gameObject.tag != "Rasengan" && gameObject.tag != "WindProjectile" && collision.CompareTag("Wall")) 
        {
            gameObject.tag = "WPickUp";
            //gManager.GetComponent<Manager>().sound_Effects[1].Play();
            speed = 0;
            wAnim.speed = 0;
            spinSFX.Stop();
            //Instantiate(gManager.GetComponent<Manager>().particle_effects[3], transform.position, Quaternion.identity);
        }
        //Rasengan impact on wall
        else if (gameObject.tag == "Rasengan" && collision.CompareTag("Wall"))
        {
            wAnim.SetTrigger("Destroy");
        }   
        //░█▀▀▄ ─█▀▀█ ░█▀▄▀█ ─█▀▀█ ░█▀▀█ ░█▀▀▀ 　 ▀█▀ ░█▀▄▀█ ░█▀▀█ ─█▀▀█ ░█▀▀█ ▀▀█▀▀ 
        //░█─░█ ░█▄▄█ ░█░█░█ ░█▄▄█ ░█─▄▄ ░█▀▀▀ 　 ░█─ ░█░█░█ ░█▄▄█ ░█▄▄█ ░█─── ─░█── 
        //░█▄▄▀ ░█─░█ ░█──░█ ░█─░█ ░█▄▄█ ░█▄▄▄ 　 ▄█▄ ░█──░█ ░█─── ░█─░█ ░█▄▄█ ─░█──
        if (curSceneName == "UchihASCIIBoss") //SASUKE VS NARUTO
        {
            //Impact with Player, Boss or Clone
            if (!collision.CompareTag("Wall") && gameObject.tag != "WindProjectile" && gameObject.tag != "WPickUp" && gameObject.tag != "Rasengan" )
            {
                //gManager.GetComponent<Manager>().sound_Effects[13].Play();
                //Instantiate(gManager.GetComponent<Manager>().particle_effects[2], transform.position, Quaternion.identity);
                gManager.GetComponent<Manager>().instances.Remove(this.gameObject);
                Destroy(gameObject, 0.01f);
            }
            if (collision.CompareTag("Player") && gameObject.tag == "Rasengan")
            {
                wAnim.SetTrigger("Destroy");
            }
        }
        else if (curSceneName == "UzumASCIIBoss") //NARUTO VS NEJI
        {
            //Impact with Player or Boss
            if (gameObject.tag != "WPickUp" && gameObject.tag == "Kunai" && !collision.CompareTag("Wall") && gameObject.tag != "Rasengan" && collision.GetComponent<HyugASCIIBossScript>().chakraShield <= 0)
            {
                //gManager.GetComponent<Manager>().sound_Effects[13].Play();
                //Instantiate(gManager.GetComponent<Manager>().particle_effects[2], transform.position, Quaternion.identity);
                gManager.GetComponent<Manager>().instances.Remove(this.gameObject);
                Destroy(gameObject, 0.01f);
            }
            if (gameObject.name == "Rasengan(Clone)" && CompareTag("Wall") || gameObject.name == "Rasengan(Clone)" && collision.name == "HyugASCIIBoss") //Faszom ebbe
            {
                wAnim.SetTrigger("Destroy");
            }
        }
        else //Other playable scenes
        {
            switch (gameObject.tag)
            {
                case "WPickUp": //Only destroy, if player touches
                    if (collision.CompareTag("Player"))
                    {
                        gManager.GetComponent<Manager>().instances.Remove(this.gameObject);
                        Destroy(gameObject, 0.01f);
                    }
                    break;
                case "Rasengan": //Rasengan is set to destroy upon impact with anything other than the Player
                    if (!collision.CompareTag("Player"))
                        wAnim.SetTrigger("Destroy");
                    break;
                case "Whirlwind": //Set Whirlwind later

                    break;
                default: //Every ninja weapon shall be destroyed, if it touches anything other than the wall
                    if (!collision.CompareTag("Wall") && !collision.CompareTag("Player") && !collision.CompareTag("WPickUp") && !collision.CompareTag("Clone"))
                    {
                        gManager.GetComponent<Manager>().instances.Remove(this.gameObject);
                        Destroy(gameObject, 0.01f);
                    }
                    break;
            }

            //if (gameObject.tag == "WPickUp" && collision.CompareTag("Player"))
            //    Destroy(gameObject, 0.01f);

            //if (gameObject.tag != "WindProjectile" && gameObject.tag != "Rasengan" && !collision.CompareTag("Wall") && !gameObject.tag.Contains("i"))
            //{
            //    //gManager.GetComponent<Manager>().sound_Effects[13].Play();
            //    //Instantiate(gManager.GetComponent<Manager>().particle_effects[2], transform.position, Quaternion.identity);
            //    Debug.Log("end" + gameObject.name);
            //    Destroy(gameObject, 0.01f);
            //}
            //else if (gameObject.tag == "Rasengan" && !collision.CompareTag("Player"))
            //{
            //    wAnim.SetTrigger("Destroy");
            //}
        }
    }
}