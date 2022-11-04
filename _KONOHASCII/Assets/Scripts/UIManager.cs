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
    private GameObject playerGameObject;
    private WeaponTemplate primaryWeapon;
    private WeaponTemplate secondaryWeapon;
    
    [Tooltip("Filled image, which corresponds to the player heath. Works as inverse.")]
    public Image playerHeatlhBar;

    [Space, Tooltip("Icon, which corresponds to the current active primary weapon.")]
    public Image primaryWeaponIcon;

    [Tooltip("Icon, which corresponds to the current active secondary weapon.")]
    public Image secondaryWeaponIcon;

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
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        secondaryWeaponBranch.transform.position = DetermineSecondaryBranchPosition();
        primaryWeaponUIIconAnimator.SetBool("isWeaponEquipped", IsPrimaryWeaponVisible());
        isPrimaryWeaponPickedUp = IsPrimaryWeaponVisible();
        SetSecondaryWeaponIconUIState(secondaryWeapon);
    }

    private void FetchRudimentaryValues()
    {
        gameManager = GameObject.FindGameObjectWithTag("Gamemanager");
        for (int i = 0; i < gameManager.GetComponent<Gamemanager>().entityList.Count; i++)
        {
            GameObject temporaryEntity = gameManager.GetComponent<Gamemanager>().entityList[i];
            if (temporaryEntity.name == "Player")
                playerGameObject = temporaryEntity;
            i = gameManager.GetComponent<Gamemanager>().entityList.Count;
        }
        secondaryWeaponBranch.transform.position = secondaryWeaponBranchStartPosition.transform.position;
        primaryWeapon = playerGameObject.GetComponent<PlayerAction>().activePrimaryWeapon;
        secondaryWeapon = playerGameObject.GetComponent<PlayerAction>().activeSecondaryWeaponTemplate;
    }
    
    private void SetSecondaryWeaponIconUIState(WeaponTemplate _secondaryWeapon)
    {
        //Hotswaps between weapons
        switch (_secondaryWeapon != null)
        {
            case true:
                secondaryWeaponIcon.sprite = secondaryWeapon.weaponSprite;
                secondaryWeaponIcon.SetNativeSize();
                break;
            case false:
                secondaryWeaponIcon.sprite = nullSprite;
                break;
        }
    }

    // Start is called before the first frame update
    public void ReplacePrimaryWeaponUIIcon(Sprite _replacementSprite)
    {
        primaryWeaponIcon.sprite = _replacementSprite;
        primaryWeaponIcon.SetNativeSize();
    }

    public void ReplaceSecondaryWeaponUIIcon(Sprite _replacementSprite)
    {
        secondaryWeaponIcon.sprite = _replacementSprite;
        secondaryWeaponIcon.SetNativeSize();
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

    private bool IsSecondaryWeaponEquipped(bool _secondaryWeaponState)
    {

        return false;
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