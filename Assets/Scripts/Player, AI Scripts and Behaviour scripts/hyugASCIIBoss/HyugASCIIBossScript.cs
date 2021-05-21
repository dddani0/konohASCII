using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HyugASCIIBossScript : MonoBehaviour
{
///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    [Header("Access Gamemanager & Access Player & Access Preference Storage Attributes")]
    public GameObject gameManager;
    public GameObject player;
    public GameObject prefObj;
    [Header("Health & Shield & Movement Speed attributes & Visual Atttribute")]
    public Image healthBar;
    [Space]
    public Image chakraShieldBar;
    [Space]
    public GameObject chakraShieldVis;
    [Space]
    public GameObject trigramRotation;
    public float trigramSizeValue;
    [Space]
    public float chakraShield;
    public float chakraShieldMax;
    [Space]
    public float maxHealth;
    public float currentHealth;
    [Space]
    public float movementSpeed;
    [Space]
    public Rigidbody2D hBody;
    [Space]
    public Animator hAnim;
    [Header("Damage Attributes")]
    public float rasenganDamage;
    public float kunaiDamage;
    public float shurikenDamage;
    public float fumaShurikenDamage;
    public float throwingStarDamage;
    [Header("Behaviour attributes")]
    public float rngForState;
    public float waitBetweenStates;
    public float waitBetweenStatesMax;
    [Space]
    float startWaitBetweenStates; //          waitDistMin waitDistMax
    public float waitDistanceMin; //<---------O------|--I---| can move between | |
    public float waitDistanceMax;//        Player      Boss
    [Space]
    public float startCountWhileRunning;
    public float countWhileRunning;
    [Space]
    public bool stateLock;
    [Space]
    public bool cloneActive;
    public GameObject target;
    public GameObject clone;
    [Header("Attack Attributes")]
    public Transform throwPos;
    [Space]
    public float damageAmount;
    public Transform punchPos;
    public float punchRad;
    public LayerMask[] attackLay;
    [Space]
    public float throwCount = 1; //Pro scripting
    [Space]
    public float knockBack;
    [Header("Second Phase Byakugan Animation Access")]
    public GameObject byakugan;
    public float byakuganCount; //bruh, the memory :(

    #region Unity Functions
    void Start()
    {
        GetData();
        ChakraShieldHealthSetup();
    }

    void Update()
    {
        switch (hAnim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            case true:
                return; //Based? Based on what?
            case false:
                switch (cloneActive)
                {
                    #region Phases with Clone
                    case true:
                        switch (Vector2.Distance(player.transform.position, transform.position) < Vector2.Distance(clone.transform.position, transform.position))
                        {
                            case true:
                                target = player;
                                break;
                            case false:
                                target = clone;
                                break;
                        }
                        switch (chakraShield > 0) //With Clone
                        {
                            #region First Phase Clone Included
                            case true:
                                PhaseOneCloneIncluded();
                                break;
                            #endregion
                            #region Second Phase Clone Included
                            case false:
                                PhaseTwoCloneIncluded();
                                break;
                                #endregion
                        }
                        break;
                    #endregion
                    #region Phases without Clone
                    case false:
                        switch (chakraShield > 0) //And I oop...
                        {
                            #region First Phase
                            case true:
                                NejiBehaviourPhaseOne();
                                break;
                            #endregion
                            #region Second Phase
                            case false:
                                NejiBehaviourPhaseTwo();
                                break;
                                #endregion
                        }
                        break;
                        #endregion
                }
                Rotate();
                ChakraShieldHealthSetup();
                AnimationSetup();
                break;
        }
    }
    #endregion
    #region GetData
    public void GetData()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        prefObj = GameObject.FindGameObjectWithTag("PreferenceStorage");
        hBody = GetComponent<Rigidbody2D>();
        hAnim = GetComponentInChildren<Animator>();
        chakraShieldVis.SetActive(false);
        currentHealth = maxHealth;
        chakraShield = chakraShieldMax;
        waitBetweenStates = waitBetweenStatesMax;
        trigramRotation.SetActive(false);
        countWhileRunning = startCountWhileRunning;
    }
    #endregion
    #region Phases
    #region Phase One
    public void NejiBehaviourPhaseOne()
    {
        switch (player.GetComponent<PlayerScriptUzumASCII>().current_Health > 0 && !hAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion")) //Only do Stuff, if player is alive, and not doing action tagged with "Non-Motion"
        {
            case true:
                switch (stateLock) //also only do basic Behaviour if not entered a statelock!
                {
                    #region Not in StateLock, so basic Behavioury
                    case false:
                        if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) > waitDistanceMax && Vector2.Distance(player.transform.position, transform.position) > waitDistanceMin) // Going to 
                        {
                            hAnim.SetFloat("motion", 1f);
                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                            if (countWhileRunning <= 0) //Throw
                            {
                                Instantiate(gameManager.GetComponent<Manager>().weapons[0], throwPos.position, throwPos.rotation);
                                hAnim.SetTrigger("throw");
                                throwCount--;
                                countWhileRunning = startCountWhileRunning;
                            }
                            else
                            {
                                countWhileRunning -= Time.deltaTime;
                            }
                        }
                        else if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) < waitDistanceMax && Vector2.Distance(player.transform.position, transform.position) > waitDistanceMin) //In waiting State
                        {
                            hAnim.SetFloat("motion", 0f);
                            trigramRotation.SetActive(false); //Reset purpose
                            if (waitBetweenStates <= 0)
                            {
                                rngForState = Random.Range(1, 3);
                                stateLock = true;
                                waitBetweenStates = waitBetweenStatesMax; //reset's waiting state
                            }
                            else
                            {
                                waitBetweenStates -= Time.deltaTime;
                            }
                        }
                        else if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) < waitDistanceMax && Vector2.Distance(player.transform.position, transform.position) <= waitDistanceMin)
                        {
                            hAnim.SetFloat("motion", 0f);
                            StartCoroutine(WaitForKaiten());
                        }
                        break;
                    #endregion
                    #region In State lock, State lock Actionary
                    case true:
                        switch (rngForState)
                        {
                            case 1:
                                if (throwCount > 0)
                                {
                                    Instantiate(gameManager.GetComponent<Manager>().weapons[0], throwPos.position, throwPos.rotation);
                                    hAnim.SetTrigger("throw");
                                    throwCount--;
                                }
                                if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) > waitDistanceMin)
                                {
                                    if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) <= (waitDistanceMin / 2))
                                    {
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                        hAnim.SetFloat("motion", 0f);
                                        StartCoroutine(WaitForKaiten());
                                    }
                                    hAnim.SetFloat("motion", 1f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                                }
                                else if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) <= waitDistanceMin)
                                {
                                    hAnim.SetFloat("motion", 0f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                    hAnim.SetTrigger("Try_Attack");
                                    throwCount = 1;
                                    rngForState = 0;
                                    stateLock = false;
                                    if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) <= (waitDistanceMin / 2))
                                    {
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                        hAnim.SetFloat("motion", 0f);
                                        StartCoroutine(WaitForKaiten());
                                    }
                                }
                                break;
                            case 2:
                                if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) > waitDistanceMin)
                                {
                                    hAnim.SetFloat("motion", 1f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                                    if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) <= (waitDistanceMin / 2))
                                    {
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                        hAnim.SetFloat("motion", 0f);
                                        StartCoroutine(WaitForKaiten());
                                    }

                                }
                                else if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) < (waitDistanceMin + 1))
                                {
                                    hAnim.SetFloat("motion", 0f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                    hAnim.SetTrigger("Try_Attack");
                                    rngForState = 0;
                                    throwCount = 1;
                                    stateLock = false;
                                    if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) <= (waitDistanceMin / 2))
                                    {
                                        hAnim.SetFloat("motion", 0f);
                                        StartCoroutine(WaitForKaiten());
                                    }
                                }
                                break;
                            default: //Fuck
                                if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) <= (waitDistanceMin / 2))
                                {
                                    hAnim.SetFloat("motion", 0f);
                                    StartCoroutine(WaitForKaiten());
                                }
                                break;
                        }
                        break;
                        #endregion
                }
                break;
            case false:
                hAnim.SetFloat("motion", 0f);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0);
                break;
        }
    }
    #endregion
    #region Phase Two
    public void NejiBehaviourPhaseTwo()
    {
        if (byakuganCount < 1) //So much for the switch statement NOW IT'S GONNA BE INSANE TO WATCH THE END OF THE WORLD
        {
            switch (player.GetComponent<PlayerScriptUzumASCII>().current_Health > 0 && !hAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion")) //Only do Stuff, if player is alive, and not doing action tagged with "Non-Motion"
            {
                case true:
                    switch (stateLock) //also only do basic Behaviour if not entered a statelock!
                    {
                        #region Not in StateLock, so basic Behavioury
                        case false:
                            if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) > waitDistanceMin) // Going to 
                            {
                                hAnim.SetFloat("punch", 0f);
                                hAnim.SetFloat("motion", 1f);
                                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                                if (countWhileRunning <= 0) //Throw
                                {
                                    Instantiate(gameManager.GetComponent<Manager>().weapons[0], throwPos.position, throwPos.rotation);
                                    hAnim.SetTrigger("throw");
                                    throwCount--;
                                    countWhileRunning = startCountWhileRunning;
                                }
                                else
                                {
                                    countWhileRunning -= Time.deltaTime;
                                }
                            }
                            else if (Vector2.Distance(player.transform.position, transform.position) <= waitDistanceMin) //In waiting State
                            {
                                hAnim.SetFloat("punch", 1f);
                                hAnim.SetFloat("motion", 0f);
                                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0);
                                if (waitBetweenStates <= 0)
                                {
                                    rngForState = Random.Range(1, 3);
                                    stateLock = true;
                                    waitBetweenStates = waitBetweenStatesMax; //reset's waiting state
                                }
                                else
                                {
                                    waitBetweenStates -= Time.deltaTime;
                                }
                            }
                            break;
                        #endregion
                        #region In State lock, State lock Actionary
                        case true:
                            switch (rngForState)
                            {
                                case 1:
                                    if (throwCount > 0)
                                    {
                                        Instantiate(gameManager.GetComponent<Manager>().weapons[0], throwPos.position, throwPos.rotation);
                                        hAnim.SetTrigger("throw");
                                        throwCount--;
                                    }
                                    if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) > waitDistanceMin)
                                    {
                                        hAnim.SetFloat("motion", 1f);
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                                    }
                                    else if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) <= waitDistanceMin)
                                    {
                                        hAnim.SetFloat("motion", 0f);
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                        hAnim.SetTrigger("Try_Attack");
                                        throwCount = 1;
                                        rngForState = 0;
                                        stateLock = false;
                                    }
                                    break;
                                case 2:
                                    if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) > waitDistanceMin)
                                    {
                                        hAnim.SetFloat("motion", 1f);
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);

                                    }
                                    else if (Vector2.Distance(new Vector2(player.transform.position.x, transform.position.y), transform.position) < (waitDistanceMin + 1))
                                    {
                                        hAnim.SetFloat("motion", 0f);
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                        hAnim.SetTrigger("Try_Attack");
                                        rngForState = 0;
                                        throwCount = 1;
                                        stateLock = false;
                                    }
                                    break;
                                default: //Fuck
                                    return; //Based? Poggers? Cringe ? Please shut the fuck up! 😭
                            }
                            break;
                            #endregion
                    }
                    break;

            }
        }
        else if (byakuganCount > 0)
        {
            hBody.constraints = RigidbodyConstraints2D.None;
            hAnim.SetTrigger("SecondPhase");
            if (transform.localScale.x == -1) //jobb
            {
                hBody.AddForce(-transform.right * knockBack);
            }
            else if (transform.localScale.x == 1)
            {
                hBody.AddForce(transform.right * knockBack);
            }
            hBody.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
    #endregion
    #region Search for Clone
    public void SearchForClone()
    {
        clone = GameObject.FindGameObjectWithTag("Clone");
        cloneActive = true;
    }
    #endregion
    #region Phase one with Clone
    public void PhaseOneCloneIncluded()
    {
        switch (player.GetComponent<PlayerScriptUzumASCII>().current_Health > 0 && !hAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion")) //Only do Stuff, if player is alive, and not doing action tagged with "Non-Motion"
        {

            case true:
                switch (stateLock) //also only do basic Behaviour if not entered a statelock!
                {
                    #region Not in StateLock, so basic Behavioury
                    case false:
                        if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) > waitDistanceMax && Vector2.Distance(target.transform.position, transform.position) > waitDistanceMin) // Going to 
                        {
                            hAnim.SetFloat("motion", 1f);
                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                            if (countWhileRunning <= 0) //Throw
                            {
                                Instantiate(gameManager.GetComponent<Manager>().weapons[0], throwPos.position, throwPos.rotation);
                                hAnim.SetTrigger("throw");
                                throwCount--;
                                countWhileRunning = startCountWhileRunning;
                            }
                            else
                            {
                                countWhileRunning -= Time.deltaTime;
                            }
                        }
                        else if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) < waitDistanceMax && Vector2.Distance(target.transform.position, transform.position) > waitDistanceMin) //In waiting State
                        {
                            hAnim.SetFloat("motion", 0f);
                            trigramRotation.SetActive(false); //Reset purpose
                            if (waitBetweenStates <= 0)
                            {
                                rngForState = Random.Range(1, 3);
                                stateLock = true;
                                waitBetweenStates = waitBetweenStatesMax; //reset's waiting state
                            }
                            else
                            {
                                waitBetweenStates -= Time.deltaTime;
                            }
                        }
                        else if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) < waitDistanceMax && Vector2.Distance(target.transform.position, transform.position) <= waitDistanceMin)
                        {
                            hAnim.SetFloat("motion", 0f);
                            StartCoroutine(WaitForKaiten());
                        }
                        break;
                    #endregion
                    #region In State lock, State lock Actionary
                    case true:
                        switch (rngForState)
                        {
                            case 1:
                                if (throwCount > 0)
                                {
                                    Instantiate(gameManager.GetComponent<Manager>().weapons[0], throwPos.position, throwPos.rotation);
                                    hAnim.SetTrigger("throw");
                                    throwCount--;
                                }
                                if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) > waitDistanceMin)
                                {
                                    if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) <= (waitDistanceMin / 2))
                                    {
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                        hAnim.SetFloat("motion", 0f);
                                        StartCoroutine(WaitForKaiten());
                                    }
                                    hAnim.SetFloat("motion", 1f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                                }
                                else if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) <= waitDistanceMin)
                                {
                                    hAnim.SetFloat("motion", 0f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                    hAnim.SetTrigger("Try_Attack");
                                    throwCount = 1;
                                    rngForState = 0;
                                    stateLock = false;
                                    if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) <= (waitDistanceMin / 2))
                                    {
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                        hAnim.SetFloat("motion", 0f);
                                        StartCoroutine(WaitForKaiten());
                                    }
                                }
                                break;
                            case 2:
                                if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) > waitDistanceMin)
                                {
                                    hAnim.SetFloat("motion", 1f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                                    if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) <= (waitDistanceMin / 2))
                                    {
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                        hAnim.SetFloat("motion", 0f);
                                        StartCoroutine(WaitForKaiten());
                                    }

                                }
                                else if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) < (waitDistanceMin + 1))
                                {
                                    hAnim.SetFloat("motion", 0f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                    hAnim.SetTrigger("Try_Attack");
                                    rngForState = 0;
                                    throwCount = 1;
                                    stateLock = false;
                                    if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) <= (waitDistanceMin / 2))
                                    {
                                        hAnim.SetFloat("motion", 0f);
                                        StartCoroutine(WaitForKaiten());
                                    }
                                }
                                break;
                            default: //Fuck
                                if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) <= (waitDistanceMin / 2))
                                {
                                    hAnim.SetFloat("motion", 0f);
                                    StartCoroutine(WaitForKaiten());
                                }
                                break;
                        }
                        break;
                        #endregion
                }
                break;
            case false:
                hAnim.SetFloat("motion", 0f);
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), 0);
                break;
        }
    }
    #endregion
    #region Phase two with Clone
    public void PhaseTwoCloneIncluded()
    {
        if (byakuganCount < 1) //So much for the switch statement NOW IT'S GONNA BE INSANE TO WATCH THE END OF THE WORLD
        {
            switch (player.GetComponent<PlayerScriptUzumASCII>().current_Health > 0 && !hAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion")) //Only do Stuff, if player is alive, and not doing action tagged with "Non-Motion"
            {
                case true:
                    switch (stateLock) //also only do basic Behaviour if not entered a statelock!
                    {
                        #region Not in StateLock, so basic Behavioury
                        case false:
                            switch (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) > waitDistanceMin) //Fix this fucking mess
                            {
                                case true:
                                    hAnim.SetFloat("punch", 0f);
                                    hAnim.SetFloat("motion", 1f);
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                                    if (countWhileRunning <= 0) //Throw
                                    {
                                        Instantiate(gameManager.GetComponent<Manager>().weapons[0], throwPos.position, throwPos.rotation);
                                        hAnim.SetTrigger("throw");
                                        throwCount--;
                                        countWhileRunning = startCountWhileRunning;
                                    }
                                    else
                                    {
                                        countWhileRunning -= Time.deltaTime;
                                    }
                                    break;
                                case false:
                                    if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) < waitDistanceMin)
                                    {
                                        hAnim.SetFloat("punch", 1f);
                                    }
                                    else
                                    {
                                        hAnim.SetFloat("punch", 0f);
                                    }
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), 0);
                                    hAnim.SetFloat("motion", 0f);
                                    trigramRotation.SetActive(false); //Reset purpose
                                    if (waitBetweenStates <= 0)
                                    {
                                        rngForState = Random.Range(1, 3);
                                        stateLock = true;
                                        waitBetweenStates = waitBetweenStatesMax; //reset's waiting state
                                    }
                                    else
                                    {
                                        waitBetweenStates -= Time.deltaTime;
                                    }
                                    break;
                            }
                            break;
                        #endregion
                        #region In State lock, State lock Actionary
                        case true:
                            switch (rngForState)
                            {
                                case 1:
                                    if (throwCount > 0)
                                    {
                                        Instantiate(gameManager.GetComponent<Manager>().weapons[0], throwPos.position, throwPos.rotation);
                                        hAnim.SetTrigger("throw");
                                        throwCount--;
                                    }
                                    if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) > waitDistanceMin)
                                    {
                                        hAnim.SetFloat("motion", 1f);
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
                                    }
                                    else if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) <= waitDistanceMin)
                                    {
                                        hAnim.SetFloat("motion", 0f);
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                        hAnim.SetTrigger("Try_Attack");
                                        throwCount = 1;
                                        rngForState = 0;
                                        stateLock = false;
                                    }
                                    break;
                                case 2:
                                    if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) > waitDistanceMin)
                                    {
                                        hAnim.SetFloat("motion", 1f);
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);

                                    }
                                    else if (Vector2.Distance(new Vector2(target.transform.position.x, transform.position.y), transform.position) < (waitDistanceMin + 1))
                                    {
                                        hAnim.SetFloat("motion", 0f);
                                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), 0 * Time.deltaTime);
                                        hAnim.SetTrigger("Try_Attack");
                                        rngForState = 0;
                                        throwCount = 1;
                                        stateLock = false;
                                    }
                                    break;
                                default: //Fuck
                                    return; //Based? Poggers? Cringe ? Please shut the fuck up! 😭
                            }
                            break;
                            #endregion
                    }
                    break;

            }
        }
        else if (byakuganCount > 0)
        {
            hBody.constraints = RigidbodyConstraints2D.None;
            hAnim.SetTrigger("SecondPhase");
            if (transform.localScale.x == -1) //jobb
            {
                hBody.AddForce(-transform.right * knockBack);
            }
            else if (transform.localScale.x == 1)
            {
                hBody.AddForce(transform.right * knockBack);
            }
            hBody.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
    #endregion
    #endregion
    #region IEnumerator Delays
    IEnumerator ByakuganDelay()
    {
        byakugan.GetComponent<Animator>().SetTrigger("Phase_Two");
        yield return new WaitForSeconds(1f);
        byakuganCount--;
    }
    IEnumerator WaitForKaiten()
    {
        yield return new WaitForSeconds(0.5f);
        hAnim.SetTrigger("KaitenDefense");
        hAnim.SetFloat("motion", 0f);
        yield return new WaitForSeconds(1f);
    }
    IEnumerator PunchInSecondPhase()
    {
        hAnim.SetTrigger("Try_Attack");
        yield return new WaitForSeconds(3f);
    }
    #endregion
    #region Attack property
    public void AttackWithPunch() //Will use Animation Event
    {
        Collider2D[] playerCol = Physics2D.OverlapCircleAll(punchPos.position, punchRad, attackLay[0]);
        Collider2D[] cloneCol = Physics2D.OverlapCircleAll(punchPos.position, punchRad, attackLay[1]);
        for (int i = 0; i < playerCol.Length; i++)
        {
            playerCol[i].GetComponent<PlayerScriptUzumASCII>().TakeDamage(damageAmount);
        }
        for (int i = 0; i < cloneCol.Length; i++)
        {
            //cloneCol[i].GetComponent<CloneBehav>().TakeDamage(damageAmount);
        }
    }
    #endregion
    #region Animation Properties
    public void AnimationSetup()
    {
        if (currentHealth <= 0)
        {
            gameManager.GetComponent<Manager>().preference_Storage_GameObject.GetComponent<PreferenceStorage>().timer = gameManager.GetComponent<Manager>().timer;
        }
        hAnim.SetFloat("deathValue", currentHealth);
        if (!hAnim.GetCurrentAnimatorStateInfo(0).IsName("Kaiten_Defense")) //Reset trigramRotation
        {
            trigramRotation.SetActive(false);
        }
    }
    public void Rotate()
    { //cpy + pst from "Jirobo.cs" //ok who the fuck asked? xd
        switch (cloneActive)
        {
            case true:
                if (transform.position.x > target.transform.position.x && !hAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    throwPos.localEulerAngles = new Vector3(0, 180, 0);
                }
                else if (transform.position.x < target.transform.position.x && !hAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    throwPos.localEulerAngles = new Vector3(0, 0, 0);
                }
                break;
            case false:
                if (transform.position.x > player.transform.position.x && !hAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    throwPos.localEulerAngles = new Vector3(0, 180, 0);
                }
                else if (transform.position.x < player.transform.position.x && !hAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    throwPos.localEulerAngles = new Vector3(0, 0, 0);
                }
                break;
        }
    }
    public void ChakraShieldHealthSetup()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        chakraShieldBar.fillAmount = chakraShield / chakraShieldMax;
    }
    #endregion
    #region Collision, Damage properties & Other
    public void TakeDamage(float damage)
    {
        switch (chakraShield > 0)
        {
            case true:
                chakraShield -= damage;
                gameManager.GetComponent<Manager>().sound_Effects[23].Play();
                break;
            case false:
                gameManager.GetComponent<Manager>().sound_Effects[20].Play();
                currentHealth -= damage;
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region Damage properties in Phase one and Two
        switch (chakraShield > 0)
        {
            #region Phase One, Take Damage from Ninja Weapons on a condition
            case true:
                switch (hAnim.GetCurrentAnimatorStateInfo(0).IsName("Stagger_Rasengan"))
                {
                    #region Take Damage upon Stagger
                    case true:
                        if (collision.CompareTag("Kunai"))
                        {
                            chakraShield -= kunaiDamage;
                        }
                        else if (collision.CompareTag("Shuriken"))
                        {
                            chakraShield -= shurikenDamage;
                        }
                        else if (collision.CompareTag("FumaShuriken"))
                        {
                            chakraShield -= fumaShurikenDamage;
                        }
                        else if (collision.CompareTag("ThrowingStar"))
                        {
                            chakraShield -= throwingStarDamage;
                        }
                        else if (collision.CompareTag("Rasengan"))
                        {
                            chakraShield -= rasenganDamage;
                        }
                        break;
                    #endregion
                    #region Don't Take Damage from Ninja Weapons in Phase one
                    case false:
                        if (collision.CompareTag("Kunai"))
                        {
                            hAnim.SetTrigger("Stagger");
                            StartCoroutine(DelayShieldAsVisual());
                        }
                        else if (collision.CompareTag("Shuriken"))
                        {
                            hAnim.SetTrigger("Stagger");
                            StartCoroutine(DelayShieldAsVisual());
                        }
                        else if (collision.CompareTag("FumaShuriken"))
                        {
                            hAnim.SetTrigger("Stagger");
                            StartCoroutine(DelayShieldAsVisual());
                        }
                        else if (collision.CompareTag("ThrowingStar"))
                        {
                            hAnim.SetTrigger("Stagger");
                            StartCoroutine(DelayShieldAsVisual());
                        }
                        else if (collision.CompareTag("Rasengan"))
                        {
                            hAnim.SetTrigger("staggerRasengan");
                            chakraShield -= rasenganDamage;
                        }
                        break;
                        #endregion
                }
                break;
            #endregion
            #region Phase two, Take Damage from Ninja Weapon
            case false:
                if (collision.CompareTag("Kunai"))
                {
                    currentHealth -= kunaiDamage;
                }
                else if (collision.CompareTag("Shuriken"))
                {
                    currentHealth -= shurikenDamage;
                }
                else if (collision.CompareTag("FumaShuriken"))
                {
                    currentHealth -= fumaShurikenDamage;
                }
                else if (collision.CompareTag("ThrowingStar"))
                {
                    currentHealth -= throwingStarDamage;
                }
                else if (collision.CompareTag("Rasengan"))
                {
                    trigramRotation.SetActive(false);
                    StartCoroutine(DelayShieldAsVisual());
                    hAnim.SetTrigger("staggerRasengan");
                    chakraShield -= (rasenganDamage * 2); //Receive double damage, when phase 2
                }
                break;
                #endregion
        }
        #endregion
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPos.position, punchRad);
    }
    IEnumerator DelayShieldAsVisual()
    {
        chakraShieldVis.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        chakraShieldVis.SetActive(false);
    }
    #endregion
}
/*
██████╗░░█████╗░███╗░░██╗██╗███████╗██╗░░░░░  ░█████╗░░█████╗░███╗░░██╗████████╗
██╔══██╗██╔══██╗████╗░██║██║██╔════╝██║░░░░░  ██╔══██╗██╔══██╗████╗░██║╚══██╔══╝
██║░░██║███████║██╔██╗██║██║█████╗░░██║░░░░░  ██║░░╚═╝███████║██╔██╗██║░░░██║░░░
██║░░██║██╔══██║██║╚████║██║██╔══╝░░██║░░░░░  ██║░░██╗██╔══██║██║╚████║░░░██║░░░
██████╔╝██║░░██║██║░╚███║██║███████╗███████╗  ╚█████╔╝██║░░██║██║░╚███║░░░██║░░░
╚═════╝░╚═╝░░╚═╝╚═╝░░╚══╝╚═╝╚══════╝╚══════╝  ░╚════╝░╚═╝░░╚═╝╚═╝░░╚══╝░░░╚═╝░░░

░█████╗░░█████╗░██████╗░███████╗  ███████╗░█████╗░██████╗░  ░██████╗██╗░░██╗██╗████████╗
██╔══██╗██╔══██╗██╔══██╗██╔════╝  ██╔════╝██╔══██╗██╔══██╗  ██╔════╝██║░░██║██║╚══██╔══╝
██║░░╚═╝██║░░██║██║░░██║█████╗░░  █████╗░░██║░░██║██████╔╝  ╚█████╗░███████║██║░░░██║░░░
██║░░██╗██║░░██║██║░░██║██╔══╝░░  ██╔══╝░░██║░░██║██╔══██╗  ░╚═══██╗██╔══██║██║░░░██║░░░
╚█████╔╝╚█████╔╝██████╔╝███████╗  ██║░░░░░╚█████╔╝██║░░██║  ██████╔╝██║░░██║██║░░░██║░░░
░╚════╝░░╚════╝░╚═════╝░╚══════╝  ╚═╝░░░░░░╚════╝░╚═╝░░╚═╝  ╚═════╝░╚═╝░░╚═╝╚═╝░░░╚═╝░░░

██╗░░██╗██████╗░
╚██╗██╔╝██╔══██╗
░╚███╔╝░██║░░██║
░██╔██╗░██║░░██║
██╔╝╚██╗██████╔╝
╚═╝░░╚═╝╚═════╝░
*/