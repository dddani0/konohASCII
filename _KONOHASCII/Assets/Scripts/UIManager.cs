using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Image))]
public class UIManager : MonoBehaviour
{
    public Image playerHeatlhBar;
    [Space] public Image primaryWeaponIcon;
    public Image secondaryWeaponIcon;
    [Space] public Sprite nullSprite;
    [Space] public bool isPrimaryWeaponPickedUp;
    [Space] public Transform secondaryWeaponBranch;
    public float secondaryWeaponBranchOffset;
    [SerializeField] public Transform secondaryWeaponBranchStartPosition;
    [SerializeField] public Transform secondaryWeaponBranchOffsetPosition;

    private void Start()
    {
        secondaryWeaponBranch.transform.position = secondaryWeaponBranchStartPosition.transform.position;
    }


    private void LateUpdate()
    {
        secondaryWeaponBranch.transform.position = DetermineSecondaryBranchPosition();
        isPrimaryWeaponPickedUp = IsPrimaryWeaponVisible();
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

    private bool IsSpriteAttached(Image _image)
    {
        bool doesImageHaveSprite = _image.sprite;
        return doesImageHaveSprite;
    }

    private bool IsPrimaryWeaponVisible() //Visible, as in "is the proper sprite enabled"
    {
        bool isVisible = false;
        switch (primaryWeaponIcon.sprite.name != nullSprite.name)
        {
            case true:
                isVisible = true;
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