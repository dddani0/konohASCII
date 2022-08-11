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
}