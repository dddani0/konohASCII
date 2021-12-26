using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YoroiBehav : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    [Header("Access the GameManager")]
    Scene scene;
    public string sceneName;
    public GameObject gameManager;
    [Header("DamageAttributes")]
    public float kunaiDamage;
    public float throwingStar;
    public float shurikenDamage;
    public float fumaShurikenDamage;
    [Header("Movement Attributes & Aggro Atribute")]
    Rigidbody2D kBody;
    Animator kAnim;
    public float speed;
    public float attackDistance;
    [Header("Player Attributes")]
    public GameObject player;
    public float pPos;

    #region Unity Functions
    void Start()
    {
        GetData();
    }

    void Update()
    {
        SetAttack();
        SetAnimation();
    }
#endregion
    #region Data
    public void GetData()
    {
        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        pPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform.position.x;
        kBody = GetComponent<Rigidbody2D>();
        kAnim = GetComponentInChildren<Animator>();
    }
    #endregion
    #region Attack
    public void SetAttack()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < attackDistance)
        {
            kAnim.SetTrigger("attack");
            transform.position = new Vector2(transform.position.x + ((-speed * 2) * Time.deltaTime), transform.position.y);
        }
    }
    #endregion
    #region Animation
    public void SetAnimation()
    {
        if (GetComponent<EnemyState>().health <= 0)
        {
            gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().score += 50f;
            gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().medallion++;
            Destroy(gameObject, 0.00001f); //xd what
            Instantiate(gameManager.GetComponent<Manager>().particle_effects[1], transform.position, Quaternion.identity);
        }
    }
    #endregion
    #region Collision Detection & Other
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rasengan"))
        {
            GetComponent<EnemyState>().health -= 600f; //Instakill haha
        }
        if ((collision.name == "Player" && gameObject.tag == "Yoroi") || (collision.name == "Clone(Clone)" && gameObject.tag == "Yoroi")) //Fuck this is hard. Shut yo mouth 
        {
            Destroy(gameObject);
        }
        if (sceneName == "UzumASCIIAdventureNormal")
        {
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
        }
        else if (sceneName == "UzumASCIIAdventureHard") //Handicap activates below this!
        {
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
        }
        else if (sceneName == "UzumASCIIAdventureVeryHard")
        {
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
        }
    }
    #endregion
}
