using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CloneBehav : MonoBehaviour
{
    [Header("Gamemanager Access")]
    public GameObject game_Manager;
    [Header("Preference Storage Access")]
    public GameObject preference_Storage;
    [Header("Player GameObject")]
    public GameObject player_GameObject;
    [Header("Clone attributes")]
    //
    Animator clone_Animator;
    Rigidbody2D clone_Rigidbody;
    //
    public float health;
    [Space]
    public float maximum_Follow_Distance; //Same with Neji boss
    public float minimum_Follow_Distance;
    [Space]
    public float ninja_Weapon_Count;
    [Space]
    public float healing_Scroll_Count;
    [Space]
    public BoxCollider2D body_Collider;
    [Header("Movement Attributes")]
    public float movement_Speed;
    [Header("Attack attributes")]
    public float damage_Amount;
    [Space]
    public GameObject target;
    [Space]
    public bool targetLock;
    [Space]
    public Transform punch_Position;
    [Space]
    public float punch_Radius;
    [Space]
    public LayerMask[] enemy_Layer;
    [Space]
    public float startTimeBtwAction;
    public float timeBtwAction;
    [Space]
    public bool canThrow;
    [Space]
    public BoxCollider2D collision_Detector;
    [Space]
    [Tooltip("Checks whether the clone is used for manouver")]
    public bool isCommanded;
    [Header("For Boss Mode")]
    public GameObject boss;

    #region Unity Functions
    void Start()
    {
        GetData();
        game_Manager.GetComponent<Manager>().instances.Add(this.gameObject);
    }

    void Update()
    {
        if (game_Manager.GetComponent<Manager>().current_Loaded_Scene_Name.Contains("Difficulty"))
        {
            AdventureModeCloneBehaviour();
        }
        //switch (current_Loaded_Scene_Name) //Whoa, first switch statement!
        //{
        //    case "UzumASCIIAdventureNormalDifficulty": //This looks sloppy, <3 memory usage
        //        AdventureModeBehav();
        //        break;
        //    case "UzumASCIIAdventureHardDifficulty":
        //        AdventureModeBehav();
        //        break;
        //    case "UzumASCIIAdventureVeryHardDifficulty": //Adv. Mode
        //        AdventureModeBehav();
        //        break;
        //    case "UzumASCIIBoss": //Neji Boss Battle
        //        NejiBossBehav();
        //        break;
        //    case "UchihASCIIBoss": //Sasuke Boss Battle
        //        NarutoBossBehav();
        //        break;
        //    case "TrainingModeNaruto": //Training Grounds
        //        TargetRangeBehav();
        //        break;
        //    case "NarutoTutorial": //Tutorial
        //        TrainingModeNaruto();
        //        break;
        //}
        //Animation();
        //if (targetLock)
        //{
        //    CheckThrowStatus();
        //}
    }
    private void LateUpdate()
    {

    }
    #endregion
    #region Get Data
    public void GetData()
    {
        game_Manager = GameObject.FindGameObjectWithTag("GameManager");
        preference_Storage = GameObject.FindGameObjectWithTag("PreferenceStorage");
        clone_Animator = GetComponentInChildren<Animator>();
        clone_Rigidbody = GetComponent<Rigidbody2D>();
        player_GameObject = GameObject.FindGameObjectWithTag("Player");
        if (GameObject.FindGameObjectWithTag("Boss") != null)
            boss = GameObject.FindGameObjectWithTag("Boss");
        for (int i = 0; i < game_Manager.GetComponent<Manager>().clone_Indicator_UI_Elements.Length; i++)
        {
            game_Manager.GetComponent<Manager>().clone_Indicator_UI_Elements[i].SetActive(true);
        }

    }
    #endregion
    #region Behaviours
    #region Adventure Mode Behaviour
    public void AdventureModeCloneBehaviour()
    {
        switch (targetLock)
        {
            case true: //Attacking State
                switch (target.GetComponent<EnemyState>().health > 0)
                {
                    case true:
                        if (target == null || target.GetComponent<EnemyState>().health <= 0)
                        {
                            collision_Detector.enabled = true;
                            clone_Animator.SetFloat("Punch", 0f);
                            targetLock = false;
                        }
                        switch (Vector2.Distance(transform.position, target.transform.position) > minimum_Follow_Distance)
                        {
                            case true:
                                clone_Animator.SetFloat("motionValue", 1f);
                                transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), movement_Speed * Time.deltaTime);
                                if (timeBtwAction <= 0 && ninja_Weapon_Count > 0)
                                {
                                    ninja_Weapon_Count--;
                                    clone_Animator.SetTrigger("throw");
                                    switch (preference_Storage.GetComponent<PreferenceStorage>().weapon) //Set weapon preferences
                                    {
                                        case 0:
                                            //game_Manager.GetComponent<Manager>().sound_Effects[6].Play();
                                            break;
                                        case 1:
                                            //game_Manager.GetComponent<Manager>().sound_Effects[7].Play();
                                            break;
                                        case 2:
                                            //game_Manager.GetComponent<Manager>().sound_Effects[8].Play();
                                            break;
                                        case 3:
                                            //game_Manager.GetComponent<Manager>().sound_Effects[7].Play();
                                            break;
                                    }
                                    Instantiate(game_Manager.GetComponent<Manager>().weapons[preference_Storage.GetComponent<PreferenceStorage>().weapon], punch_Position.position, punch_Position.transform.rotation);
                                    timeBtwAction = startTimeBtwAction;
                                    //switch (current_Loaded_Scene_Name != "NarutoTutorial")
                                    //{
                                    //    case true:
                                    //        break;
                                    //    case false:
                                    //        ninja_Weapon_Count--;
                                    //        clone_Animator.SetTrigger("throw");
                                    //        game_Manager.GetComponent<Manager>().sound_Effects[6].Play();
                                    //        Instantiate(game_Manager.GetComponent<Manager>().weapons[0], punch_Position.position, punch_Position.transform.rotation);
                                    //        timeBtwA = mTimeBtwA;
                                    //        break;
                                    //}
                                }
                                else
                                {
                                    timeBtwAction -= Time.deltaTime;
                                }
                                switch (transform.position.x > target.transform.position.x)
                                {
                                    case true:
                                        transform.localScale = new Vector3(-1, 1, 1);
                                        punch_Position.transform.localEulerAngles = new Vector3(0, 180, 0);
                                        break;
                                    case false:
                                        transform.localScale = new Vector3(1, 1, 1);
                                        punch_Position.transform.localEulerAngles = new Vector3(0, 180, 0);
                                        break;
                                }
                                break;
                            case false:
                                clone_Animator.SetFloat("motionValue", 0f);
                                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y), 0);
                                clone_Animator.SetFloat("Punch", 1f);
                                break;
                        }

                        break;
                    case false:
                        if (target == null)
                        {
                            collision_Detector.enabled = true;
                        }
                        clone_Animator.SetFloat("Punch", 0f);
                        targetLock = false;
                        break;
                }
                break;
            case false: //Default state
                if (!targetLock && !collision_Detector.enabled)
                    collision_Detector.enabled = true;
                if (transform.position.x < player_GameObject.transform.position.x) //Left side, could face right //Rotation
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    punch_Position.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
                else if (transform.position.x > player_GameObject.transform.position.x) //Right side, could face left
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    punch_Position.transform.localEulerAngles = new Vector3(0, 180, 0);
                }
                if (Vector2.Distance(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y)) > maximum_Follow_Distance) //Fucking use tags, you useless piece of shit
                {
                    clone_Animator.SetFloat("motionValue", 1f);
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y), movement_Speed * Time.deltaTime);
                }
                else if (Vector2.Distance(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y)) > minimum_Follow_Distance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y), 0);
                    clone_Animator.SetFloat("motionValue", 0f);
                }
                break;
        }
    }
    //    public void AdventureModeBehav()
    //    {
    //        if (!clone_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
    //        {
    //            switch (isCommanded) 
    //            {
    //                case false:
    ////
    //                    switch (targetLock)
    //                    {
    //                        case false:
    //                            if (Input.GetKeyDown(KeyCode.Alpha2))
    //                            {
    //                                //gManager.GetComponent<Manager>().sound_Effects[12].Play();
    //                                switch (player_GameObject.transform.localScale.x)
    //                                {
    //                                    case 1: //Right
    //                                        command_Position.x += 7.5f;
    //                                        break;
    //                                    case -1: //Left
    //                                        command_Position.x -= 7.5f;
    //                                        break;
    //                                }
    //                                isCommanded = true;
    //                            }
    //                            if (Vector2.Distance(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y)) > maximum_Aggro_Distance && !clone_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion")) //Fucking use tags, you useless piece of shit
    //                            {
    //                                clone_Animator.SetFloat("motionValue", 1f);
    //                                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y), movement_Speed * Time.deltaTime);
    //                            }
    //                            else if (Vector2.Distance(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y)) > minimum_Aggro_Distance && !clone_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
    //                            {
    //                                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y), 0);
    //                                clone_Animator.SetFloat("motionValue", 0f);
    //                            }
    //                            if (current_Health < maximum_Health && healing_Scroll_Count > 0)
    //                            {
    //                                healing_Scroll_Count--;
    //                                clone_Animator.SetTrigger("healWound");
    //                                current_Health = maximum_Health;
    //                            }
    //                            if (transform.position.x < player_GameObject.transform.position.x) //Left side, could face right //Rotation
    //                            {
    //                                transform.localScale = new Vector3(1, 1, 1);
    //                                punch_Position.transform.localEulerAngles = new Vector3(0, 0, 0);
    //                            }
    //                            else if (transform.position.x > player_GameObject.transform.position.x) //Right side, could face left
    //                            {
    //                                transform.localScale = new Vector3(-1, 1, 1);
    //                                punch_Position.transform.localEulerAngles = new Vector3(0, 180, 0);
    //                            }
    //                            break;
    //                        case true:
    //                            

    //                            /*if (target == null || target.GetComponent<EnemyState>().health <= 0)
    //                            {
    //                                for (int i = 0; i < dh.Length; i++)
    //                                {
    //                                    dh[i].enabled = true;
    //                                }
    //                                cAnim.SetFloat("Punch", 0f);
    //                                targetLock = false;
    //                            }
    //                            else if (target.GetComponent<EnemyState>().health > 0)
    //                            {
    //                                if (Vector2.Distance(transform.position, target.transform.position) > minDist && !cAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
    //                                {
    //                                    cAnim.SetFloat("motionValue", 1f);
    //                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
    //                                    if (timeBtwA <= 0 && ninjaWeaponCount > 0 && canThrow)
    //                                    {
    //                                        ninjaWeaponCount--;
    //                                        cAnim.SetTrigger("throw");
    //                                        Instantiate(gManager.GetComponent<Manager>().weapons[prefStorage.GetComponent<PreferenceStorage>().weapon], punchPos.position, punchPos.transform.rotation);
    //                                        timeBtwA = mTimeBtwA;
    //                                    }
    //                                    else if (timeBtwA > 0)
    //                                    {
    //                                        timeBtwA -= Time.deltaTime;
    //                                    }
    //                                }
    //                                else if (Vector2.Distance(transform.position, target.transform.position) < minDist && !cAnim.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
    //                                {
    //                                    cAnim.SetFloat("motionValue", 0f);
    //                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 0);
    //                                    cAnim.SetFloat("Punch", 1f);
    //                                }
    //                                if (target.name == "Yoroi" && Vector2.Distance(transform.position, target.transform.position) < (minDist + 2))
    //                                {
    //                                    cAnim.SetFloat("motionValue", 0f);
    //                                    //cAnim.SetTrigger("Jump");
    //                                    cBody.gravityScale = player.GetComponent<PlayerScriptUzumASCII>().uBody.gravityScale;
    //                                    cBody.velocity = new Vector2(cBody.velocity.x, jumpHeight);
    //                                }
    //                                if (transform.position.x < target.transform.position.x) //Left side, could face right //Rotation
    //                                {
    //                                    transform.localScale = new Vector3(1, 1, 1);
    //                                    punchPos.transform.localEulerAngles = new Vector3(0, 0, 0);
    //                                }
    //                                else if (transform.position.x > target.transform.position.x) //Right side, could face left
    //                                {
    //                                    transform.localScale = new Vector3(-1, 1, 1);
    //                                    punchPos.transform.localEulerAngles = new Vector3(0, 180, 0);
    //                                }
    //                            }*/
    //                            break;
    //                    }
    //                    break;
    //                case true:
    //                    cloneModes[0].SetActive(false);
    //                    cloneModes[1].SetActive(true);
    //                    switch (targetLock) //Looking lame, Mona!
    //                    {
    //                        case false:
    //                            //Copy Paste here, the commanded code
    //                            command_Position = player_GameObject.GetComponent<PlayerScriptUzumASCII>().clonePosition.position;
    //                            if (Vector2.Distance(transform.position, new Vector2(command_Position.x, transform.position.y)) > 0)
    //                            {
    //                                clone_Animator.SetFloat("motionValue", 1f);
    //                                transform.position = Vector2.MoveTowards(transform.position, new Vector3(command_Position.x, transform.position.y, transform.position.z), movement_Speed * Time.deltaTime);
    //                            }
    //                            else
    //                            {
    //                                clone_Animator.SetFloat("motionValue", 0f);
    //                            }
    //                            if (transform.position.x < command_Position.x) //Left side, could face right //Rotation
    //                            {
    //                                transform.localScale = new Vector3(1, 1, 1);
    //                                punch_Position.transform.localEulerAngles = new Vector3(0, 180, 0);
    //                            }
    //                            else if (transform.position.x > command_Position.x) //Right side, could face left
    //                            {
    //                                transform.localScale = new Vector3(-1, 1, 1);
    //                                punch_Position.transform.localEulerAngles = new Vector3(0, 0, 0);
    //                            }
    //                            if (Input.GetKeyDown(KeyCode.Alpha2) && isCommanded && !clone_Animator.GetCurrentAnimatorStateInfo(0).IsName("Motion"))
    //                            {
    //                                game_Manager.GetComponent<Manager>().sound_Effects[12].Play();
    //                                isCommanded = false;
    //                            }
    //                            if (current_Health < maximum_Health && healing_Scroll_Count > 0)
    //                            {
    //                                healing_Scroll_Count--;
    //                                clone_Animator.SetTrigger("healWound");
    //                                current_Health = maximum_Health;
    //                            }
    //                            break;
    //                        case true:
    //                            switch (target != null && target.GetComponent<EnemyState>().health > 0)
    //                            {
    //                                case true:
    //                                    if (target == null || target.GetComponent<EnemyState>().health <= 0)
    //                                    {
    //                                        for (int i = 0; i < dh.Length; i++)
    //                                        {
    //                                            dh[i].enabled = true;
    //                                        }
    //                                        clone_Animator.SetFloat("Punch", 0f);
    //                                        targetLock = false;
    //                                    }
    //                                    switch (Vector2.Distance(transform.position, target.transform.position) > minimum_Aggro_Distance)
    //                                    {
    //                                        case true:
    //                                            clone_Animator.SetFloat("motionValue", 1f);
    //                                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), movement_Speed * Time.deltaTime);
    //                                            if (timeBtwA <= 0 && ninja_Weapon_Count > 0 && canThrow)
    //                                            {
    //                                                switch (current_Loaded_Scene_Name != "NarutoTutorial")
    //                                                {
    //                                                    case true:
    //                                                        ninja_Weapon_Count--;
    //                                                        clone_Animator.SetTrigger("throw");
    //                                                        switch (preference_Storage.GetComponent<PreferenceStorage>().weapon) //Set weapon preferences
    //                                                        {
    //                                                            case 0:
    //                                                                game_Manager.GetComponent<Manager>().sound_Effects[6].Play();
    //                                                                break;
    //                                                            case 1:
    //                                                                game_Manager.GetComponent<Manager>().sound_Effects[7].Play();
    //                                                                break;
    //                                                            case 2:
    //                                                                game_Manager.GetComponent<Manager>().sound_Effects[8].Play();
    //                                                                break;
    //                                                            case 3:
    //                                                                game_Manager.GetComponent<Manager>().sound_Effects[7].Play();
    //                                                                break;
    //                                                        }
    //                                                        Instantiate(game_Manager.GetComponent<Manager>().weapons[preference_Storage.GetComponent<PreferenceStorage>().weapon], punch_Position.position, punch_Position.transform.rotation);
    //                                                        timeBtwA = mTimeBtwA;
    //                                                        break;
    //                                                    case false:
    //                                                        ninja_Weapon_Count--;
    //                                                        clone_Animator.SetTrigger("throw");
    //                                                        game_Manager.GetComponent<Manager>().sound_Effects[6].Play();
    //                                                        Instantiate(game_Manager.GetComponent<Manager>().weapons[0], punch_Position.position, punch_Position.transform.rotation);
    //                                                        timeBtwA = mTimeBtwA;
    //                                                        break;
    //                                                }
    //                                            }
    //                                            else if (timeBtwA > 0)
    //                                            {
    //                                                timeBtwA -= Time.deltaTime;
    //                                            }
    //                                            switch (transform.position.x > target.transform.position.x)
    //                                            {
    //                                                case true:
    //                                                    transform.localScale = new Vector3(-1, 1, 1);
    //                                                    punch_Position.transform.localEulerAngles = new Vector3(0, 180, 0);
    //                                                    break;
    //                                                case false:
    //                                                    transform.localScale = new Vector3(1, 1, 1);
    //                                                    punch_Position.transform.localEulerAngles = new Vector3(0, 0, 0);
    //                                                    break;
    //                                            }
    //                                            break;
    //                                        case false:
    //                                            clone_Animator.SetFloat("motionValue", 0f);
    //                                            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y), 0);
    //                                            clone_Animator.SetFloat("Punch", 1f);
    //                                            break;
    //                                    }
    //                                    break;
    //                                case false:
    //                                    for (int i = 0; i < dh.Length; i++)
    //                                    {
    //                                            dh[i].enabled = true;
    //                                    }
    //                                    clone_Animator.SetFloat("Punch", 0f);
    //                                    targetLock = false;
    //                                    break;
    //                            }
    //                            break;
    //                    }
    //                break;
    //            }
    //        }       
    //    }
    #endregion
    #region UzumASCII Boss Behaviour
    public void Boss_Behaviour()
    {

    }
    //public void NejiBossBehav()
    //{
    //    cloneModes[0].SetActive(true);
    //    if (!clone_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
    //    {
    //        if (Vector2.Distance(transform.position, boss.transform.position) > minimum_Aggro_Distance)
    //        {
    //            clone_Animator.SetFloat("Punch", 0f);
    //            clone_Animator.SetFloat("motionValue", 1f);
    //            transform.position = Vector2.MoveTowards(transform.position, new Vector2(boss.transform.position.x, transform.position.y), movement_Speed * Time.deltaTime); //Move faser, to avoid confliction
    //            if (timeBtwA <= 0 && ninja_Weapon_Count > 0 && (transform.position.x > player_GameObject.transform.position.x) && canThrow)
    //            {
    //                ninja_Weapon_Count--;
    //                clone_Animator.SetTrigger("throw");
    //                switch (preference_Storage.GetComponent<PreferenceStorage>().weapon) //Set weapon preferences
    //                {
    //                    case 0:
    //                        game_Manager.GetComponent<Manager>().sound_Effects[6].Play();
    //                        break;
    //                    case 1:
    //                        game_Manager.GetComponent<Manager>().sound_Effects[7].Play();
    //                        break;
    //                    case 2:
    //                        game_Manager.GetComponent<Manager>().sound_Effects[8].Play();
    //                        break;
    //                    case 3:
    //                        game_Manager.GetComponent<Manager>().sound_Effects[7].Play();
    //                        break;
    //                }
    //                Instantiate(game_Manager.GetComponent<Manager>().weapons[0], punch_Position.position, punch_Position.transform.rotation);
    //                timeBtwA = mTimeBtwA;
    //            }
    //            else if (timeBtwA > 0)
    //            {
    //                timeBtwA -= Time.deltaTime;
    //            }
    //        }
    //        else if (Vector2.Distance(transform.position, boss.transform.position) < minimum_Aggro_Distance) //Give it a damn rest!
    //        {
    //            clone_Animator.SetFloat("Punch", 1f);
    //            clone_Animator.SetFloat("motionValue", 0f);
    //            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y), 0);
    //        }
    //        if (transform.position.x < boss.transform.position.x) //Left side, could face right //Rotation
    //        {
    //            transform.localScale = new Vector3(1, 1, 1);
    //            punch_Position.transform.localEulerAngles = new Vector3(0, 0, 0);
    //        }
    //        else if (transform.position.x > boss.transform.position.x) //Right side, could face left //Bruh what the fuck are these comments?
    //        {
    //            transform.localScale = new Vector3(-1, 1, 1);
    //            punch_Position.transform.localEulerAngles = new Vector3(0, 180, 0);
    //        }
    //    }   
    //}
    #endregion
    #region UchihASCII Boss Behaviour
    //public void NarutoBossBehav()
    //{
    //    cloneModes[0].SetActive(true);
    //    //cloneModes[1].SetActive(false);
    //    if (player_GameObject.GetComponent<PlayerScriptUchihASCII>().healthCur > 0 && !clone_Animator.GetCurrentAnimatorStateInfo(0).IsTag("Non-Motion"))
    //    {
    //        if (Vector2.Distance(transform.position, player_GameObject.transform.position) > minimum_Aggro_Distance)
    //        {
    //            clone_Animator.SetFloat("Punch", 0f);
    //            clone_Animator.SetFloat("motionValue", 1f);
    //            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y), (movement_Speed - 3) * Time.deltaTime); //Move faser, to avoid confliction
    //            if (timeBtwA <= 0 && ninja_Weapon_Count > 0 && canThrow)
    //            {
    //                ninja_Weapon_Count--;
    //                clone_Animator.SetTrigger("throw");
    //                game_Manager.GetComponent<Manager>().sound_Effects[6].Play();
    //                Instantiate(game_Manager.GetComponent<Manager>().weapons[0], punch_Position.position, punch_Position.transform.rotation);
    //                timeBtwA = mTimeBtwA;
    //            }
    //            else if (timeBtwA > 0)
    //            {
    //                timeBtwA -= Time.deltaTime;
    //            }
    //        }
    //        else if (Vector2.Distance(transform.position, player_GameObject.transform.position) < minimum_Aggro_Distance) //Give it a damn rest!
    //        {
    //            clone_Animator.SetFloat("Punch", 1f);
    //            clone_Animator.SetFloat("motionValue", 0f);
    //            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player_GameObject.transform.position.x, transform.position.y), 0);
    //        }
    //        if (transform.position.x < player_GameObject.transform.position.x) //Left side, could face right //Rotation
    //        {
    //            transform.localScale = new Vector3(1, 1, 1);
    //            punch_Position.transform.localEulerAngles = new Vector3(0, 0, 0);
    //        }
    //        else if (transform.position.x > player_GameObject.transform.position.x) //Right side, could face left //Bruh what the fuck are these comments?
    //        {
    //            transform.localScale = new Vector3(-1, 1, 1);
    //            punch_Position.transform.localEulerAngles = new Vector3(0, 180, 0);
    //        }
    //    }
    //}
    #endregion
    #region Training Mode Behaviour
    public void TargetRangeBehav()
    {

    }
    #endregion
    #region Tutorial Behaviour
    //public void TrainingModeNaruto() //Tuto xd
    //{
    //    AdventureModeBehav();
    //    //cloneModes[0].SetActive(true);
    //    //cloneModes[1].SetActive(false);
    //}
    #endregion
    #endregion
    public void Punch_Attack() //Adv. need one for the boss
    {
        if (game_Manager.GetComponent<Manager>().current_Loaded_Scene_Name.Contains("Difficulty"))
        {
            Collider2D[] enemiesToAttack = Physics2D.OverlapCircleAll(punch_Position.position, punch_Radius, enemy_Layer[0]);
            for (int i = 0; i < enemiesToAttack.Length; i++)
            {
                enemiesToAttack[i].GetComponent<EnemyState>().TakeDamage(damage_Amount);
            }
        }
        else if (game_Manager.GetComponent<Manager>().current_Loaded_Scene_Name == "UchihASCIIBoss")
        {
            Collider2D[] playerToAttack = Physics2D.OverlapCircleAll(punch_Position.position, punch_Radius, enemy_Layer[1]); //Player Layer
            for (int i = 0; i < playerToAttack.Length; i++)
            {
                playerToAttack[i].GetComponent<PlayerScriptUchihASCII>().TakeDamage(damage_Amount);
            }
        }
        else if (game_Manager.GetComponent<Manager>().current_Loaded_Scene_Name == "UzumASCIIBoss")
        {
            Collider2D[] bossToAttack = Physics2D.OverlapCircleAll(punch_Position.position, punch_Radius, enemy_Layer[2]); //Boss Layer
            for (int i = 0; i < bossToAttack.Length; i++)
            {
                bossToAttack[i].GetComponent<HyugASCIIBossScript>().TakeDamage(damage_Amount);
            }
        }
    }
    public void Suffer_Basic_Damage(float dmg)
    {
        health -= dmg;
        //game_Manager.GetComponent<Manager>().sound_Effects[20].Play();
    }
    public void Suffer_Specail_Damage()
    {
        health = 0;
        clone_Animator.SetTrigger("stagger");
        game_Manager.GetComponent<Manager>().instances.Remove(this.gameObject);
    }
    public void Animation_Death_Trigger()
    {
        if (health <= 0)
        {
            clone_Animator.SetTrigger("death");
            game_Manager.GetComponent<Manager>().instances.Remove(this.gameObject);
        }
    }

    //public void CheckThrowStatus()
    //{
    //    switch (current_Loaded_Scene_Name)
    //    {
    //        case "UzumASCIIBoss":
    //            if (transform.position.x > player_GameObject.transform.position.x && transform.localScale.x > 0 || transform.position.x < player_GameObject.transform.position.x && transform.localScale.x < 0 || /* I__O__I */ transform.position.x < player_GameObject.transform.position.x && boss.transform.position.x < player_GameObject.transform.position.x && transform.localScale.x > 0 || /* I__O__I */ transform.position.x > player_GameObject.transform.position.x && boss.transform.position.x > player_GameObject.transform.position.x && transform.localScale.x > 0)
    //            { //Dani just spat in the computers eyes
    //                canThrow = true;
    //            }
    //            break;
    //        case "UchihASCIIBoss":
    //            if (transform.position.x > boss.transform.position.x && transform.localScale.x > 0 || transform.position.x < boss.transform.position.x && transform.localScale.x < 0)
    //            {
    //                canThrow = true;
    //            }
    //            break;
    //        case "UzumASCIIAdventureNormal":
    //            if (targetLock)
    //            {
    //                if (transform.position.x > player_GameObject.transform.position.x && transform.localScale.x > 0 || transform.position.x < player_GameObject.transform.position.x && transform.localScale.x < 0 || transform.position.x < player_GameObject.transform.position.x && target.transform.position.x < player_GameObject.transform.position.x && transform.localScale.x > 0 || transform.position.x > player_GameObject.transform.position.x && target.transform.position.x > player_GameObject.transform.position.x && transform.localScale.x > 0)
    //                { //Dani just spat in the computers eyes
    //                    canThrow = true;
    //                }
    //            }
    //            break;
    //        case "UzumASCIIAdventureHard":
    //            if (targetLock)
    //            {
    //                if (transform.position.x > player_GameObject.transform.position.x && transform.localScale.x > 0 || transform.position.x < player_GameObject.transform.position.x && transform.localScale.x < 0 || transform.position.x < player_GameObject.transform.position.x && target.transform.position.x < player_GameObject.transform.position.x && transform.localScale.x > 0 || transform.position.x > player_GameObject.transform.position.x && target.transform.position.x > player_GameObject.transform.position.x && transform.localScale.x > 0)
    //                { //Dani just spat in the computers eyes
    //                    canThrow = true;
    //                }
    //            }
    //            break;
    //        case "UzumASCIIAdventureVeryHard":
    //            if (targetLock)
    //            {
    //                if (transform.position.x > player_GameObject.transform.position.x && transform.localScale.x > 0 || transform.position.x < player_GameObject.transform.position.x && transform.localScale.x < 0 || transform.position.x < player_GameObject.transform.position.x && target.transform.position.x < player_GameObject.transform.position.x && transform.localScale.x > 0 || transform.position.x > player_GameObject.transform.position.x && target.transform.position.x > player_GameObject.transform.position.x && transform.localScale.x > 0)
    //                { //Dani just spat in the computers eyes
    //                    canThrow = true;
    //                }
    //            }
    //            break;
    //        case "NarutoTutorial":
    //            if (targetLock)
    //            {
    //                if (transform.position.x > player_GameObject.transform.position.x && transform.localScale.x > 0 || transform.position.x < player_GameObject.transform.position.x && transform.localScale.x < 0 || transform.position.x < player_GameObject.transform.position.x && target.transform.position.x < player_GameObject.transform.position.x && transform.localScale.x > 0 || transform.position.x > player_GameObject.transform.position.x && target.transform.position.x > player_GameObject.transform.position.x && transform.localScale.x > 0)
    //                { //Dani just spat in the computers eyes
    //                    canThrow = true;
    //                }
    //            }
    //            break;
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //if (collision.CompareTag("WindProjectile"))
        //{
        //    current_Health -= (damage_Amount * 2);
        //    clone_Animator.SetTrigger("stagger");
        //}
        //if (collision.name != "GameObject" && collision.CompareTag("Yoroi")) //What the fuck ? Okay, not touching that shit
        //{
        //    current_Health -= (damage_Amount * 2.5f);
        //    clone_Animator.SetTrigger("stagger");
        //}
        switch (game_Manager.GetComponent<Manager>().current_Loaded_Scene_Name.Contains("Difficulty"))
        {
            case true:
                switch (targetLock)
                {
                    case true:
                        if (collision.CompareTag("WindProjectile"))
                        {
                            health -= (damage_Amount * 2);
                            clone_Animator.SetTrigger("stagger");
                        }
                        break;
                    case false:
                        if (collision.CompareTag("Jirobo") || collision.CompareTag("Zaku") || collision.CompareTag("Yoroi"))
                        {
                            target = collision.gameObject;
                            collision_Detector.enabled = false;
                            targetLock = true;
                        }
                        break;
                }
                break;
            case false:

                break;
        }

        if (game_Manager.GetComponent<Manager>().current_Loaded_Scene_Name.Contains("Difficulty") && !targetLock)
        {
            if (collision.CompareTag("Jirobo") || collision.CompareTag("Zaku") || collision.CompareTag("Yoroi"))
            {
                target = collision.gameObject;
                collision_Detector.enabled = false;
                targetLock = true;
            }
            
        }
        //if (current_Loaded_Scene_Name == "UzumASCIIAdventureNormal" && !targetLock || current_Loaded_Scene_Name == "UzumASCIIAdventureHard" && !targetLock || current_Loaded_Scene_Name == "UzumASCIIAdventureVeryHard" && !targetLock || current_Loaded_Scene_Name == "NarutoTutorial" && !targetLock)
        //{
        //    if (collision.CompareTag("Jirobo") || collision.CompareTag("Zaku") || collision.CompareTag("Yoroi")) 
        //    {
        //        for (int i = 0; i < dh.Length; i++)
        //        {
        //            dh[0].enabled = false;
        //        }
        //        
        //        targetLock = true;
        //    }
        //}
        //if (collision.CompareTag("Boss") && current_Loaded_Scene_Name == "UzumASCIIBoss")
        //{
        //    boss.GetComponent<HyugASCIIBossScript>().GetComponentInChildren<HyugASCIIBossAnimation>().hAnim.SetTrigger("ComboTrigger");
        //    clone_Animator.SetTrigger("stagger");
        //}
        //if (collision.CompareTag("trigram"))
        //{
        //    current_Health -= (damage_Amount * 1.5f); //do more damage? egh fuck balance
        //    cBody.constraints = RigidbodyConstraints2D.None;
        //    clone_Animator.SetTrigger("trigramImpact");
        //    if (transform.localScale.x == -1) //Bal
        //    {
        //        cBody.AddForce(transform.right * 1250f);
        //    }
        //    else if (transform.localScale.x == 1) //Jobb
        //    {
        //        cBody.AddForce(-transform.right * 1250f);
        //    }
        //    cBody.constraints = RigidbodyConstraints2D.FreezePosition;
        //}
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punch_Position.position, punch_Radius);
    }

    //public void DestroyGMBJCT() //Ignore//OK?
    //{
    //    Destroy(gameObject);
    //}
}