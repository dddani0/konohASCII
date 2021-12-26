using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Naruto : MonoBehaviour
{
///////////////////////////////
//UNNECESSARY? UNNECESSARY?
///////////////////////////////

    /*//Number/Logic Var
    [Header("NUMBER AND LOGIC VARIABLES")]
    public string sceneName;
    public float sfxStartTime;
    public float groundCheckRadius; //
    public float jumpForce; //
    public float cloneJutsuReq;
    public float rasenganJutsuReq;
    public float medicalNinjaScrolls;
    public float damageAmount;
    public float jutsuReq;
    public float attackRange;
    public float startTimeBtwThrow;
    public float startTimeBtwAttack;
    public float throwableCount;
    public float speed;
    public float maximumHealthPoints;
    public float chakraMax;
    public float startTimeBtwPunchesReceived;
    private float timeBtwThrow;
    private float timeBtwAttack;
    public float cloneCount = 0;
    public float currentHealthPoints;
    float timeBtwPunchesReceived;
    [SerializeField]float chakra;
    float JutsuValue = 1f;
    float timeBtwCharge = 5.0f;
    float input;
    [SerializeField]bool isGrounded;
    bool handSeal = false;
    bool isPunch = false;
    bool isAttack = false;
    Vector2 player;

    //GameObject Var
    [Space]
    [Header("GAMEOBJECT VARIABLES")]
    public ParticleSystem deathP;
    public Transform groundCheckPos;
    public Transform deathPo;
    public Transform[] cloneSpot;
    public Transform attackPoint;
    public LayerMask canCollideWithGround;
    public LayerMask enemiesToAttack;
    public LayerMask chestsToOpen;
    public GameObject jutsu;
    public Image healthFill;
    public Text mNSText;
    public Text chakraText;
    public Text kunaiText;
    public Transform throwablePos;
    public GameObject preferenceCheck;
    public GameObject weapon;
    public Rigidbody2D naruto;
    Scene curScene;
    GameObject beforeJutsuMode;
    GameObject jutsuUI;
    GameObject particleManager;
    GameObject gameManager;
    GameObject sfx; 
    ChestBehav chest;
    GameObject lootChest;
    Animator narutoAnim;

    #region Start and Update Methods
    void Start()
    {
        Data();
    }
    void Update()
    {
        VibeCheck();
        Movement();
        Text();
        Animation();
        FillSetup();
        GroundCheck();
        Throwables();
        JutsuAndChakraSwitch();
        ChakraJutsuDetect();
        Attack();
        Jump();
        Heal();
        ChakraManager();
        StopAttack();
    }
    #endregion
    #region Data
    public void Data()
    {
        curScene = SceneManager.GetActiveScene();
        sceneName = curScene.name;
        narutoAnim = GetComponent<Animator>();
        if (sceneName != "HowToPlay")
        {
            preferenceCheck = GameObject.FindGameObjectWithTag("PreferenceCheck");
            if (preferenceCheck.GetComponent<PreferenceStorage>().weaponID == 0)
            {
                throwableCount = 8;
            }
            else if (preferenceCheck.GetComponent<PreferenceStorage>().weaponID == 1)
            {
                throwableCount = 12;
            }
            else if (preferenceCheck.GetComponent<PreferenceStorage>().weaponID == 2)
            {
                throwableCount = 4;
            }
            else if (preferenceCheck.GetComponent<PreferenceStorage>().weaponID == 3)
            {
                throwableCount = 12;
            }
            preferenceCheck.GetComponent<PreferenceStorage>().weaponsThrown = 0;
            preferenceCheck.GetComponent<PreferenceStorage>().enemiesDefeated = 0;
        }
        if (sceneName == "HowToPlay")
        {
            throwableCount = 99;
        }
        if (sceneName == "NarutoStoryFight")
        {
            throwableCount = 8;
        }
        weapon = GameObject.FindGameObjectWithTag("Storage"); //prefab storage
        beforeJutsuMode = GameObject.FindGameObjectWithTag("smallJutsu");
        jutsuUI = GameObject.FindGameObjectWithTag("Jutsu");
        particleManager = GameObject.FindGameObjectWithTag("ParticleStorage");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        sfx = GameObject.FindGameObjectWithTag("SFXStorage");
        chest = GetComponent<ChestBehav>();
        lootChest = GameObject.FindGameObjectWithTag("Chest");
        chakra = chakraMax;
        currentHealthPoints = maximumHealthPoints;
        naruto = GetComponent<Rigidbody2D>();
        if (particleManager == null || gameManager == null || sfx == null)
        {
            return;
        }
    }
    #endregion
    #region Attack and Action
    public void Heal()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentHealthPoints > 0 && currentHealthPoints != maximumHealthPoints && medicalNinjaScrolls > 0 && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jump") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fall"))
        {
            narutoAnim.SetTrigger("Heal");
            Instantiate(particleManager.GetComponent<ParticleStorage>().particles[0], transform.position, Quaternion.identity);
            currentHealthPoints = maximumHealthPoints;
            medicalNinjaScrolls--;
        }
    }
    public void Attack()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jump") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fall"))
            {
                narutoAnim.SetTrigger("punch");
                isPunch = true;
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(throwablePos.position, attackRange, enemiesToAttack);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<BasicEnemyTraits>().TakeDamage(damageAmount);
                }
                Collider2D[] chestToOpen = Physics2D.OverlapCircleAll(throwablePos.position, attackRange, chestsToOpen);
                for (int i = 0; i < chestToOpen.Length; i++)
                {
                    if (chestToOpen[i].GetComponent<ChestBehav>().health > 0)
                    {
                        sfx.GetComponent<SFXStorage>().sound[11].Play();
                    }
                    chestToOpen[i].GetComponent<ChestBehav>().TakeDamage(damageAmount);
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);
        Gizmos.DrawWireSphere(throwablePos.position, attackRange);
    }
    public void Throwables() //make the weapon turn or smth
    {
        if (sceneName != "HowToPlay" && sceneName != "NarutoStoryFight")
        {
            if (timeBtwThrow <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1) && throwableCount > 0 && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Stance") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jutsu") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jump") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fall") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Landed") && preferenceCheck.GetComponent<PreferenceStorage>().weaponID == 0) //kunai
                {
                    preferenceCheck.GetComponent<PreferenceStorage>().weaponsThrown++;
                    narutoAnim.SetTrigger("throw");
                    sfx.GetComponent<SFXStorage>().sound[3].Play();
                    isAttack = true;
                    throwableCount--;
                    Instantiate(weapon.GetComponent<PrefabStorage>().prefabs[6], throwablePos.position, gameObject.transform.rotation); //KUNAI
                    timeBtwThrow = startTimeBtwThrow;
                }
                if (Input.GetKeyDown(KeyCode.Mouse1) && throwableCount > 0 && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Stance") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jutsu") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jump") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fall") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Landed") && preferenceCheck.GetComponent<PreferenceStorage>().weaponID == 1 && sceneName != "HowToPlay") //shuriken
                {
                    preferenceCheck.GetComponent<PreferenceStorage>().weaponsThrown++;
                    narutoAnim.SetTrigger("throw");
                    sfx.GetComponent<SFXStorage>().sound[13].Play();
                    isAttack = true;
                    throwableCount--;
                    Instantiate(weapon.GetComponent<PrefabStorage>().prefabs[7], throwablePos.position, gameObject.transform.rotation);
                    timeBtwThrow = startTimeBtwThrow;
                }
                if (Input.GetKeyDown(KeyCode.Mouse1) && throwableCount > 0 && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Stance") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jutsu") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jump") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fall") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Landed") && preferenceCheck.GetComponent<PreferenceStorage>().weaponID == 2 && sceneName != "HowToPlay") //fuma shuriken
                {
                    preferenceCheck.GetComponent<PreferenceStorage>().weaponsThrown++;
                    narutoAnim.SetTrigger("throw");
                    sfx.GetComponent<SFXStorage>().sound[13].Play();
                    isAttack = true;
                    throwableCount--;
                    Instantiate(weapon.GetComponent<PrefabStorage>().prefabs[9], throwablePos.position, gameObject.transform.rotation);
                    timeBtwThrow = startTimeBtwThrow;
                }
                if (Input.GetKeyDown(KeyCode.Mouse1) && throwableCount > 0 && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Stance") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jutsu") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jump") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fall") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Landed") && preferenceCheck.GetComponent<PreferenceStorage>().weaponID == 3 && sceneName != "HowToPlay") //throwing star
                {
                    preferenceCheck.GetComponent<PreferenceStorage>().weaponsThrown++;
                    narutoAnim.SetTrigger("throw");
                    sfx.GetComponent<SFXStorage>().sound[13].Play();
                    isAttack = true;
                    throwableCount--;
                    Instantiate(weapon.GetComponent<PrefabStorage>().prefabs[11], throwablePos.position, gameObject.transform.rotation);
                    timeBtwThrow = startTimeBtwThrow;
                }
            }
            else
            {
                timeBtwThrow -= Time.deltaTime;
            }
        }
        if (sceneName == "HowToPlay" || sceneName == "NarutoStoryFight")
        {
            if (timeBtwThrow <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1) && throwableCount > 0 && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Stance") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jutsu") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jump") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fall") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Landed") && (sceneName == "HowToPlay" || sceneName == "NarutoStoryFight")) //kunai
                {
                    narutoAnim.SetTrigger("throw");
                    sfx.GetComponent<SFXStorage>().sound[3].Play();
                    isAttack = true;
                    throwableCount--;
                    Instantiate(weapon.GetComponent<PrefabStorage>().prefabs[6], throwablePos.position, gameObject.transform.rotation); //KUNAI
                    timeBtwThrow = startTimeBtwThrow;
                }
            }
            else
            {
                timeBtwThrow -= Time.deltaTime;
            }
        }
    }
    public void StopAttack()
    {
        handSeal = false;
        isAttack = false;
        isPunch = false;
    }
    #endregion
    #region Movement and Jump
    public void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, canCollideWithGround);
    }
    public void Jump()
    {
        if (isGrounded && Input.GetButtonDown("Jump") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Stance"))
        {
            narutoAnim.SetTrigger("Jump");
            naruto.velocity = new Vector2(naruto.velocity.x, jumpForce);
        }
    }
    public void Movement()
    {
        if (!narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Landed") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Stance"))
        {
            if (!narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Shine")) {
                input = Input.GetAxisRaw("Horizontal");
                if (!narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jutsu")) 
                    {
                        naruto.velocity = new Vector2(input * Time.deltaTime * speed, naruto.velocity.y);
                    }
                else
                    {
                        naruto.velocity = new Vector2(0, naruto.velocity.y);
                    }
            }else
            {
                naruto.velocity = new Vector2(0, naruto.velocity.y);
            }
        }
        else
        {
            naruto.velocity = new Vector2(input * Time.deltaTime * speed * 0, naruto.velocity.y);
            if (Input.GetKeyDown(KeyCode.P))
            {
                narutoAnim.SetFloat("JutsuStanceValue", 0f);
            }
        }
    }
    #endregion
    #region Animation
    public void Animation()
    {
        if (isGrounded && narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Fall"))
        {
            narutoAnim.SetTrigger("Landed");
        }
        narutoAnim.SetFloat("falling", naruto.velocity.y);
        if (input < 0)
        {
            transform.localEulerAngles = new Vector3(180, 0, 0);
        }
        if (input < 0)
        {
            transform.localEulerAngles = new Vector3(180, 0, 0);
        }
        if (input != 0)
        {
            narutoAnim.SetBool("isMoving", true);
        }
        else if (input == 0)
        {
            narutoAnim.SetBool("isMoving", false);
        }
        if (input < 0)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (input > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (isPunch)
        {
            narutoAnim.SetTrigger("punch");
        }
        if (isAttack)
        {
            narutoAnim.SetTrigger("throw");
        }
        if (handSeal)
        {
            narutoAnim.SetTrigger("HandIgn");
        }
        if (narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Throw") || narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Punch") || narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("HandSeal"))
        {
            naruto.velocity = Vector2.zero; ;
            naruto.rotation = 0;
        }
    }
    #endregion
    #region ChakraStance
    public void JutsuAndChakraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) && chakra > cloneJutsuReq && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("jutsu")) //cast a clone
        {
            beforeJutsuMode.GetComponent<SmallJutsu>().JutsuOn();
            narutoAnim.SetTrigger("cloneTrigger");
            chakra -= cloneJutsuReq;
            narutoAnim.SetFloat("JutsuStanceValue", JutsuValue);
            jutsuUI.GetComponent<JutsuActivation>().ActivateJutsuMode();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && chakra > rasenganJutsuReq && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("jutsu"))
        {
            beforeJutsuMode.GetComponent<SmallJutsu>().JutsuOn();
            narutoAnim.SetTrigger("rasenganTrigger");
            chakra -= rasenganJutsuReq;
            narutoAnim.SetFloat("JutsuStanceValue", JutsuValue);
            jutsuUI.GetComponent<JutsuActivation>().ActivateJutsuMode();
        }
        if (Input.GetKeyDown(KeyCode.F)) //mi a fasz xd
        {
            beforeJutsuMode.GetComponent<SmallJutsu>().JutsuOn();
            narutoAnim.SetFloat("JutsuStanceValue", JutsuValue);
            jutsuUI.GetComponent<JutsuActivation>().ActivateJutsuMode();
        }
    }
    public void ChakraJutsuDetect()
    {
        if (narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Stance") || narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jutsu"))
        {
            if (Input.GetKeyDown(KeyCode.Alpha0) && chakra > cloneJutsuReq) //Clone, around naruto, throw a Kunai
            {
                narutoAnim.SetTrigger("cloneTrigger");
                chakra -= cloneJutsuReq;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && chakra > rasenganJutsuReq) //Throw a Rasengan
            {
                narutoAnim.SetTrigger("rasenganTrigger");
                chakra -= rasenganJutsuReq;
            }
            if (Input.GetKeyDown(KeyCode.F) && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jutsu"))
            {
                for (int i = 0; i < 1; i++)
                {
                    jutsuUI.GetComponent<JutsuActivation>().DeActivateJutsuMode();
                }
                beforeJutsuMode.GetComponent<SmallJutsu>().JutsuOff();
                narutoAnim.SetFloat("JutsuStanceValue", 0f);
                beforeJutsuMode.GetComponent<SmallJutsu>().jutsu.ResetTrigger("activated");
            }
        }
    }
    #endregion
    #region Trigger Colliders
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HazardGS"))
        {
            VibeCheck();
            currentHealthPoints -= 55;
        }
        if (collision.CompareTag("HazardK"))
        {
            VibeCheck();
            currentHealthPoints -= 20;
        }
        if (collision.CompareTag("Kamikaze"))
        {
            VibeCheck();
            currentHealthPoints -= 95;
        }
        #region pick ups
        if (collision.CompareTag("Hazard"))
        {
            sfx.GetComponent<SFXStorage>().sound[12].Play();
            throwableCount++;
        }
        if (collision.CompareTag("PickUpK"))
        {
            sfx.GetComponent<SFXStorage>().sound[12].Play();
            throwableCount++;
        }
        #endregion
    }
    public void VibeCheck()
    {
        if (currentHealthPoints <= 0 || chakra <= 0)
        {
           Die();
        }
    }
    public void Die()
    {
        narutoAnim.SetTrigger("Die");
    }
    #endregion
    #region Text to UI
    public void Text()
    {
        chakraText.text = Mathf.Round(Mathf.Abs(chakra)).ToString();
        kunaiText.text = throwableCount.ToString();
        mNSText.text = medicalNinjaScrolls.ToString();
    }
    #endregion
    #region Health System
    public void TakeDamage(float damage)
    {
        if (timeBtwPunchesReceived <= 0)
        {
            sfx.GetComponent<SFXStorage>().sound[17].Play();
            timeBtwPunchesReceived = startTimeBtwPunchesReceived;
        }
        else
        {
            timeBtwPunchesReceived -= Time.deltaTime;
        }
        currentHealthPoints -= damage;
    }
    public void FillSetup()
    {
        healthFill.fillAmount = currentHealthPoints / maximumHealthPoints;
    }
    #endregion
    #region Chakra System
    public void ChakraManager()
    {
         if (chakra != chakraMax && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Stance") && !narutoAnim.GetCurrentAnimatorStateInfo(0).IsTag("Jutsu"))
         {
            Mathf.Abs(Mathf.Round(chakra++));
         }
    }
    #endregion
    #region SoundManager
    public void FootstepSFX()
    {
        sfx.GetComponent<SFXStorage>().sound[7].Play();
    }
    public void PunchSFX()
    {
        sfx.GetComponent<SFXStorage>().sound[14].Play();
    }
    public void HealSFX()
    {
        sfx.GetComponent<SFXStorage>().sound[10].Play();
    }

    public void QueRasengan()
    {
        sfx.GetComponent<SFXStorage>().sound[19].Play();
    }

    public void HandsChakraConc()
    {
        sfx.GetComponent<SFXStorage>().sound[1].Play();
    }

    public void NarutoClapSFX()
    {
        sfx.GetComponent<SFXStorage>().sound[20].Play();
    }
    #endregion
    #region DeathEvent
    public void DeathEvent()
    {
        gameManager.GetComponent<PlayManager>().Died();
    }
    #endregion

    /*OMW to ichiraku
      O 
      \======
       \
        /\
       /  \_ _ 
      /
     */
}