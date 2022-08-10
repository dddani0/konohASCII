using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player/new Player Template", order = 2)]
public class PlayableCharacterTemplate : ScriptableObject
{
    public string playerName;
    [Header("Player action values")] public int playerHealth;

    [Tooltip("Value should be atleast 1250")]
    public float playerSpeed;

    [Space] public int chakraReserve;
    [Header("Player animation")] public AnimatorOverrideController playerAnimatorOverrideController;
}