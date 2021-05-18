using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UzumASCIIBossBehav : MonoBehaviour
{
    [Header("Access the Gamemanager")]
    public GameObject gamemanager;
    [Header("Health Attributes & Movement Attributes & Access the Player & Attack Attributes ")]
    public Image healthBar;
    [Space]
    public float maxHealth;
    float currentHealth;
    [Space]
    public float movementSpeed;
    [Space]
    public float minMovDist; //   player  mxmd nmy mnmd
    [Space]
    public GameObject player;
    [Space]
    public Transform throwTransform;
    [Space]
    public float timeBtwThrows;
    public float startTimeBtwThrows;
    [Space]
    public float startTimeBtwSpecAttacks;
    float timeBtwSpecAttacks;
    [Space]
    public Transform punchPos;
    public LayerMask playerLayer;
    public float punchRad;
    public float punchDmg;
    Animator uAnim;
    [Space]
    public float cloneCount;
    [Space]
    public Transform cloneSpot;
    [Space]
    public float weaponRainCount = 10;
    [Space]
    public Transform rasenganSpot;
    [Header("Damage attributes")]
    public float kunaiDmg;
    public float shurikenDmg;
    public float fumaShurikenDmg;
    public float throwingStarDmg;
    [Header("Prepare for Second Phase (Only Clone needs this, or as I like to call it: Poor scripting")]
    public GameObject boss;

    #region Basic Unity Functions
    void Start()
    {
        GetData();
    }

    void Update()
    {
        BossBehaviour();
        HealthSetup();
        AnimationWork();
    }
    #endregion
    #region Get Data
    public void GetData()
    {
        gamemanager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        uAnim = GetComponentInChildren<Animator>();
    }
    #endregion
    #region Boss Behav
    public void BossBehaviour()
    {
        switch (gameObject.tag)
        {
            //Boss Behav -> Second Phase
            case "Boss":
                switch (currentHealth > 0 && !uAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
                {
                    case true:
                        if (player.GetComponent<PlayerScriptUchihASCII>().healthCur > 0)
                        {
                            switch (transform.position.x > player.transform.position.x)
                            {
                                case true:
                                    rasenganSpot.localEulerAngles = new Vector3(0, 180, 0);
                                    throwTransform.localEulerAngles = new Vector3(0, 180, 0);
                                    transform.localScale = new Vector3(-1, 1, 1);
                                    break;
                                case false:
                                    rasenganSpot.localEulerAngles = new Vector3(0, 0, 0);
                                    throwTransform.localEulerAngles = new Vector3(0, 0, 0);
                                    transform.localScale = new Vector3(1, 1, 1);
                                    break;
                            }
                            switch (Vector2.Distance(transform.position, player.transform.position) > minMovDist)
                            {
                                case true:
                                    uAnim.SetFloat("attack", 0f);
                                    uAnim.SetFloat("motion", 1f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                                    if (timeBtwThrows <= 0)
                                    {
                                        float rng = Random.Range(0, 3); //Local var
                                        switch (rng)
                                        {
                                            case 0:
                                                uAnim.SetTrigger("throw");
                                                Instantiate(gamemanager.GetComponent<Manager>().weapons[0], throwTransform.position, throwTransform.rotation);
                                                break;
                                            case 1:
                                                uAnim.SetTrigger("castRasengan");
                                                break;
                                            case 2:
                                                if (cloneCount < 1)
                                                {
                                                    uAnim.SetTrigger("castAClone");
                                                    cloneCount++;
                                                }
                                                break;
                                        }
                                        timeBtwThrows = startTimeBtwThrows;
                                    }
                                    else
                                    {
                                        timeBtwThrows -= Time.deltaTime;
                                    }
                                    break;
                                case false:
                                    if (Vector2.Distance(transform.position, player.transform.position) < minMovDist)
                                    {
                                        uAnim.SetFloat("attack", 1f);

                                    }
                                    //uAnim.SetFloat("attack", 0f);
                                    uAnim.SetFloat("motion", 0f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0);
                                    if (timeBtwSpecAttacks <= 0)
                                    {
                                        float sRng = Random.Range(0, 3);
                                        switch (sRng)
                                        {
                                            case 0:
                                                Rasengan();
                                                break;
                                            case 1:
                                                SetOffTrap();
                                                break;
                                            case 2:
                                                if (cloneCount < 1)
                                                {
                                                    CastAClone();
                                                }
                                                break;
                                        }
                                        timeBtwSpecAttacks = startTimeBtwSpecAttacks;
                                    }
                                    else
                                    {
                                        timeBtwSpecAttacks -= Time.deltaTime;
                                    }
                                    break;
                            }
                        }
                        break;
                    case false:
                            //some code
                        break;
                }
                break;
                //Clone Behav -> First Phase
            case "BossClone":
                switch (currentHealth > 0 && !uAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion")) //
                {
                    case true:
                        if (player.GetComponent<PlayerScriptUchihASCII>().healthCur > 0)
                        {
                            if (transform.position.x > player.transform.position.x)
                            {
                                throwTransform.localEulerAngles = new Vector3(0, 180, 0);
                                transform.localScale = new Vector3(-1, 1, 1);
                            }
                            if (transform.position.x < player.transform.position.x)
                            {
                                throwTransform.localEulerAngles = new Vector3(0, 0, 0);
                                transform.localScale = new Vector3(1, 1, 1);
                            }
                            switch (transform.position.x > player.transform.position.x)
                            {
                                case true:
                                    rasenganSpot.localEulerAngles = new Vector3(0, 180, 0);
                                    throwTransform.localEulerAngles = new Vector3(0, 180, 0);
                                    transform.localScale = new Vector3(-1, 1, 1);
                                    break;
                                case false:
                                    rasenganSpot.localEulerAngles = new Vector3(0, 0, 0);
                                    throwTransform.localEulerAngles = new Vector3(0, 0, 0);
                                    transform.localScale = new Vector3(1, 1, 1);
                                    break;
                            }

                            switch (Vector2.Distance(transform.position, player.transform.position) > minMovDist)
                            {
                                case true:
                                    uAnim.SetFloat("attack", 0f);
                                    uAnim.SetFloat("motion", 1f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                                    if (timeBtwThrows <= 0)
                                    {
                                        uAnim.SetTrigger("throw");
                                        Instantiate(gamemanager.GetComponent<Manager>().weapons[0], throwTransform.position, throwTransform.rotation);
                                        timeBtwThrows = startTimeBtwThrows;
                                    }
                                    else
                                    {
                                        timeBtwThrows -= Time.deltaTime;
                                    }
                                    break;
                                case false:
                                    if (timeBtwThrows <= 0)
                                    {
                                        uAnim.SetFloat("attack", 0f);
                                        uAnim.SetTrigger("throw");
                                        Instantiate(gamemanager.GetComponent<Manager>().weapons[0], throwTransform.position, throwTransform.rotation);
                                        timeBtwThrows = startTimeBtwThrows;
                                    }
                                    else
                                    {
                                        timeBtwThrows -= Time.deltaTime;
                                    }
                                    if (Vector2.Distance(transform.position, player.transform.position) < (minMovDist - 3))
                                    {
                                        uAnim.SetFloat("attack", 1f);
                                    }
                                    uAnim.SetFloat("motion", 0f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0);
                                    break;
                            }
                        }
                        break;
                    case false:
                        //Die and spawn Clone
                        break;
                }
                break;
        }
    }
    #endregion
    IEnumerator DelayAttack()
    {
        uAnim.SetFloat("attack", 1f);
        yield return new WaitForSeconds(0.5f);
        uAnim.SetFloat("attack", 0f);
    }
    #region Attacks
    public void PunchDamage() //Call in "UzumASCIIBossAnimation.cs"
    {
        Collider2D[] playerToDmg = Physics2D.OverlapCircleAll(punchPos.position, punchRad, playerLayer);
        for (int i = 0; i < playerToDmg.Length; i++)
        {
            playerToDmg[i].GetComponent<PlayerScriptUchihASCII>().TakeDamage(punchDmg);
        }
    }

    public void SetOffTrap()
    {
        uAnim.SetTrigger("SAFabove");
    }
    public void CastAClone()
    {
        uAnim.SetTrigger("castAClone");
        cloneCount++;
    }
    public void Rasengan()
    {
        uAnim.SetTrigger("castRasengan");
    }
    #endregion
    #region Health Setup
    public void HealthSetup()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
    #endregion
    #region CallFromChildExceptionShit
    public void DestCleanUp()
    {
        healthBar.enabled = false;
        Destroy(gameObject);
    }
    #endregion
    #region Animation Setup
    public void AnimationWork()
    {
        if (currentHealth <= 0)
        {
            gamemanager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().timer = gamemanager.GetComponent<Manager>().timer;
        }
        switch (gameObject.name)
        {
            case "UzumASCIIBossClone":
                uAnim.SetFloat("health", currentHealth);
                uAnim.SetFloat("healthNotClone", 1); //Set to 1, don't die, thanks babe
                break;
            case "UzumASCIIBoss":
                uAnim.SetFloat("healthNotClone", currentHealth); //Nice var names faggot!
                uAnim.SetFloat("health", 1); //Same
                break;
        }
    }
    #endregion
    #region Take Damage & Other Collision
    public void TakeDamage(float dmg) //The metric system, is indeed rational
    {
        currentHealth -= dmg;
        float rng = Random.Range(0,101);
        if (rng < 15f && weaponRainCount % 2 == 0 && weaponRainCount > 0 && gameObject.name != "UzumASCIIBossClone")
        {
            weaponRainCount--;
            uAnim.SetTrigger("SAFabove");
        }
    }
    public void TakeDamageFromSpecialAb(float damage)
    {
        currentHealth -= damage;
        uAnim.SetTrigger("spStagger");

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("WPickUp") && !collision.CompareTag("WindProjectile") && collision.CompareTag("Kunai"))
        {
            currentHealth -= kunaiDmg; 
        }
        else if (!collision.CompareTag("WPickUp") && !collision.CompareTag("WindProjectile") && collision.CompareTag("Shuriken")) {
            currentHealth -= shurikenDmg;
        }
        else if (!collision.CompareTag("WPickUp") && !collision.CompareTag("WindProjectile") && collision.CompareTag("FumaShuriken"))
        {
            currentHealth -= fumaShurikenDmg;
            uAnim.SetTrigger("Stagger");
        }else if (!collision.CompareTag("WPickUp") && !collision.CompareTag("WindProjectile") && collision.CompareTag("ThrowingStar"))
        {
            currentHealth -= throwingStarDmg;
        }
    }
    #endregion
    #region Gizmos and Other
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPos.position, punchRad);
    }
    #endregion
}