using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Uchihascii : MonoBehaviour
{
///////////////////////////////
//UNNECCESSARY? UNNECCESSARY?
///////////////////////////////

    /*/Number and Logic based Variables
    [Header("Health")]
    public float maxHealth;
    [Space]
    [Header("Sharingan slowing time")]
    [Range(0,1)]public float sharinganSlowingTime;
    public float medicalNinjaScrollValue;
    public float chakraMax;
    public float kunaiCount;
    public float speed;
    public float startTimeBtwThrows;
    [Header("Sharingan Cooldown & Lenght")]
    public float startSharinganCooldown;
    public float startSharinganDuration;
    public float damage;
    [Range(0,5)]public float attackRange;
    public float startTimeBtwAttack;
    [Range(0, 5)] public float groundCheckRadius;
    public float jumpForce;
    public float chidoriJutsuReq;
    public float kunaiDamage;
    public float rasenganDamage;
    public bool isJumping;
    float curHealth;
    float timeBtwAttack;
    float currentTimeFlow;
    float sharinganCooldown;
    float sharinganDuration;
    float timeBtwThrows;
    float timeBtwCharge;
    float chakra;
    float input;
    bool changeDisplay = false;
    bool isPressed;
    bool isActivated;

    //Object based Variables
    public GameObject sharinganEffect;
    public GameObject chidori;
    public GameObject sharingan;
    public Transform attackPoint;
    public Transform groundCheckPos;
    public Image health;
    public Text sharinganDurationTimer;
    public Text sharinganCooldownTimer;
    public Text sharinganState;
    public Text chakraText;
    public Text medicalNinjaScrollText;
    public Text kunaiText;
    public LayerMask cloneLayer;
    public LayerMask enemiesToAttack;
    public LayerMask whatIsGround;
    public Transform attackRangePos;
    Rigidbody2D sasuke;
    GameObject particleEffects;
    GameObject sfx;
    GameObject prefabStorage;
    Animator animated;

    #region Start, Update and Awake
    private void Awake()
    {
        currentTimeFlow = Time.realtimeSinceStartup;
    }
    void Start()
    {
        Data();
    }
    void Update()
    {
        VibeCheck();
        HealthFill();
        Sharingan();
        SharinganTimerStart();
        Animation();
        GroundCheck();
        Jump();
        Throw();
        ChakraManager();
        Movement();
        Punch();
        Chidori();
    }
    private void LateUpdate()
    {
        Heal();
        TextToUI();
    }
    #endregion
    #region Data
    public void Data()
    {
        chakra = chakraMax;
        particleEffects = GameObject.FindGameObjectWithTag("ParticleStorage");
        curHealth = maxHealth;
        isPressed = false;
        isActivated = false;
        sfx = GameObject.FindGameObjectWithTag("SFXStorage");
        prefabStorage = GameObject.FindGameObjectWithTag("Storage");
        sasuke = GetComponent<Rigidbody2D>();
        animated = GetComponent<Animator>();
    }
    #endregion
    #region Movement and Jump
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isJumping && !animated.GetCurrentAnimatorStateInfo(0).IsTag("Throw"))
        {
            sasuke.velocity = new Vector2(sasuke.velocity.x, jumpForce);
        }
    }
    public void GroundCheck()
    {
        isJumping = Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, whatIsGround);
    }

    public void Movement()
    {
        if (!animated.GetCurrentAnimatorStateInfo(0).IsTag("Punch") && isActivated && !animated.GetCurrentAnimatorStateInfo(0).IsTag("Throw"))
        {
            input = Input.GetAxisRaw("Horizontal");
            sasuke.velocity = new Vector2(input * speed * ((Time.deltaTime / Time.timeScale) * 1.5f), sasuke.velocity.y);
        }
        else if (!animated.GetCurrentAnimatorStateInfo(0).IsTag("Punch") && !animated.GetCurrentAnimatorStateInfo(0).IsTag("Throw") && !isActivated)
        {
            input = Input.GetAxisRaw("Horizontal");
            sasuke.velocity = new Vector2(input * speed * Time.deltaTime, sasuke.velocity.y);
        }
        else
        {
            sasuke.velocity = new Vector2(0, sasuke.velocity.y);
        }
    }
    #endregion
    #region Animation
    public void Animation()
    {
        //animated.SetFloat("movingValue", input);
        if (input != 0)
        {
            animated.SetBool("isMoving", true);
        }
        else
        {
            animated.SetBool("isMoving", false);
        }
        if (input < 0)
        {
            transform.localEulerAngles = new Vector3(180, 0, 0);
        }
        if (input < 0)
        {
            transform.localEulerAngles = new Vector3(180, 0, 0);
        }
        if (input > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (input < 0)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }
    #endregion
    #region Attack
    public void Throw()
    {
        if (timeBtwThrows <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && kunaiCount > 0)
            {
                sfx.GetComponent<SFXStorage>().sound[3].Play();
                animated.SetTrigger("Throw");
                kunaiCount--;
                Instantiate(prefabStorage.GetComponent<PrefabStorage>().prefabs[6], attackPoint.position, transform.rotation);
                timeBtwThrows = startTimeBtwThrows;
            }
        }
        else
        {
            timeBtwThrows -= Time.deltaTime;
        }
    }
    public void Chidori()
    {
        if (Input.GetKeyDown(KeyCode.F) && chakra > chidoriJutsuReq)
        {
            animated.SetTrigger("Chidori");
            chakra -= chidoriJutsuReq;
        }
    }
    public void Punch()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                animated.SetTrigger("Punch");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesToAttack);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<NarutoBoss>().TakeDamage(damage);
                }
                Collider2D[] clonesToDamage = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, cloneLayer);
                for (int i = 0; i < clonesToDamage.Length; i++)
                {
                    clonesToDamage[i].GetComponent<NarutoBossClone>().TakeDamage(damage);
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
    #endregion
    #region Gizmo's
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackRangePos.position, attackRange);
        Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius);
    }
    #endregion
    #region Action
    public void Heal()
    {
        if (medicalNinjaScrollValue > 0 && Input.GetKeyDown(KeyCode.E) && curHealth < maxHealth)
        {
            Instantiate(particleEffects.GetComponent<ParticleStorage>().particles[0], transform.position, Quaternion.identity);
            sfx.GetComponent<SFXStorage>().sound[10].Play();
            curHealth = maxHealth;
            medicalNinjaScrollValue--;
        }
    }
    #endregion
    #region sharingan
    public void Sharingan()
    {
        if (sharinganCooldown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Q) && !isPressed)
            {
                sharinganEffect.GetComponent<Animator>().SetTrigger("ActivateSharingan");
                sfx.GetComponent<SFXStorage>().sound[13].Play();
                sharinganDuration = startSharinganDuration;
                isActivated = true; //activálódik a timer
                isPressed = true;
            }
        }else
        {
            sharinganCooldown -= Time.deltaTime;
        }
    }
    public void SharinganTimerStart() //ezt hívom az updatebe
    {
        if (isActivated) //amég aktiválódik, addig ismételje
        {
            if (sharinganDuration <= 0) // ha lejár a timer, akkor váltson vissza
            {
                Time.timeScale = 1.0f;
                sfx.GetComponent<SFXStorage>().sound[14].Play(); //deaktiválódik!
                isActivated = false; // most kapcsolja ki
                isPressed = false; // ezt is
                sharinganCooldown = startSharinganCooldown; //hozzáadok a cooldownhoz
                sharinganEffect.GetComponent<Animator>().ResetTrigger("ActivateSharingan");
                sharinganEffect.GetComponent<Animator>().SetTrigger("DeactivateSharingan");
                sharingan.GetComponent<Sharingan>().SharinganDefState(); //alap Animáció
            }
            else //még nem járt le, vonogasson le!
            {
                Time.timeScale = sharinganSlowingTime;
                sharinganDuration -= Time.deltaTime;
                sharingan.GetComponent<Sharingan>().SharinganActState(); //kapcsolja be az animációt
            }
        }
    }
    #endregion
    #region HealthCheck
    public void VibeCheck()
    {
        if (curHealth <= 0)
        {
            animated.SetTrigger("Die");
        }
    }
    #endregion
    #region Chakra System
    public void ChakraManager()
    {
        if (chakra < chakraMax && !animated.GetCurrentAnimatorStateInfo(0).IsTag("Action"))
        {
            Mathf.Abs(Mathf.Round(chakra ++));
        }
    }
    #endregion
    #region Health Fill
    public void HealthFill()
    {
        health.fillAmount = curHealth / maxHealth;
    }
    #endregion
    #region DamageToTake
    public void TakeTheDamage(float damage)
    {
        curHealth -= damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HazardK"))
        {
            curHealth -= kunaiDamage;
        }
        if (collision.CompareTag("Rasengan"))
        {
            curHealth -= rasenganDamage; 
        }
        if (collision.CompareTag("PickUpK"))
        {
            kunaiCount++;
        }
    }
    #endregion
    #region TextToUi
    public void TextToUI()
    {
        kunaiText.text = kunaiCount.ToString();
        medicalNinjaScrollText.text = medicalNinjaScrollValue.ToString();
        chakraText.text = chakra.ToString();
        sharinganCooldownTimer.text = Mathf.Round(sharinganCooldown).ToString();
        sharinganDurationTimer.text = Mathf.Round(sharinganDuration).ToString();
        if (sharinganDuration <= 0) //sharingan ability is exhausted DURATION
        {
            sharinganDurationTimer.text = Mathf.Round(sharinganDuration).ToString();
            sharinganDurationTimer.text = sharinganDurationTimer.ToString();
            sharinganState.text = "sharingan exhausted".ToString();
        }
        if (sharinganCooldown <= 0) //sharingan cooldown is down
        {
            sharinganCooldownTimer.text = startSharinganCooldown.ToString();
            sharinganState.text = "sharingan available".ToString();
        }
        if (sharinganCooldown <= 0 && sharinganDuration <= 0)// Can activate sharingan
        {
            sharinganDurationTimer.text = Mathf.Round(startSharinganDuration).ToString();
            sharinganCooldownTimer.text = Mathf.Round(startSharinganCooldown).ToString();
            sharinganState.text = "sharingan available".ToString();
        }
        if (isActivated && sharinganCooldown <= 0)
        {
            sharinganState.text = "sharingan activated".ToString();
        }
    }
    #endregion
    */
}
