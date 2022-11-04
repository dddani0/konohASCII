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
                    ""name"": ""down"",
                    ""id"": ""c8e6bc45-ece8-4c5c-a127-b4ffec323dce"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
    public struct PlayableCharacterActions
    {
        private @PlayerMovementInput m_Wrapper;
        public PlayableCharacterActions(@PlayerMovementInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_PlayableCharacter_Jump;
        public InputAction @Movement => m_Wrapper.m_PlayableCharacter_Movement;
        public InputAction @Mouse => m_Wrapper.m_PlayableCharacter_Mouse;
        public InputAction @Wallgrab => m_Wrapper.m_PlayableCharacter_Wallgrab;
        public InputAction @ItemPickUp => m_Wrapper.m_PlayableCharacter_ItemPickUp;
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
    }
}
