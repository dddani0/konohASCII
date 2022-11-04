using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemFlag : MonoBehaviour
{
    public WeaponTemplate weaponFlag;
    [Space] private SpriteRenderer spriteHandler;
    public Sprite itemFlagSprite;
    [Space] public GameObject buttonPromptSpriteHandler;
    public bool canBePickedUp;

    void Start()
    {
        spriteHandler = GetComponent<SpriteRenderer>();
        spriteHandler.sprite = itemFlagSprite;
    }

    public int FetchFlagType()
    {
        //Determines which item, the flag contains
        int _flagType = 0;
        if (weaponFlag)
            _flagType = 1;

        return _flagType;
    }

    public bool EnablePickUpPrompt(bool isAvaialable)
    {
        bool _fetchAvailability = isAvaialable;
        PromptStatus(_fetchAvailability, buttonPromptSpriteHandler);
        return _fetchAvailability;
    }

    private void PromptStatus(bool enable, GameObject prompt)
    {
        prompt.SetActive(enable);
        canBePickedUp = enable;
    }
}