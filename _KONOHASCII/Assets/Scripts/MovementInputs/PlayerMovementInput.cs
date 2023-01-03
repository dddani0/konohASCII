// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/MovementInputs/PlayerMovementInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerMovementInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerMovementInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerMovementInput"",
    ""maps"": [
        {
            ""name"": ""PlayableCharacter"",
            ""id"": ""1f7075a5-2384-448e-96b6-2a3f2471799d"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a9bc6a91-3e4a-4c3b-bf70-7e9c66d217ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""68a30bad-0823-4fe3-a22d-ae2d6a41c36d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Value"",
                    ""id"": ""c01f4a51-26fa-4a73-bdd8-53317802871b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Wall grab"",
                    ""type"": ""Button"",
                    ""id"": ""67414f0f-26b3-4d2b-8c08-77b387009e36"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Item Pick Up"",
                    ""type"": ""Button"",
                    ""id"": ""7d0bc8a8-c0b0-4163-8ca0-f706fe3904b8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""08565fd3-b96d-41d1-a4e7-ff2c14b0c0fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Primary Attack"",
                    ""type"": ""Button"",
                    ""id"": ""8ef809b8-ffe3-4307-a4b4-96cd3aa45682"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryAttack"",
                    ""type"": ""Button"",
                    ""id"": ""5e0f8db5-66d9-4645-a187-51269b6c4888"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""09706f64-38fe-4728-9995-a24c474a5d3f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Wall grip"",
                    ""type"": ""Button"",
                    ""id"": ""74597e3b-969e-4a07-ad63-dd7accb4497d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""91da6143-7933-491b-8d17-fbd04b6bb34c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""be2a0bda-5c78-4862-8d41-d18912363d65"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fa39de38-6d65-480e-9837-de44aff7a546"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b3a39575-e728-4e8a-9749-e6eecf361848"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c0b280ab-21ab-4df8-a611-ee3f046a19cb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""034baf08-b277-4802-b3aa-b79d06ff44d1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""184faa9a-e778-4504-9166-7ed2cb768138"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Wall grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a67e3102-2dba-440d-8f29-7c7cde595c31"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""Clamp(min=-1,max=1)"",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd6a18ae-f562-4a3d-b820-51a26c242463"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Item Pick Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5acf0994-7106-409e-b0f2-b5936d575e76"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0676f5d-b462-4f81-8321-e98682ba922c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Primary Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""222febfc-ec0c-4877-a94f-06ec77dcb11d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87b64bf0-5f05-4ce8-a35c-bee797f34c09"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b6fefe7-589f-419b-bafc-ef3a456d491e"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Wall grip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayableCharacter
        m_PlayableCharacter = asset.FindActionMap("PlayableCharacter", throwIfNotFound: true);
        m_PlayableCharacter_Jump = m_PlayableCharacter.FindAction("Jump", throwIfNotFound: true);
        m_PlayableCharacter_Movement = m_PlayableCharacter.FindAction("Movement", throwIfNotFound: true);
        m_PlayableCharacter_Mouse = m_PlayableCharacter.FindAction("Mouse", throwIfNotFound: true);
        m_PlayableCharacter_Wallgrab = m_PlayableCharacter.FindAction("Wall grab", throwIfNotFound: true);
        m_PlayableCharacter_ItemPickUp = m_PlayableCharacter.FindAction("Item Pick Up", throwIfNotFound: true);
        m_PlayableCharacter_Pause = m_PlayableCharacter.FindAction("Pause", throwIfNotFound: true);
        m_PlayableCharacter_PrimaryAttack = m_PlayableCharacter.FindAction("Primary Attack", throwIfNotFound: true);
        m_PlayableCharacter_SecondaryAttack = m_PlayableCharacter.FindAction("SecondaryAttack", throwIfNotFound: true);
        m_PlayableCharacter_Block = m_PlayableCharacter.FindAction("Block", throwIfNotFound: true);
        m_PlayableCharacter_Wallgrip = m_PlayableCharacter.FindAction("Wall grip", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlayableCharacter
    private readonly InputActionMap m_PlayableCharacter;
    private IPlayableCharacterActions m_PlayableCharacterActionsCallbackInterface;
    private readonly InputAction m_PlayableCharacter_Jump;
    private readonly InputAction m_PlayableCharacter_Movement;
    private readonly InputAction m_PlayableCharacter_Mouse;
    private readonly InputAction m_PlayableCharacter_Wallgrab;
    private readonly InputAction m_PlayableCharacter_ItemPickUp;
    private readonly InputAction m_PlayableCharacter_Pause;
    private readonly InputAction m_PlayableCharacter_PrimaryAttack;
    private readonly InputAction m_PlayableCharacter_SecondaryAttack;
    private readonly InputAction m_PlayableCharacter_Block;
    private readonly InputAction m_PlayableCharacter_Wallgrip;
    public struct PlayableCharacterActions
    {
        private @PlayerMovementInput m_Wrapper;
        public PlayableCharacterActions(@PlayerMovementInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_PlayableCharacter_Jump;
        public InputAction @Movement => m_Wrapper.m_PlayableCharacter_Movement;
        public InputAction @Mouse => m_Wrapper.m_PlayableCharacter_Mouse;
        public InputAction @Wallgrab => m_Wrapper.m_PlayableCharacter_Wallgrab;
        public InputAction @ItemPickUp => m_Wrapper.m_PlayableCharacter_ItemPickUp;
        public InputAction @Pause => m_Wrapper.m_PlayableCharacter_Pause;
        public InputAction @PrimaryAttack => m_Wrapper.m_PlayableCharacter_PrimaryAttack;
        public InputAction @SecondaryAttack => m_Wrapper.m_PlayableCharacter_SecondaryAttack;
        public InputAction @Block => m_Wrapper.m_PlayableCharacter_Block;
        public InputAction @Wallgrip => m_Wrapper.m_PlayableCharacter_Wallgrip;
        public InputActionMap Get() { return m_Wrapper.m_PlayableCharacter; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayableCharacterActions set) { return set.Get(); }
        public void SetCallbacks(IPlayableCharacterActions instance)
        {
            if (m_Wrapper.m_PlayableCharacterActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnJump;
                @Movement.started -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnMovement;
                @Mouse.started -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnMouse;
                @Wallgrab.started -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnWallgrab;
                @Wallgrab.performed -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnWallgrab;
                @Wallgrab.canceled -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnWallgrab;
                @ItemPickUp.started -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnItemPickUp;
                @ItemPickUp.performed -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnItemPickUp;
                @ItemPickUp.canceled -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnItemPickUp;
                @Pause.started -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnPause;
                @PrimaryAttack.started -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnPrimaryAttack;
                @PrimaryAttack.performed -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnPrimaryAttack;
                @PrimaryAttack.canceled -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnPrimaryAttack;
                @SecondaryAttack.started -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnSecondaryAttack;
                @SecondaryAttack.performed -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnSecondaryAttack;
                @SecondaryAttack.canceled -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnSecondaryAttack;
                @Block.started -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnBlock;
                @Block.performed -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnBlock;
                @Block.canceled -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnBlock;
                @Wallgrip.started -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnWallgrip;
                @Wallgrip.performed -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnWallgrip;
                @Wallgrip.canceled -= m_Wrapper.m_PlayableCharacterActionsCallbackInterface.OnWallgrip;
            }
            m_Wrapper.m_PlayableCharacterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
                @Wallgrab.started += instance.OnWallgrab;
                @Wallgrab.performed += instance.OnWallgrab;
                @Wallgrab.canceled += instance.OnWallgrab;
                @ItemPickUp.started += instance.OnItemPickUp;
                @ItemPickUp.performed += instance.OnItemPickUp;
                @ItemPickUp.canceled += instance.OnItemPickUp;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @PrimaryAttack.started += instance.OnPrimaryAttack;
                @PrimaryAttack.performed += instance.OnPrimaryAttack;
                @PrimaryAttack.canceled += instance.OnPrimaryAttack;
                @SecondaryAttack.started += instance.OnSecondaryAttack;
                @SecondaryAttack.performed += instance.OnSecondaryAttack;
                @SecondaryAttack.canceled += instance.OnSecondaryAttack;
                @Block.started += instance.OnBlock;
                @Block.performed += instance.OnBlock;
                @Block.canceled += instance.OnBlock;
                @Wallgrip.started += instance.OnWallgrip;
                @Wallgrip.performed += instance.OnWallgrip;
                @Wallgrip.canceled += instance.OnWallgrip;
            }
        }
    }
    public PlayableCharacterActions @PlayableCharacter => new PlayableCharacterActions(this);
    public interface IPlayableCharacterActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnMouse(InputAction.CallbackContext context);
        void OnWallgrab(InputAction.CallbackContext context);
        void OnItemPickUp(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnPrimaryAttack(InputAction.CallbackContext context);
        void OnSecondaryAttack(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
        void OnWallgrip(InputAction.CallbackContext context);
    }
}
