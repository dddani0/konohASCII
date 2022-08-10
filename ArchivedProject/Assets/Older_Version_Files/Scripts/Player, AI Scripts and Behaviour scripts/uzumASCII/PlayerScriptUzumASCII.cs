using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScriptUzumASCII : MonoBehaviour
{
///////////////////////////////
//REWORK! REWORK! REWORK! REWORK!
///////////////////////////////
///////////////////////////////
//IN PROGRESS! IN PROGRESS!
///////////////////////////////

    [Header("Access GameManager & Preference Selection & Set basic Delay")]
    [Range(0,1)]public float healingDelay;
    public GameObject gManager;
    public GameObject preferenceObject;
    [Header("Player Attributes:")]
    public float maximum_Health;
    public float current_Health;
    [Space]
    public float chakra;
    [Space]
    public float startTimeBtwChakraRecharge;
    float timeBtwChakraRecharge;
    [Space]
    public float punchDamage;
    [Space]
    [Range(0, 1)] public byte jutsuValue; //Chakra/jutsu Mode 
    [Space]
    public float knockbackForce;
    [Space]
    public float rasenganTechniqueRequirement;
    public float cloneTechniqueRequirement;
    public float cloneCount = 0;
    [Space]
    public float healing_Scroll;
    [Space]
    public float kunaiDamage;
    public float shurikenDamage;
    [Space]
    [Range(0, 10)] public float punchRad;
    [Header("Variables display attributes")]
    public Text scrollValueDisp;
    [Space]
    public Text healthValueDisp;
    [Space]
    public Text chakraValueDisp;
    [Space]
    public Image healthFill;
    [Header("Movement attributes")]
    [Range(0, 1)] float input;
    public float movementSpeed;
    [Space]
    public float jumpHeight;
    Rigidbody2D uBody;
    Animator uAnim;
    [Space]
    public bool isGround;
    public Transform groundCheck;
    [Range(0, 3)] public float groundCheckRad;
    [Space]
    public LayerMask groundLayer;
    [Header("Weapon Attributes")]
    public float maximum_Ninja_Weapon;
    public float current_Ninja_Weapon;
    [Space]
    public Text[] weaponProperties;
    public GameObject[] weaponUIDisplay;
    [Space]
    public Transform throwPosition;
    public Transform jutsuPosition; //Rasengan, only for Naruto
    public Transform clonePosition;
    [Space]
    public LayerMask enemyLayer;
    public LayerMask chestLayer;
    public LayerMask bossLayer;
    [Header("Damage attributes")]
    public ushort whirlWindDamage;
    public ushort kamikazeeDamage;
    public float trigramDamage;
    [Header("Only for Chuunin Exam: Final Round scene!")]
    public GameObject neji;

    private void Start()
    {
        GetData();
    }
    private void Update()
    {
        switch (!gManager.GetComponent<Manager>().pause)
        {
            case true:
                Jump();
                Punch();
                Throw_Selected_Ninja_Weapon();
                HealingScroll();
                RasenganTechnique();
                CloneTechnique();
                ChakraModeAndJutsuMode();
                break;
            case false:
                uBody.velocity = new Vector2(0, uBody.velocity.y);
                break;
        }
        Health_Manager();
        Chakra_Manager();
    }
    private void FixedUpdate()
    {
        switch (!gManager.GetComponent<Manager>().pause)
        {
            case true:
            GroundCheck();
            Movement();
                break;
            case false:
            uBody.velocity = new Vector2(0, uBody.velocity.y);
                break;
        }
    }
    private void LateUpdate()
    {
        
        if (!gManager.GetComponent<Manager>().pause)
        {
            MiscAndTextsAndDisplays();
            RotationAndAnimation();
        }
    }
    #region Get Data & Difficulty Handicaps
    public void GetData()
    {
        gManager = GameObject.FindGameObjectWithTag("GameManager");
        gManager.GetComponent<Manager>().instances.Add(this.gameObject);
        gManager.GetComponent<Manager>().jutsu_Scroll_UI.SetActive(false);
        if (gManager.GetComponent<Manager>().current_Loaded_Scene_Name != "NarutoTutorial")
        {
            preferenceObject = GameObject.FindGameObjectWithTag("PreferenceStorage");
        }
        uBody = GetComponent<Rigidbody2D>();
        uAnim = GetComponentInChildren<Animator>();
        current_Health = maximum_Health;
        timeBtwChakraRecharge = startTimeBtwChakraRecharge;
        MiscAndTextsAndDisplays();
        current_Ninja_Weapon = maximum_Ninja_Weapon;

        UI_Icon_Text_Setup();
        if (gManager.GetComponent<Manager>().current_Loaded_Scene_Name == "UzumASCIIBoss")
            neji = GameObject.FindGameObjectWithTag("Boss");
        //if (currentSceneName != "UzumASCIIBoss") //Ignores boss scene
        //{
        //    DifficultyHandicaps();
        //}
    }
    public void DifficultySettingsHandicapOptions()
    {
        //switch (currentSceneName)
        //{
        //    case "UzumASCIIAdventureVeryHard":
        //        healingDelay += (healingDelayHandicap / 3f); //Slows down healing more
        //        break;
        //}
    }
    #endregion
    #region Basic Movement Ability: Run & Jump
    public void Movement()
    {
        switch (uAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
        {
            case true:
                uBody.velocity = new Vector2(0, uBody.velocity.y);
                break;
            case false:
                input = Input.GetAxisRaw("Horizontal");
                if (!uAnim.GetCurrentAnimatorStateInfo(0).IsName("Jutsu_Mode")) //Can't move in Jutsu mode
                {
                    uBody.velocity = new Vector2(input * movementSpeed * Time.fixedDeltaTime, uBody.velocity.y);
                }
                else //Don't slide, if -> Jutsu mode when in motion
                {
                    uBody.velocity = new Vector2(0, uBody.velocity.y);
                }
                break;
        }
    }
    public void GroundCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRad, groundLayer);
    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && !uAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
        {
            uAnim.SetTrigger("jump");
            uBody.velocity = new Vector2(uBody.velocity.x, jumpHeight);
        }
    }
    #endregion
    #region Attacks: Jutsu Mode/Chakra Mode & Throwing Weapons & Jutsus & Punching & Healing 
    public void ChakraModeAndJutsuMode()
    {
        //gManager.GetComponent<Manager>().jutsu_Scroll_UI.GetComponent<Animator>().SetFloat("scrollValue", jutsuValue);
        if (Input.GetKeyDown(KeyCode.F) && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Jutsu_Mode") && uBody.velocity.y == 0) //Enter Jutsu Mode
        {
            jutsuValue = 1;
            gManager.GetComponent<Manager>().jutsu_Scroll_UI.SetActive(true);
            //Instantiate(gManager.GetComponent<Manager>().particle_effects[0], transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.F) && uAnim.GetCurrentAnimatorStateInfo(0).IsName("Jutsu_Mode")) //Enter Chakra Mode
        {
            jutsuValue = 0;
            gManager.GetComponent<Manager>().jutsu_Scroll_UI.SetActive(false);
        }
    }
    public void HealingScroll()
    {
        scrollValueDisp.text = healing_Scroll.ToString();
        if (Input.GetKeyDown(KeyCode.E) && healing_Scroll > 0 && current_Health < maximum_Health && uAnim.GetCurrentAnimatorStateInfo(0).IsTag("Action_Free")) //pro scripting xd
        {
            healing_Scroll--;
            uAnim.SetTrigger("heal");
            current_Health = maximum_Health;
        }
        //if (jutsuValue == 0 && current_Chakra < maximum_Chakra)
        //{
        //    current_Chakra++;
        //}
    }
    public void Punch()
    {
        if (Input.GetMouseButtonDown(0) && !uAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion") && !uAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jutsu") && uBody.velocity.y == 0)
        {
            //gManager.GetComponent<Manager>().sound_Effects[28].Play();
            uAnim.SetTrigger("punch");
            Collider2D[] chestToOpen = Physics2D.OverlapCircleAll(throwPosition.position, punchRad, chestLayer);
            Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(throwPosition.position, punchRad, enemyLayer);
            Collider2D[] bossToHit = Physics2D.OverlapCircleAll(throwPosition.position, punchRad, bossLayer);
            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                enemiesToHit[i].GetComponent<EnemyState>().TakeDamage(punchDamage);
            }
            for (int i = 0; i < chestToOpen.Length; i++)
            {
                chestToOpen[i].GetComponent<ChestBehaviour>().OpenChestWithBruteForce(punchDamage);
            }
            for (int i = 0; i < bossToHit.Length; i++)
            {
                bossToHit[i].GetComponent<HyugASCIIBossScript>().TakeDamage(punchDamage);
            }
        }
    }
    public void RasenganTechnique()
    {
        if (jutsuValue == 1 && Input.GetKeyDown(KeyCode.Alpha1) && !uAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion") && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall")) //Don't work in the air!
        {
            uAnim.SetTrigger("rasengan");
            if (gManager.GetComponent<Manager>().current_Loaded_Scene_Name != "NarutoTutorial")
            {
                preferenceObject.GetComponent<PreferenceStorage>().chakraConsumed += rasenganTechniqueRequirement;
            }
            Instantiate(gManager.GetComponent<Manager>().weapons[4], jutsuPosition.position, transform.rotation);
            switch (chakra > 0)
            {
                case true:
                    chakra -= rasenganTechniqueRequirement;
                    break;
                case false:
                    current_Health -= 500;
                    break;
            }
        }
        if (jutsuValue == 0 && Input.GetKeyDown(KeyCode.Alpha1) && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Jutsu_Mode") && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
        {
            jutsuValue = 1;
            gManager.GetComponent<Manager>().jutsu_Scroll_UI.SetActive(true);
            uAnim.SetTrigger("rasengan");
            if (gManager.GetComponent<Manager>().current_Loaded_Scene_Name != "NarutoTutorial")
            {
                preferenceObject.GetComponent<PreferenceStorage>().chakraConsumed += rasenganTechniqueRequirement;
            }
            Instantiate(gManager.GetComponent<Manager>().weapons[4], jutsuPosition.position, transform.rotation);
            switch (chakra > 0)
            {
                case true:
                    chakra -= rasenganTechniqueRequirement;
                    break;
                case false:
                    current_Health -= 500;
                    break;
            }
        }
    }
    public void CloneTechnique()
    {
        if (jutsuValue == 1 && cloneCount < 1 && Input.GetKeyDown(KeyCode.Alpha2) && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Cast_A_Clone") && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
        {
            switch (chakra > 0)
            {
                case true:
                    chakra -= cloneTechniqueRequirement;
                    break;
                case false:
                    current_Health -= 500;
                    break;
            }
            uAnim.SetTrigger("Clone");
            if (gManager.GetComponent<Manager>().current_Loaded_Scene_Name != "NarutoTutorial")
            {
                Instantiate(gManager.GetComponent<Manager>().weapons[6], new Vector2(clonePosition.position.x, transform.position.y), Quaternion.identity);
                preferenceObject.GetComponent<PreferenceStorage>().chakraConsumed += cloneTechniqueRequirement;
            }
            else
            {
                Instantiate(gManager.GetComponent<Manager>().weapons[6], new Vector2(clonePosition.position.x, transform.position.y), Quaternion.identity);
            }
            cloneCount++;

        }
        if (jutsuValue == 0 && cloneCount < 1 && Input.GetKeyDown(KeyCode.Alpha2) && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !uAnim.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
        {
            switch (chakra > 0)
            {
                case true:
                    chakra -= cloneTechniqueRequirement;
                    break;
                case false:
                    current_Health -= 500;
                    break;
            }
            jutsuValue = 1;
            gManager.GetComponent<Manager>().jutsu_Scroll_UI.SetActive(true);
            uAnim.SetTrigger("Clone");
            if (gManager.GetComponent<Manager>().current_Loaded_Scene_Name != "NarutoTutorial")
            {
                Instantiate(gManager.GetComponent<Manager>().weapons[6], new Vector2(clonePosition.position.x, transform.position.y), Quaternion.identity);
                preferenceObject.GetComponent<PreferenceStorage>().chakraConsumed += cloneTechniqueRequirement;
            }
            else
            {
                Instantiate(gManager.GetComponent<Manager>().weapons[6], new Vector2(clonePosition.position.x, transform.position.y), Quaternion.identity);
            }
            cloneCount++;
        }
    }
    public void Throw_Selected_Ninja_Weapon()
    {
        if (Input.GetMouseButtonDown(1) && current_Ninja_Weapon > 0 && uAnim.GetCurrentAnimatorStateInfo(0).IsTag("Action_Free"))
        {
            current_Ninja_Weapon--;
            uAnim.SetTrigger("throw");
            switch (gManager.GetComponent<Manager>().current_Loaded_Scene_Name != "NarutoTutorial")
            {
                case true:
                    Instantiate(gManager.GetComponent<Manager>().weapons[preferenceObject.GetComponent<PreferenceStorage>().weapon], throwPosition.position, transform.rotation);
                    //switch (preferenceObject.GetComponent<PreferenceStorage>().weapon) //Set weapon preferences
                    //{
                    //    case 0:
                    //        gManager.GetComponent<Manager>().sound_Effects[6].Play();
                    //        break;
                    //    case 1:
                    //        gManager.GetComponent<Manager>().sound_Effects[7].Play();
                    //        break;
                    //    case 2:
                    //        gManager.GetComponent<Manager>().sound_Effects[8].Play();
                    //        break;
                    //    case 3:
                    //        gManager.GetComponent<Manager>().sound_Effects[7].Play();
                    //        break;
                    //}
                    break;
                case false: //Tutorial
                    Instantiate(gManager.GetComponent<Manager>().weapons[0], throwPosition.position, transform.rotation);
                    //gManager.GetComponent<Manager>().sound_Effects[6].Play();
                    break;
            }
        }
    }
    #endregion
    #region Health, Pick up and Damage properties
    public void Health_Manager()
    {
        healthValueDisp.text = current_Health.ToString();
        healthFill.fillAmount = current_Health / maximum_Health;
    }
    public void Chakra_Manager()
    {
        //chakraFill.fillAmount = current_Chakra / maximum_Chakra;
        //chakraValueDisp[0].text = Mathf.Round(current_Chakra).ToString();
        chakraValueDisp.text = chakra.ToString();
        if (jutsuValue < 1 && chakra < 3)
        {
            if (timeBtwChakraRecharge <= 0)
            {
                chakra++;
                timeBtwChakraRecharge = startTimeBtwChakraRecharge;
            }
            else
            {
                timeBtwChakraRecharge -= Time.deltaTime;
            }
        }
    }
    public void UI_Icon_Text_Setup()
    {
        switch (gManager.GetComponent<Manager>().current_Loaded_Scene_Name == "NarutoTutorial")
        {
            case true:
                for (int i = 0; i < weaponUIDisplay.Length; i++)
                {
                    weaponUIDisplay[i].SetActive(false);
                }
                weaponUIDisplay[0].SetActive(true);
                break;
            case false:
                for (int i = 0; i < weaponUIDisplay.Length; i++)
                {
                    weaponUIDisplay[i].SetActive(false);
                }
                weaponUIDisplay[preferenceObject.GetComponent<PreferenceStorage>().weapon].SetActive(true);
                break;
        }
    }
    public void MiscAndTextsAndDisplays() //Shit void name bruh
    {
        switch (gManager.GetComponent<Manager>().current_Loaded_Scene_Name == "NarutoTutorial")
        {
            case true:
                maximum_Ninja_Weapon = 99;
                weaponProperties[0].text = current_Ninja_Weapon.ToString();
                weaponProperties[1].text = maximum_Ninja_Weapon.ToString();
                break;
            case false:
                switch (preferenceObject.GetComponent<PreferenceStorage>().weapon)
                {
                    case 0:
                        maximum_Ninja_Weapon = 8;
                        break;
                    case 1:
                        maximum_Ninja_Weapon = 12;
                        break;
                    case 2:
                        maximum_Ninja_Weapon = 4;
                        break;
                    case 3:
                        maximum_Ninja_Weapon = 12;
                        break;
                }
                weaponProperties[0].text = current_Ninja_Weapon.ToString();
                weaponProperties[1].text = maximum_Ninja_Weapon.ToString();
                break;
        }
    }
    public void RotationAndAnimation() //Rotates player on the RIGHT AXIS DANIEL ; Shut the fuck up
    {
        if (gManager.GetComponent<Manager>().current_Loaded_Scene_Name == "UzumASCIIBoss" && current_Health <= 0)
        {
            preferenceObject.GetComponent<PreferenceStorage>().bossValues = 4;
            preferenceObject.GetComponent<PreferenceStorage>().timer = gManager.GetComponent<Manager>().timer;
        }
        else if (gManager.GetComponent<Manager>().current_Loaded_Scene_Name == "NarutoTutorial" && current_Health <= 0)
        {
            //gManager.GetComponent<Manager>().StartCoroutine(gManager.GetComponent<Manager>().ResetCurrentScene()); Reset current scene
        }
        if (isGround && uAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fall")) //so, the player landed, and current animation tag is "Fall"
        {
            uAnim.SetTrigger("landed");
        }
        uAnim.SetFloat("jutsuValue", jutsuValue);
        uAnim.SetFloat("fallValue", uBody.velocity.y);
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
            uAnim.SetBool("idle", false);
        }
        else if (input == 0)
        {
            uAnim.SetBool("idle", true);
        }

    }
    public void TakeDamage(float damage)
    {
        if (gManager.GetComponent<Manager>().current_Loaded_Scene_Name != "UzumASCIIAdventureNormalDifficulty") //Stagger on harder difficulty
        {
            uAnim.SetTrigger("stagger");
        }
        //gManager.GetComponent<Manager>().sound_Effects[20].Play();
        //gManager.GetComponent<Manager>().sound_Effects[14].Play();
        current_Health -= damage;
    }
    private void OnTriggerEnter2D(Collider2D collision) //Collision never sleep
    {
        switch (gManager.GetComponent<Manager>().current_Loaded_Scene_Name != "NarutoTutorial")
        {
            case true:
                switch (gManager.GetComponent<Manager>().current_Loaded_Scene_Name == "UzumASCIIBoss")
                {
                    case true:
                        if (collision.CompareTag("Boss"))
                        {
                            neji.GetComponent<HyugASCIIBossScript>().GetComponentInChildren<HyugASCIIBossAnimation>().hAnim.SetTrigger("ComboTrigger");
                        }
                        else if (collision.CompareTag("trigram"))
                        {
                            current_Health -= trigramDamage;
                            uAnim.SetTrigger("stagger");
                            //gManager.GetComponent<Manager>().sound_Effects[14].Play();
                            if (transform.position.x < neji.transform.position.x && transform.localEulerAngles.y == 0 || transform.position.x > neji.transform.position.x && transform.localEulerAngles.y == 180)
                            {
                                uBody.AddForce(-transform.right * knockbackForce);
                            }
                            else if (transform.position.x < neji.transform.position.x && transform.localEulerAngles.y == 180 || transform.position.x > neji.transform.position.x && transform.localEulerAngles.y == 0)
                            {
                                uBody.AddForce(transform.right * knockbackForce);
                            }
                        }
                        else if (collision.CompareTag("Kunai"))
                        {
                            current_Health -= kunaiDamage;
                            //gManager.GetComponent<Manager>().sound_Effects[14].Play();
                        }
                        else if (collision.CompareTag("WPickUp") && (collision.name == gManager.GetComponent<Manager>().weapons[preferenceObject.GetComponent<PreferenceStorage>().weapon].name + "(Clone)")) //Pick up => WeaponCount++
                        {
                            current_Ninja_Weapon++;
                        }
                        else if (collision.CompareTag("WindProjectile"))
                        {
                            uAnim.SetTrigger("stagger");
                            current_Health -= whirlWindDamage;
                            //gManager.GetComponent<Manager>().sound_Effects[14].Play();
                        }
                        else if (collision.CompareTag("Yoroi"))
                        {
                            uAnim.SetTrigger("stagger");
                            current_Health -= kamikazeeDamage;
                            //gManager.GetComponent<Manager>().sound_Effects[14].Play();
                        }
                        break;
                    case false:
                        if (collision.CompareTag("Kunai"))
                        {
                            current_Health -= kunaiDamage;
                            //gManager.GetComponent<Manager>().sound_Effects[14].Play();
                        }
                        else if (collision.CompareTag("WPickUp") && (collision.name.Contains(gManager.GetComponent<Manager>().weapons[preferenceObject.GetComponent<PreferenceStorage>().weapon].name)))
                        {
                            current_Ninja_Weapon++;
                        }
                        else if (collision.CompareTag("WindProjectile"))
                        {
                            uAnim.SetTrigger("stagger");
                            current_Health -= whirlWindDamage;
                            //gManager.GetComponent<Manager>().sound_Effects[14].Play();
                        }
                        else if (collision.CompareTag("Yoroi"))
                        {
                            uAnim.SetTrigger("stagger");
                            current_Health -= kamikazeeDamage;
                            //gManager.GetComponent<Manager>().sound_Effects[14].Play();
                        }
                        return;
                }
                break;
            case false:
                if (collision.CompareTag("WPickUp"))
                {
                    current_Ninja_Weapon++;
                }
                else if (collision.CompareTag("WindProjectile"))
                {
                    if (!uAnim.GetCurrentAnimatorStateInfo(0).IsName("Jutsu_Mode"))
                    {
                        uAnim.SetTrigger("stagger");
                    }
                    current_Health -= whirlWindDamage;
                   //gManager.GetComponent<Manager>().sound_Effects[14].Play();
                }
                break;
        }
    }
    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRad);
        Gizmos.DrawWireSphere(throwPosition.position, punchRad);
    }
}