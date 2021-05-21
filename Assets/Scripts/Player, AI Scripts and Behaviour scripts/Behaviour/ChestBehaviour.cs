using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    [Header("Health points & Find player")]
    public float hitPoints;
    public GameObject player;
    Animator cAnim;
    BoxCollider2D hitbox;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hitbox = GetComponent<BoxCollider2D>();
        cAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        SetAnimation();
        CheckHitPoints();
    }

    #region Animation
    public void SetAnimation()
    {
        cAnim.SetFloat("hitPoints", hitPoints);
    }
    #endregion
    #region HitPoints & Take Damage
    public void CheckHitPoints()
    {
        if (hitPoints <=0)
        {
            hitbox.enabled = false;
        }
    }

    public void OpenChestWithBruteForce(float damage)
    {
        hitPoints -= damage;
    }
    #endregion
    #region Reward
    public void Reward()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<PlayerScriptUzumASCII>().healing_Scroll += Random.Range(0, 2);
        player.GetComponent<PlayerScriptUzumASCII>().current_Ninja_Weapon += Random.Range(0, 5);
    }
    #endregion
}
