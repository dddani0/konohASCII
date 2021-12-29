using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScriptUchihASCII : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    [Header("Check current Scene & Access the GameManager & Preference Selection")]
    Scene cScene;
    public string cSceneName;
    [Space]
    public GameObject gameManager;
    [Space]
    public GameObject preferenceSelec;
    [Space]
    [Header("Player attributes & Movement Attributes & Attack Attributes")]
    Animator pAnim; //Doesnt show in inspector
    Rigidbody2D pBody;
    float input;
    public float healthMax;
    public float healthCur;
    [Space]
    public float chakraMax;
    float chakraCurrent;
    [Space]
    public float jutsuVal = 0; //For Jutsu Mode
    [Space]
    public Transform jPosition;
    [Space]
    public float speed;
    [Space]
    public float jumpHeight;
    public bool onGround;
    [Range(0, 7)] public byte groundCheckRadius;
    [Space]
    public Transform groundCheckT;
    public LayerMask gLayer;
    [Space]
    public Transform punchPosition;
    [Space]
    public Transform fireballPosition;
    [Space]
    public Transform chidoriPosition;
    [Space]
    public float punchRad;
    [Space]
    public float fireballRad;
    [Space]
    public float chidoriRad;
    [Space]
    public float fireballChakraReq;
    [Space]
    public float chidoriChakraReq;
    [Space]
    public LayerMask[] enemyLayer;
    [Space]
    sbyte ninWeaponsCountCurrent;
    public sbyte ninWeaponsCountMax;
    [Space]
    public Transform throwingPosition;
    [Space]
    public float punchDmg;
    [Header("Take Damage attributes")]
    public float kunaiDmg;
    public float shurikenDmg;
    public float fumaShurikenDmg;
    public float throwingStarDmg;
    public float RasenganDmg;
    [Header("Sharingan | Special Ability attributes")]
    public GameObject sharingan;
    [Space]
    public float sharinganTimeMaximum;
    public float sharinganTimeMinimum;
    [Space]
    public float healingDelay;
    [Space]
    public bool isCRunning;
    [Space]
    public float currentHealingScroll;
    public float maxHealingScroll;
    [Header("UI Attributes")]
    public Text[] weaponUIDispProperties; // 0 = current / 1 = max
    public GameObject[] weaponUIDisplay;
    [Space]
    public Text[] chakraValueDisplayOnUI;
    [Space]
    public Text[] healingScrollDispOnUI;
    [Space]
    public Image healingScrollDispFillOnUI;
    [Space]
    public Image chakraFilling;
    public Image healthFilling;
    [Space]
    public Image sharinganFill;

    #region Unity Functions
    void Start()
    {
        GetData();
        UIWeaponDisplay();
    }
    void Update()
    {
        HealthSetup();
        ChakraSetup(); //Remove Debug features
        GroundCheck();
        SharinganSetup();
        Movement();
        ThrowNinjaWeapons();
        Jump();
        UIWeaponDisplay();
        SharinganAbility();
        PunchAttack();
        ChakraAndJutsuMode();
        FireballTechnique();
        ChidoriTechnique();
        HealWithScroll();
    }
    private void LateUpdate()
    {
        AnimationAndRotation();
    }
    #endregion
    #region Get Data
    public void GetData()
    {
        Time.timeScale = 1.0f;
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        cScene = SceneManager.GetActiveScene();
        cSceneName = cScene.name;
        sharingan = GameObject.FindGameObjectWithTag("Sharingan");
        if (cSceneName != "SasukeTutorial")
        {
            preferenceSelec = GameObject.FindGameObjectWithTag("PreferenceStorage");
        }
        UIWeaponDisplay();
        pBody = GetComponent<Rigidbody2D>();
        pAnim = GetComponentInChildren<Animator>();
        sharinganTimeMinimum = sharinganTimeMaximum;
        chakraCurrent = chakraMax;
        healthCur = healthMax;
        ninWeaponsCountCurrent = ninWeaponsCountMax;
        currentHealingScroll = maxHealingScroll;
    }
    #endregion
    #region Basic Movement Ability & Jump 
    public void Movement()
    {
        switch (sharingan.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("CanActivate"))
        {
            case true:
                switch (pAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
                {
                    case true:
                        pBody.velocity = new Vector2(0, 0);
                        break;
                    case false:
                        input = Input.GetAxisRaw("Horizontal");
                        pBody.velocity = new Vector2(input * speed * Time.deltaTime, pBody.velocity.y);
                        break;
                }
                break;
            case false:
                switch (pAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
                {
                    case true:
                        pBody.velocity = new Vector2(0, 0);
                        break;
                    case false:
                        input = Input.GetAxisRaw("Horizontal");
                        pBody.velocity = new Vector2(input * ((speed / 2) * 0.25f) * Time.deltaTime * ((Time.timeScale / Time.deltaTime) * 1.5f), pBody.velocity.y); //What the fuck
                        break;
                }
                break;
        }
        #region bin?
        /*
    if (sharingan.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Default_State") || sharingan.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Exhauste_Deactivated"))
    {
        switch (pAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
        {
            case true:
                input = Input.GetAxisRaw("Horizontal");
                pBody.velocity = new Vector2(input * speed * Time.deltaTime, pBody.velocity.y);
                break;
            case false:
                pBody.velocity = new Vector2(0, 0);
                break;
        }
        if (!pAnim.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
        {
            input = Input.GetAxisRaw("Horizontal");
            pBody.velocity = new Vector2(input * speed * Time.deltaTime, pBody.velocity.y);
        }
        else
        {
            pBody.velocity = new Vector2(0, 0);
        }
    }
        else if (sharingan.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Activate") && Time.timeScale == sharingan.GetComponent<SharinganSystem>().timeSlider)
        {       
            if (!pAnim.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
            {
                input = Input.GetAxisRaw("Horizontal");
                pBody.velocity = new Vector2(input * (speed / 2) * Time.deltaTime * ((Time.timeScale /Time.deltaTime) * 1.5f) , pBody.velocity.y);
            }
            else
            {
                pBody.velocity = new Vector2(0, 0);
            }
        }*/
        #endregion
    }

    public void Jump()
    {
        switch (sharingan.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("CanActivate"))
        {
            case true:
                if (Input.GetKeyDown(KeyCode.Space) && onGround)
                {
                    pAnim.SetTrigger("jump");
                    pBody.velocity = new Vector2(pBody.velocity.x, jumpHeight);
                }
                break;
            case false:
                if (Input.GetKeyDown(KeyCode.Space) && onGround)
                {
                    pAnim.SetTrigger("jump");
                    pBody.velocity = new Vector2(pBody.velocity.x, jumpHeight * (speed * 0.25f) * Time.deltaTime * ((Time.timeScale / Time.deltaTime) * 1.5f));
                }
                break;
        }
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            pAnim.SetTrigger("jump");
            pBody.velocity = new Vector2(pBody.velocity.x, jumpHeight);
        }
    }
    public void GroundCheck()
    {
        onGround = Physics2D.OverlapCircle(groundCheckT.position, groundCheckRadius, gLayer);
    }
    #endregion
    #region Health & Chakra Setup
    public void HealthSetup()
    {
        healthFilling.fillAmount = healthCur / healthMax;
    }
    public void ChakraSetup()
    {
        chakraFilling.fillAmount = chakraCurrent / chakraMax;
    }
    #endregion
    #region Special Ability: Sharingan & Attack & Chakra Mode / Jutsu Mode & Two Jutsu Abilities & Heal Ability
    public void SharinganSetup()
    {
        sharinganFill.fillAmount = sharinganTimeMinimum / sharinganTimeMaximum;
    }
    public void SharinganAbility()
    {
        if (sharinganTimeMinimum >= sharinganTimeMaximum && Input.GetKeyDown(KeyCode.Q) && sharingan.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Default_State"))
        {
            sharingan.GetComponent<Animator>().SetTrigger("sharinganOn");
        }
        if (sharinganTimeMinimum < sharinganTimeMaximum && !sharingan.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Activate"))
        {
            sharinganTimeMinimum += 0.01f; 
            sharingan.GetComponent<Animator>().ResetTrigger("sharinganOff");
        }
        if (sharinganTimeMinimum < 0)
        {
            sharingan.GetComponent<Animator>().SetTrigger("sharinganOff");
        }
        if (sharingan.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Activate"))
        {
            sharinganTimeMinimum -= 0.03f;
        }
    }
    public void FireballTechnique()
    {
        if (jutsuVal > 0 && Input.GetKeyDown(KeyCode.Alpha1) && chakraCurrent > fireballChakraReq)
        {
            pAnim.SetTrigger("castFireball");
            chakraCurrent -= fireballChakraReq;
        }
        if (jutsuVal < 1 && Input.GetKeyDown(KeyCode.Alpha1) && chakraCurrent > fireballChakraReq && !pAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
        {
            pAnim.SetTrigger("castFireball");
            chakraCurrent -= fireballChakraReq;
            jutsuVal = 1;
        }
    }
    public void ChidoriTechnique()
    {
        if (jutsuVal > 0 && Input.GetKeyDown(KeyCode.Alpha2) && chakraCurrent > chidoriChakraReq)
        {
            pAnim.SetTrigger("castChidori");
            chakraCurrent -= chidoriChakraReq;
        }
        if (jutsuVal < 1 && Input.GetKeyDown(KeyCode.Alpha2) && chakraCurrent > chidoriChakraReq && !pAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
        {
            pAnim.SetTrigger("castChidori");
            chakraCurrent -= chidoriChakraReq;
            jutsuVal = 1;
        }

    }
    public void PunchAttack()
    {
        switch (cSceneName == "SasukeTutorial")
        {
            case true:
                if (!pAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion") && Input.GetMouseButtonDown(0))
                {
                    pAnim.SetTrigger("attack");
                    Collider2D[] enemyDmg = Physics2D.OverlapCircleAll(punchPosition.position, punchRad, enemyLayer[0]);
                    Collider2D[] cloneDmg = Physics2D.OverlapCircleAll(punchPosition.position, punchRad, enemyLayer[1]);
                    Collider2D[] enemyTutDmg = Physics2D.OverlapCircleAll(punchPosition.position, punchRad, enemyLayer[2]);
                    for (int i = 0; i < enemyDmg.Length; i++) //First Phase Boss's Doppelganger
                    {
                        enemyDmg[i].GetComponent<UzumASCIIBossBehav>().TakeDamage(punchDmg);
                    }
                    for (int i = 0; i < cloneDmg.Length; i++)
                    {
                        //cloneDmg[i].GetComponent<CloneBehav>().TakeDamage(punchDmg);
                    }
                    for (int i = 0; i < enemyTutDmg.Length; i++)
                    {
                        enemyTutDmg[i].GetComponent<EnemyState>().TakeDamage(punchDmg);
                    }
                }
                break;
            case false:
                if (!pAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion") && Input.GetMouseButtonDown(0))
                {
                    pAnim.SetTrigger("attack");
                    Collider2D[] enemyDmg = Physics2D.OverlapCircleAll(punchPosition.position, punchRad, enemyLayer[0]);
                    Collider2D[] cloneDmg = Physics2D.OverlapCircleAll(punchPosition.position, punchRad, enemyLayer[1]);
                    for (int i = 0; i < enemyDmg.Length; i++) //First Phase Boss's Doppelganger
                    {
                        enemyDmg[i].GetComponent<UzumASCIIBossBehav>().TakeDamage(punchDmg);
                    }
                    for (int i = 0; i < cloneDmg.Length; i++)
                    {
                        //cloneDmg[i].GetComponent<CloneBehav>().TakeDamage(punchDmg);
                    }
                }
                break;
        }
    }
    #region Call from child Attacks
    public void FireBallJutsuTrig()
    {
        switch (cSceneName == "SasukeTutorial")
        {
            case true:
                Collider2D[] enemyDmg = Physics2D.OverlapCircleAll(fireballPosition.position, punchRad, enemyLayer[0]);
                Collider2D[] cloneDmg = Physics2D.OverlapCircleAll(fireballPosition.position, punchRad, enemyLayer[1]);
                Collider2D[] enemyTutDmg = Physics2D.OverlapCircleAll(punchPosition.position, punchRad, enemyLayer[2]);
                for (int i = 0; i < enemyDmg.Length; i++) //First Phase Boss's Doppelganger
                {
                    enemyDmg[i].GetComponent<UzumASCIIBossBehav>().TakeDamageFromSpecialAb(punchDmg * 1.5f);
                }
                for (int i = 0; i < cloneDmg.Length; i++)
                {
                    //cloneDmg[i].GetComponent<CloneBehav>().TakeSpecDMG();
                }
                for (int i = 0; i < enemyTutDmg.Length; i++)
                {
                    enemyTutDmg[i].GetComponent<EnemyState>().TakeDamage(punchDmg * 1000);
                }
                break;
            case false:
                Collider2D[] enemy = Physics2D.OverlapCircleAll(fireballPosition.position, punchRad, enemyLayer[0]);
                Collider2D[] clone = Physics2D.OverlapCircleAll(fireballPosition.position, punchRad, enemyLayer[1]);
                for (int i = 0; i < enemy.Length; i++) //First Phase Boss's Doppelganger
                {
                    enemy[i].GetComponent<UzumASCIIBossBehav>().TakeDamageFromSpecialAb(punchDmg * 1.5f);
                }
                for (int i = 0; i < clone.Length; i++)
                {
                    //clone[i].GetComponent<CloneBehav>().TakeSpecDMG();
                }
                break;
        }
    }
    public void ChidoriJutsuTrig()
    {
        switch (cSceneName == "SasukeTutorial")
        {
            case true:
                Collider2D[] enemyDmg = Physics2D.OverlapCircleAll(fireballPosition.position, punchRad, enemyLayer[0]);
                Collider2D[] cloneDmg = Physics2D.OverlapCircleAll(fireballPosition.position, punchRad, enemyLayer[1]);
                Collider2D[] enemyTutDmg = Physics2D.OverlapCircleAll(punchPosition.position, punchRad, enemyLayer[2]); //nigga remember: DRY
                for (int i = 0; i < enemyDmg.Length; i++) //First Phase Boss's Doppelganger
                {
                    enemyDmg[i].GetComponent<UzumASCIIBossBehav>().TakeDamageFromSpecialAb(punchDmg * 2);
                }
                for (int i = 0; i < cloneDmg.Length; i++)
                {
                    //cloneDmg[i].GetComponent<CloneBehav>().TakeSpecDMG();
                }
                for (int i = 0; i < enemyTutDmg.Length; i++)
                {
                    enemyTutDmg[i].GetComponent<EnemyState>().TakeDamage(punchDmg * 1000);
                }
                break;
            case false:
                Collider2D[] enemyD = Physics2D.OverlapCircleAll(fireballPosition.position, punchRad, enemyLayer[0]); //Fuck this
                Collider2D[] cloneD = Physics2D.OverlapCircleAll(fireballPosition.position, punchRad, enemyLayer[1]);
                for (int i = 0; i < enemyD.Length; i++) //First Phase Boss's Doppelganger
                {
                    enemyD[i].GetComponent<UzumASCIIBossBehav>().TakeDamageFromSpecialAb(punchDmg * 2);
                }
                for (int i = 0; i < cloneD.Length; i++)
                {
                    //cloneD[i].GetComponent<CloneBehav>().TakeSpecDMG();
                }
                break;
        }

    }
    #endregion
    public void ThrowNinjaWeapons()
    {
        if (Input.GetMouseButtonDown(1) && !pAnim.GetCurrentAnimatorStateInfo(0).IsName("Throw") && pBody.velocity.y == 0 && ninWeaponsCountCurrent > 0)
        {
            pAnim.SetTrigger("throw");
            ninWeaponsCountCurrent--;
            if (cSceneName != "SasukeTutorial")
            {
                switch (preferenceSelec.GetComponent<PreferenceStorage>().weapon) //Set weapon preferences
                {
                    case 0:
                        gameManager.GetComponent<Manager>().sound_Effects[6].Play();
                        break;
                    case 1:
                        gameManager.GetComponent<Manager>().sound_Effects[7].Play();
                        break;
                    case 2:
                        gameManager.GetComponent<Manager>().sound_Effects[8].Play();
                        break;
                    case 3:
                        gameManager.GetComponent<Manager>().sound_Effects[7].Play();
                        break;
                }
                Instantiate(gameManager.GetComponent<Manager>().weapons[preferenceSelec.GetComponent<PreferenceStorage>().weapon], throwingPosition.position, transform.rotation);
            }
            else
            {
                gameManager.GetComponent<Manager>().sound_Effects[6].Play();
                Instantiate(gameManager.GetComponent<Manager>().weapons[0], throwingPosition.position, transform.rotation);
            }
        }
    }
    public void ChakraAndJutsuMode()
    {
        pAnim.SetFloat("healthVal", healthCur);
        pAnim.SetFloat("jutsuVal", jutsuVal);
        if (Input.GetKeyDown(KeyCode.F) && !pAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion") && jutsuVal < 1)
        {
            jutsuVal = 1f;
            Instantiate(gameManager.GetComponent<Manager>().particle_effects[0], transform.position, Quaternion.identity);
        }
        else if (Input.GetKeyDown(KeyCode.F) && jutsuVal > 0)
        {
            jutsuVal = 0f;
        }
    }
    public void HealWithScroll()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentHealingScroll > 0 & healthCur < healthMax & !pAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion") && !isCRunning)
        {
            currentHealingScroll--;
            pAnim.SetTrigger("heal");
            StartCoroutine(HealWithDelay());
        }
        if (chakraCurrent < chakraMax & jutsuVal < 1) //Well, all of this scripts are shitty. nice
        {
            chakraCurrent += 0.1f;
        }

        IEnumerator HealWithDelay()
        {
            isCRunning = true;
            for (float i = healthCur; i < healthMax; i++) //Ladies and Gentleman, may I present to you, the for loop with float type index var!
            {
                healthCur++;
                yield return new WaitForSeconds(healingDelay);
            }
            isCRunning = false;
        }
    }
    #endregion
    #region Animation, Turning and Misc
    public void AnimationAndRotation()
    {
        gameManager.GetComponent<Manager>().jutsu_Scroll_UI.GetComponent<Animator>().SetFloat("scrollValue", jutsuVal);
        if (cSceneName != "SasukeTutorial" && healthCur <= 0)
        {
            preferenceSelec.GetComponent<PreferenceStorage>().bossValues = 3;
            preferenceSelec.GetComponent<PreferenceStorage>().timer = gameManager.GetComponent<Manager>().timer;
            StartCoroutine(DelayDeathForAnimation());
            //yuganda sekai ni dan dan boku wa sukitootte mienaku nattemitsukenaide boku no koto wo mitsumenaide 😭😭
        }else if (healthCur <= 0 && cSceneName == "SasukeTutorial")
        {
            gameManager.GetComponent<Manager>().ResetScene();
        }
        pAnim.SetFloat("jumpValue", pBody.velocity.y);
        if (onGround && pAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fall"))
        {
            pAnim.SetTrigger("landed");
        }
        if (input < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (input > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (input != 0)
        {
            pAnim.SetBool("motion", true);
        }
        else if (input == 0)
        {
            pAnim.SetBool("motion", false);
        }
    }

    IEnumerator DelayDeathForAnimation()
    {
        gameManager.GetComponent<Manager>().transition.GetComponent<TransitionBehaviour>().TransitionIn();
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(6);
    }
    #endregion
    #region WeaponUIDisplayFunctions
    public void UIWeaponDisplay()
    {
        switch (cSceneName != "SasukeTutorial")
        {
            case true:
                if (preferenceSelec.GetComponent<PreferenceStorage>().weapon == 0)
                {
                    ninWeaponsCountMax = 8;
                }
                if (preferenceSelec.GetComponent<PreferenceStorage>().weapon == 1)
                {
                    ninWeaponsCountMax = 12;
                }
                if (preferenceSelec.GetComponent<PreferenceStorage>().weapon == 2)
                {
                    ninWeaponsCountMax = 4;
                }
                if (preferenceSelec.GetComponent<PreferenceStorage>().weapon == 3)
                {
                    ninWeaponsCountMax = 12;
                }
                weaponUIDispProperties[0].text = ninWeaponsCountCurrent.ToString();
                weaponUIDispProperties[1].text = ninWeaponsCountMax.ToString();
                chakraValueDisplayOnUI[0].text = Mathf.Round(Mathf.Abs(chakraCurrent)).ToString();
                chakraValueDisplayOnUI[1].text = chakraMax.ToString();
                healingScrollDispOnUI[0].text = currentHealingScroll.ToString();
                healingScrollDispOnUI[1].text = maxHealingScroll.ToString();
                healingScrollDispFillOnUI.fillAmount = currentHealingScroll / maxHealingScroll;
                for (int i = 0; i < weaponUIDisplay.Length; i++)
                {
                    weaponUIDisplay[i].SetActive(false);
                }
                weaponUIDisplay[preferenceSelec.GetComponent<PreferenceStorage>().weapon].SetActive(true);
                break;
            case false:
                ninWeaponsCountMax = 99;
                weaponUIDispProperties[0].text = ninWeaponsCountCurrent.ToString();
                weaponUIDispProperties[1].text = ninWeaponsCountMax.ToString();
                chakraValueDisplayOnUI[0].text = Mathf.Round(Mathf.Abs(chakraCurrent)).ToString();
                chakraValueDisplayOnUI[1].text = chakraMax.ToString();
                healingScrollDispOnUI[0].text = currentHealingScroll.ToString();
                healingScrollDispOnUI[1].text = maxHealingScroll.ToString();
                healingScrollDispFillOnUI.fillAmount = currentHealingScroll / maxHealingScroll;
                for (int i = 0; i < weaponUIDisplay.Length; i++)
                {
                    weaponUIDisplay[i].SetActive(false);
                }
                weaponUIDisplay[0].SetActive(true);
                break;
        } 

    }
    #endregion
    #region Collision Detection & Other

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (cSceneName != "SasukeTutorial")
        {
            case true:
                if (collision.CompareTag("WPickUp") && (collision.name == gameManager.GetComponent<Manager>().weapons[preferenceSelec.GetComponent<PreferenceStorage>().weapon].name + "(Clone)")) //Pick up => WeaponCount++ //Oh, now you can only pick up the same weapons, check mate gamers
                {
                    ninWeaponsCountCurrent++;
                }
                if (collision.CompareTag("Kunai"))
                {
                    healthCur -= kunaiDmg;
                }
                else if (collision.CompareTag("Shuriken"))
                {
                    healthCur -= shurikenDmg;
                }
                else if (collision.CompareTag("FumaShuriken"))
                {
                    healthCur -= fumaShurikenDmg;
                }
                else if (collision.CompareTag("ThrowingStar"))
                {
                    healthCur -= throwingStarDmg;
                }
                if (collision.CompareTag("Rasengan"))
                {
                    healthCur -= RasenganDmg;
                    pAnim.SetTrigger("staggerRasen");
                    //pAnim.ResetTrigger("staggerRasen");
                }
                break;
            case false:
                if (collision.CompareTag("WPickUp") && collision.name == "Kunai(Clone)") //Pick up => WeaponCount++ //Oh, now you can only pick up the same weapons, check mate gamers
                {
                    ninWeaponsCountCurrent++;
                }
                else if (collision.CompareTag("Kunai"))
                {
                    healthCur -= kunaiDmg;
                }
                else if (collision.CompareTag("Shuriken"))
                {
                    healthCur -= shurikenDmg;
                }
                else if (collision.CompareTag("FumaShuriken"))
                {
                    healthCur -= fumaShurikenDmg;
                }
                else if (collision.CompareTag("ThrowingStar"))
                {
                    healthCur -= throwingStarDmg;
                }
                else if (collision.CompareTag("WindProjectile"))
                {
                    healthCur -= fumaShurikenDmg;
                    pAnim.SetTrigger("stagger");
                }
                break;
        }
    }
    public void TakeDamage(float dmg)
    {
        healthCur -= dmg;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckT.position, groundCheckRadius);
        Gizmos.DrawWireSphere(punchPosition.position, punchRad);
        Gizmos.DrawWireSphere(fireballPosition.position, fireballRad);
        Gizmos.DrawWireSphere(chidoriPosition.position, chidoriRad);
    }
    #endregion
}
