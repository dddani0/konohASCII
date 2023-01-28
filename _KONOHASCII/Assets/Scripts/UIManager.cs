using System;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Image))]
public class UIManager : MonoBehaviour
{
    public GameObject gameManager;
    public PlayerAction playerAction;
    private WeaponTemplate primaryWeapon;
    private WeaponTemplate secondaryWeapon;

    [Tooltip("Filled image, which corresponds to the player heath. Works as inverse.")]
    public Image playerHeatlhBar;

    public Image playerPortrait;

    [Space, Tooltip("Icon, which corresponds to the current active primary weapon.")]
    public Image primaryWeaponIcon;

    [Tooltip("Icon, which corresponds to the current active secondary weapon.")]
    public Image secondaryWeaponIcon;

    public TMPro.TextMeshProUGUI secondaryWeaponUIText;

    [Space, Tooltip("Null sprite is temporary sprite, which stays active until it can be replaced.")]
    public Sprite nullSprite;

    [Space, Tooltip("Ipso facto")] public bool isPrimaryWeaponPickedUp;

    [Space, Tooltip("A branch, which concetanets secondary UI weapon components together")]
    public Transform secondaryWeaponBranch;

    [SerializeField] public Transform secondaryWeaponBranchStartPosition;
    [SerializeField] public Transform secondaryWeaponBranchOffsetPosition;
    [Space] public bool isPrimaryWeaponEquipped;
    public bool isSecondaryWeaponEquipped;
    [Space] public Animator primaryWeaponUIIconAnimator;

    private void Start()
    {
        FetchRudimentaryValues();
        SetDefaultSecondaryWeaponUI();
    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        secondaryWeaponBranch.transform.position = DetermineSecondaryBranchPosition();
        UpdateSecondaryWeaponFeedback();
        primaryWeaponUIIconAnimator.SetBool("isWeaponEquipped", IsPrimaryWeaponVisible());
        isPrimaryWeaponPickedUp = IsPrimaryWeaponVisible();
    }

    private void FetchRudimentaryValues()
    {
        gameManager = GameObject.FindGameObjectWithTag("Gamemanager");

        playerAction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAction>();
        secondaryWeaponBranch.transform.position = secondaryWeaponBranchStartPosition.transform.position;
        if (playerAction.activePrimaryWeapon !=
            null)
            primaryWeapon = playerAction.activePrimaryWeapon;
        if (playerAction.activeSecondaryWeapon !=
            null)
            secondaryWeapon = playerAction.activeSecondaryWeapon;
    }

    private void SetDefaultSecondaryWeaponUI()
    {
        secondaryWeaponIcon.sprite = nullSprite;
        secondaryWeaponUIText.text = "".ToString();
    }

    public void ReplacePrimaryWeaponUIIcon(Sprite _replacementSprite)
    {
        primaryWeaponIcon.sprite = _replacementSprite;
        primaryWeaponIcon.SetNativeSize();
    }

    public void ReplaceSecondaryWeaponUIIcon(Sprite _replacementSprite, string ammunition)
    {
        secondaryWeaponIcon.sprite = _replacementSprite;
        secondaryWeaponIcon.SetNativeSize();
        secondaryWeaponUIText.text = $"x {ammunition}";
    }

    public Sprite GetUISprite(Image _attachedSprite)
    {
        Sprite _currentSprite = IsSpriteAttached(_attachedSprite) ? _attachedSprite.GetComponent<Image>().sprite : null;
        return _currentSprite;
    }

    private static bool IsSpriteAttached(Image _image)
    {
        bool doesImageHaveSprite = _image.sprite;
        return doesImageHaveSprite;
    }

    private void UpdateSecondaryWeaponFeedback()
    {
        bool IsSecondaryWeaponPickedUp()
        {
            return playerAction.activeSecondaryWeapon;
        }

        if (IsSecondaryWeaponPickedUp())
            secondaryWeaponUIText.text = $"x {playerAction.secondaryWeaponAmmunition}";
        else
            SetDefaultSecondaryWeaponUI();
    }

    private bool IsPrimaryWeaponVisible() //Visible, as in "is the proper sprite enabled"
    {
        var isVisible = false;
        switch (primaryWeaponIcon.sprite.name != nullSprite.name)
        {
            case true:
                isVisible = true;
                isPrimaryWeaponEquipped = true;
                break;
        }

        return isVisible;
    }

    private Vector3 DetermineSecondaryBranchPosition()
    {
        bool _isPrimaryWeaponEquipped = isPrimaryWeaponPickedUp;
        Vector3 _secondaryBranchNewPosition = _isPrimaryWeaponEquipped
            ? secondaryWeaponBranchOffsetPosition.position
            : secondaryWeaponBranchStartPosition.position;
        return _secondaryBranchNewPosition;
    }
}