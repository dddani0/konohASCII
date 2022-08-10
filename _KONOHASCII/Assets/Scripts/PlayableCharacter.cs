using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
    [Header("Resources")] [SerializeField] private PlayableCharacterTemplate playableCharacter;
    [Space] public PlayerAction playerAction;
    public PlayerMovement playerMovement;
    [Space] public AnimatorOverrideController playerAnimatorOverrideController;


    void Start()
    {
        FetchRudimentaryValues();
        AssignNewPlayableCharacter(playableCharacter);
    }

    private void AssignNewPlayableCharacter(PlayableCharacterTemplate _playableCharacterTemplate)
    {
        if (_playableCharacterTemplate)
        {
            playerAction.playerAnimation.defaultAnimator.runtimeAnimatorController =
                _playableCharacterTemplate.playerAnimatorOverrideController;
            playerMovement.movementSpeed = _playableCharacterTemplate.playerSpeed;
            playerAction.maximumChakra = _playableCharacterTemplate.chakraReserve;
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