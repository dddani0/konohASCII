using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
    [Header("Resources")] public PlayableCharacterTemplate playableCharacter;
    [Space] public PlayerAction playerAction;
    public PlayerMovement playerMovement;
    [Space] public AnimatorOverrideController playerAnimatorOverrideController;


    void Start()
    {
        FetchRudimentaryValues();
        //AssignNewPlayableCharacter(playableCharacter);
    }

    public void AssignNewPlayableCharacter(PlayableCharacterTemplate _playableCharacterTemplate)
    {
        //Call after every important variable value is fetched in PlayerAction.cs
        if (_playableCharacterTemplate)
        {
            playerMovement.peakMovementSpeed = _playableCharacterTemplate.playerSpeed;
            playerAction.maximumChakra = _playableCharacterTemplate.chakraReserve;
            playerAnimatorOverrideController = _playableCharacterTemplate.playerAnimatorOverrideController;
            playerAction.playerAnimation.defaultAnimator.runtimeAnimatorController = playerAnimatorOverrideController;
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().playerPortrait.sprite =
                _playableCharacterTemplate.headshot;
        }
        else
        {
            print("No playable character template assigned!");
        }
    }

    private void FetchRudimentaryValues()
    {
        playerAction = GetComponent<PlayerAction>();
        playerMovement = GetComponent<PlayerMovement>();
    }
}