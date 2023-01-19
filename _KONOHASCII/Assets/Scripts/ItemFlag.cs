using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemFlag : MonoBehaviour
{
    public WeaponTemplate weaponFlag;
    [Space] public int ammoCapacity;
    [Space] private SpriteRenderer spriteHandler;
    public Sprite itemFlagSprite;
    [Space] public GameObject buttonPromptSpriteHandler;
    public bool canBePickedUp;
    public string flagName;

    void Start()
    {
        FetchRudimentaryValues();
        ammoCapacity = weaponFlag.ammunition;
    }

    private void FetchRudimentaryValues()
    {
        spriteHandler = GetComponent<SpriteRenderer>();
        spriteHandler.sprite = itemFlagSprite;
        flagName = FetchFlagName();
        buttonPromptSpriteHandler.GetComponent<TMPro.TextMeshPro>().text = flagName;
    }

    public int FetchFlagType()
    {
        //Determines which item, the flag contains
        int _flagType = 0;
        if (weaponFlag)
            _flagType = 1;

        return _flagType;
    }

    public string FetchFlagName()
    {
        int _flagType = FetchFlagType();
        string _flagName = "";
        switch (_flagType)
        {
            case 1:
                _flagName = weaponFlag.weaponName;
                break;
            case 2:

                break;
        }

        return _flagName;
    }


    public void RefreshFlag()
    {
        FetchRudimentaryValues();
    }

    public void EnablePickUpPrompt(bool isAvaialable)
    {
        bool _fetchAvailability = isAvaialable;
        PromptStatus(_fetchAvailability, buttonPromptSpriteHandler);
    }

    private void PromptStatus(bool enable, GameObject prompt)
    {
        prompt.SetActive(enable);
        canBePickedUp = enable;
    }
}