// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/GameplayInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameplayInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameplayInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameplayInputs"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""f4f4f13b-10ef-4fd7-8812-55cccef72dcc"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f9c6aea1-d5a2-487b-8c8d-4c21396d7cab"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6362a3f8-bab4-4634-a427-d48aa7723a91"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PrimaryAction"",
                    ""type"": ""Button"",
                    ""id"": ""9998f3c8-ab73-49c0-ad58-8ce6f3d0493c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SecondaryAction"",
                    ""type"": ""Button"",
                    ""id"": ""fb3a5e06-e56d-4ac5-8c67-d8490699be43"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TertiaryAction"",
                    ""type"": ""Button"",
                    ""id"": ""41cfb2d8-41d4-4ef8-a526-7d0e68ae200e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""25c89d18-967d-492a-955e-de63b39c61eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3468f944-aef9-4eb8-b217-de435daaa3eb"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""143747b6-a38a-408d-837e-2169a07e5313"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c766303-44dd-48ec-9931-2ca907679d4f"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""caaa57d0-fec0-43cf-8854-02ae7ad8d109"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SecondaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f9b441f-0187-4bc9-ae85-7e1cbc92aa0e"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TertiaryAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5760452-8d7e-43fe-bcaf-9fb5d290b318"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GamePlay
        m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
        m_GamePlay_Move = m_GamePlay.FindAction("Move", throwIfNotFound: true);
        m_GamePlay_Aim = m_GamePlay.FindAction("Aim", throwIfNotFound: true);
        m_GamePlay_PrimaryAction = m_GamePlay.FindAction("PrimaryAction", throwIfNotFound: true);
        m_GamePlay_SecondaryAction = m_GamePlay.FindAction("SecondaryAction", throwIfNotFound: true);
        m_GamePlay_TertiaryAction = m_GamePlay.FindAction("TertiaryAction", throwIfNotFound: true);
        m_GamePlay_Menu = m_GamePlay.FindAction("Menu", throwIfNotFound: true);
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

    // GamePlay
    private readonly InputActionMap m_GamePlay;
    private IGamePlayActions m_GamePlayActionsCallbackInterface;
    private readonly InputAction m_GamePlay_Move;
    private readonly InputAction m_GamePlay_Aim;
    private readonly InputAction m_GamePlay_PrimaryAction;
    private readonly InputAction m_GamePlay_SecondaryAction;
    private readonly InputAction m_GamePlay_TertiaryAction;
    private readonly InputAction m_GamePlay_Menu;
    public struct GamePlayActions
    {
        private @GameplayInputs m_Wrapper;
        public GamePlayActions(@GameplayInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_GamePlay_Move;
        public InputAction @Aim => m_Wrapper.m_GamePlay_Aim;
        public InputAction @PrimaryAction => m_Wrapper.m_GamePlay_PrimaryAction;
        public InputAction @SecondaryAction => m_Wrapper.m_GamePlay_SecondaryAction;
        public InputAction @TertiaryAction => m_Wrapper.m_GamePlay_TertiaryAction;
        public InputAction @Menu => m_Wrapper.m_GamePlay_Menu;
        public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
        public void SetCallbacks(IGamePlayActions instance)
        {
            if (m_Wrapper.m_GamePlayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                @Aim.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                @PrimaryAction.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnPrimaryAction;
                @PrimaryAction.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnPrimaryAction;
                @PrimaryAction.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnPrimaryAction;
                @SecondaryAction.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSecondaryAction;
                @SecondaryAction.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSecondaryAction;
                @SecondaryAction.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnSecondaryAction;
                @TertiaryAction.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTertiaryAction;
                @TertiaryAction.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTertiaryAction;
                @TertiaryAction.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTertiaryAction;
                @Menu.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMenu;
            }
            m_Wrapper.m_GamePlayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @PrimaryAction.started += instance.OnPrimaryAction;
                @PrimaryAction.performed += instance.OnPrimaryAction;
                @PrimaryAction.canceled += instance.OnPrimaryAction;
                @SecondaryAction.started += instance.OnSecondaryAction;
                @SecondaryAction.performed += instance.OnSecondaryAction;
                @SecondaryAction.canceled += instance.OnSecondaryAction;
                @TertiaryAction.started += instance.OnTertiaryAction;
                @TertiaryAction.performed += instance.OnTertiaryAction;
                @TertiaryAction.canceled += instance.OnTertiaryAction;
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
            }
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);
    public interface IGamePlayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnPrimaryAction(InputAction.CallbackContext context);
        void OnSecondaryAction(InputAction.CallbackContext context);
        void OnTertiaryAction(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
    }
}
