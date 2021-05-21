using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class JiroboBehaviour : MonoBehaviour
{
///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    [Header("Scene name & Access GameManager & Access Player & Target Attributes")]
    public GameObject gameManager; 
    [Space]
    public GameObject target; //Will attack the target, which can switch between the player and the clone
    public GameObject player;
    public GameObject clone;
    [Space]
    public BoxCollider2D[] targetDet;
    [Space]
    public bool targetLocked;
    Scene scene;
    public string sceneName;
    [Header("Damage Attributes")]
    public float kunaiDamage;
    public float shurikenDamage;
    public float fumaShurikenDamage;
    public float throwingStar;
    [Header("Movement ability & Aggro / Attack distance & Animation Properties")]
    Rigidbody2D jBody; //May not need, but for collision (?)
    Animator jAnim;
    public float movementSpeed;
    [Space]
    public float aggroDistance;
    public float attackDistance;
    [Header("Attack")]
    public Transform punchPoint;
    [Space]
    [Range(0,10)]public byte punchRad;
    [Space]
    public LayerMask playerLayer;
    public LayerMask cloneLayer; 
    [Space]
    public float damageDealt;
    [Header("Boxcollider2D on head")] //Disable on Death //Fuck you
    public GameObject boxcoll2D;
    public BoxCollider2D detDis; //Disable, don't slow down the clone

    #region Unity's functions
    void Start()
    {
        GetData();
        jAnim.SetFloat("health", GetComponent<EnemyState>().health); //Fuck you
    }

    void Update()
    {
        if (!gameManager.GetComponent<Manager>().pause)
        {
            JiroboBehav();
            AnimationJirobo();
            SetDistanceInTargetLock();
        }
    }
    #endregion
    #region Get Data
    public void GetData()
    {
        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        jBody = GetComponent<Rigidbody2D>();
        jAnim = GetComponentInChildren<Animator>();
        gameManager.GetComponent<Manager>().instances.Add(this.gameObject);
    }

    #endregion
    #region Jirobo's Behaviour
    public void JiroboBehav()
    {
        switch (targetLocked && !jAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
        {
            case true:
                switch (target != null)
                {
                    case true:
                        if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) < aggroDistance && Vector2.Distance(new Vector2(target.transform.position.x, target.transform.position.y), transform.position) > attackDistance)
                        {
                            jAnim.SetBool("motion", true);
                            jAnim.SetFloat("attack", 0f);
                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                        }
                        else if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) <= attackDistance)
                        {
                            jAnim.SetBool("motion", false);
                            jAnim.SetFloat("attack", 1f);
                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), 0);
                        }
                        break;
                    case false:
                        //targetLocked = false;
                        break;
                }
                break;
            case false:
                transform.position = Vector2.MoveTowards(transform.position, punchPoint.position, 0); //This doesn't actually move, so I use a random Transform variable, I created earlier
                jAnim.SetBool("motion", false);
                jAnim.SetFloat("attack", 0f);
                break;
        }
    }
    #endregion
    #region Attack Attribute (Call from Animation Event)
    public void AttackPunch()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(punchPoint.position, punchRad, playerLayer);
        Collider2D[] clonesToDamage = Physics2D.OverlapCircleAll(punchPoint.position, punchRad, cloneLayer);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            switch (sceneName != "SasukeTutorial")
            {
                case true:
                    enemiesToDamage[i].GetComponent<PlayerScriptUzumASCII>().TakeDamage(damageDealt);
                    break;
                case false:
                    enemiesToDamage[i].GetComponent<PlayerScriptUchihASCII>().TakeDamage(damageDealt);
                    break;
            }

        }
        for (int i = 0; i < clonesToDamage.Length; i++)
        {
            //clonesToDamage[i].GetComponent<CloneBehav>().TakeDamage(damageDealt);
        }
    }
    #endregion
    #region Animation and Misc
    public void AnimationJirobo()
    {
        jAnim.SetFloat("health", GetComponent<EnemyState>().health);
        switch (GetComponent<EnemyState>().health > 0) //Q: Is alive
        {
            case true: //A: sure, rotate towards target
                switch (target != null && targetLocked)
                {
                    case true:
                        switch (transform.position.x > target.transform.position.x)
                        {
                            case true:
                                transform.localScale = new Vector3(1, 1, 1);
                                break;
                            case false:
                                transform.localScale = new Vector3(-1, 1, 1);
                                break;
                        }
                        break;
                    case false:
                        return; //Returns nothing
                }
            break;
            case false: //A: No, Die
                jAnim.SetTrigger("Perish");
                Destroy(boxcoll2D);
                Destroy(detDis);
                Destroy(this.GetComponent<JiroboBehaviour>(),0.1f);
                break;
        }
    }
    public void SetDistanceInTargetLock() //Call in Late Update
    {
        switch (targetLocked && clone != null)
        {
            case true:
                switch (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), transform.position) < Vector2.Distance(new Vector2(clone.transform.position.x, clone.transform.position.y), transform.position))
                { //Player is closer to the Enemy
                    case true:
                        target = player;
                        break;
                    case false:
                        target = clone;
                        break;
                }
                break;
            case false:
                target = player;
                break;
        }
    }
    #endregion
    #region Collision Detection & Other
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!targetLocked && collision.name == "PlayerUchihASCII" ||!targetLocked && collision.tag == "Clone"  || !targetLocked && collision.tag == "Player")
        {
            player = GameObject.FindGameObjectWithTag("Player");
            clone = GameObject.FindGameObjectWithTag("Clone");
            targetDet[0].enabled = false;
            targetLocked = true;
        }
        switch (collision.gameObject.name) //Use
        {
            case "Kunai(Clone)":
                GetComponent<EnemyState>().health -= kunaiDamage;
                break;
            case "Shuriken(Clone)":
                GetComponent<EnemyState>().health -= shurikenDamage;
                break;
            case "FumaShuriken(Clone)":
                GetComponent<EnemyState>().health -= fumaShurikenDamage;
                break;
            case "ThrowingStar(Clone)":
                GetComponent<EnemyState>().health -= throwingStar;
                break;
            case "Rasengan(Clone)":
                GetComponent<EnemyState>().health -= (GetComponent<EnemyState>().health + 1);
                break;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPoint.position, punchRad);
    }
    #endregion
}