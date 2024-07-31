//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.8.2
//     from Assets/Scripts/Input/InputSystem_Actions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;

namespace Download
{
    public partial class @InputSystem_Actions: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputSystem_Actions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSystem_Actions"",
    ""maps"": [
        {
            ""name"": ""Cursor"",
            ""id"": ""b753854f-6db7-4f73-af75-b784cf9c620d"",
            ""actions"": [
                {
                    ""name"": ""SubButtonClick"",
                    ""type"": ""Button"",
                    ""id"": ""4681ad3d-6d90-419d-a946-ca554f0376d6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""434b93cd-1de2-46ce-8458-be510a6d2717"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""8261a7a8-3f8a-4274-a9a3-ecee6984389c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Wheel"",
                    ""type"": ""Value"",
                    ""id"": ""2862de7f-a4a8-4581-be94-256b676db847"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""11b08389-826a-46d3-adf7-fd2870a2ba37"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SubButtonClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c757772-fbcd-4ce7-b1d8-d793b9703569"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82dbc77f-7e81-4f29-b49e-31662658a1f3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a51a750f-b8d7-486d-ac1a-b37b01d1386c"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7c523b9-1d5a-4293-9fbd-87317ca0eec5"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Wheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard"",
            ""id"": ""1dd76c51-737a-4bb5-b6fd-8866974059d5"",
            ""actions"": [
                {
                    ""name"": ""Shift"",
                    ""type"": ""Button"",
                    ""id"": ""c824faee-dd53-48a8-ad11-2b1bd10b7974"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8654afc6-9e4c-4080-b905-29b4ddc26f3f"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Cursor
            m_Cursor = asset.FindActionMap("Cursor", throwIfNotFound: true);
            m_Cursor_SubButtonClick = m_Cursor.FindAction("SubButtonClick", throwIfNotFound: true);
            m_Cursor_Move = m_Cursor.FindAction("Move", throwIfNotFound: true);
            m_Cursor_Click = m_Cursor.FindAction("Click", throwIfNotFound: true);
            m_Cursor_Wheel = m_Cursor.FindAction("Wheel", throwIfNotFound: true);
            // Keyboard
            m_Keyboard = asset.FindActionMap("Keyboard", throwIfNotFound: true);
            m_Keyboard_Shift = m_Keyboard.FindAction("Shift", throwIfNotFound: true);
        }

        ~@InputSystem_Actions()
        {
            Debug.Assert(!m_Cursor.enabled, "This will cause a leak and performance issues, InputSystem_Actions.Cursor.Disable() has not been called.");
            Debug.Assert(!m_Keyboard.enabled, "This will cause a leak and performance issues, InputSystem_Actions.Keyboard.Disable() has not been called.");
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

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Cursor
        private readonly InputActionMap m_Cursor;
        private List<ICursorActions> m_CursorActionsCallbackInterfaces = new List<ICursorActions>();
        private readonly InputAction m_Cursor_SubButtonClick;
        private readonly InputAction m_Cursor_Move;
        private readonly InputAction m_Cursor_Click;
        private readonly InputAction m_Cursor_Wheel;
        public struct CursorActions
        {
            private @InputSystem_Actions m_Wrapper;
            public CursorActions(@InputSystem_Actions wrapper) { m_Wrapper = wrapper; }
            public InputAction @SubButtonClick => m_Wrapper.m_Cursor_SubButtonClick;
            public InputAction @Move => m_Wrapper.m_Cursor_Move;
            public InputAction @Click => m_Wrapper.m_Cursor_Click;
            public InputAction @Wheel => m_Wrapper.m_Cursor_Wheel;
            public InputActionMap Get() { return m_Wrapper.m_Cursor; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CursorActions set) { return set.Get(); }
            public void AddCallbacks(ICursorActions instance)
            {
                if (instance == null || m_Wrapper.m_CursorActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_CursorActionsCallbackInterfaces.Add(instance);
                @SubButtonClick.started += instance.OnSubButtonClick;
                @SubButtonClick.performed += instance.OnSubButtonClick;
                @SubButtonClick.canceled += instance.OnSubButtonClick;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
                @Wheel.started += instance.OnWheel;
                @Wheel.performed += instance.OnWheel;
                @Wheel.canceled += instance.OnWheel;
            }

            private void UnregisterCallbacks(ICursorActions instance)
            {
                @SubButtonClick.started -= instance.OnSubButtonClick;
                @SubButtonClick.performed -= instance.OnSubButtonClick;
                @SubButtonClick.canceled -= instance.OnSubButtonClick;
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @Click.started -= instance.OnClick;
                @Click.performed -= instance.OnClick;
                @Click.canceled -= instance.OnClick;
                @Wheel.started -= instance.OnWheel;
                @Wheel.performed -= instance.OnWheel;
                @Wheel.canceled -= instance.OnWheel;
            }

            public void RemoveCallbacks(ICursorActions instance)
            {
                if (m_Wrapper.m_CursorActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ICursorActions instance)
            {
                foreach (var item in m_Wrapper.m_CursorActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_CursorActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public CursorActions @Cursor => new CursorActions(this);

        // Keyboard
        private readonly InputActionMap m_Keyboard;
        private List<IKeyboardActions> m_KeyboardActionsCallbackInterfaces = new List<IKeyboardActions>();
        private readonly InputAction m_Keyboard_Shift;
        public struct KeyboardActions
        {
            private @InputSystem_Actions m_Wrapper;
            public KeyboardActions(@InputSystem_Actions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Shift => m_Wrapper.m_Keyboard_Shift;
            public InputActionMap Get() { return m_Wrapper.m_Keyboard; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(KeyboardActions set) { return set.Get(); }
            public void AddCallbacks(IKeyboardActions instance)
            {
                if (instance == null || m_Wrapper.m_KeyboardActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_KeyboardActionsCallbackInterfaces.Add(instance);
                @Shift.started += instance.OnShift;
                @Shift.performed += instance.OnShift;
                @Shift.canceled += instance.OnShift;
            }

            private void UnregisterCallbacks(IKeyboardActions instance)
            {
                @Shift.started -= instance.OnShift;
                @Shift.performed -= instance.OnShift;
                @Shift.canceled -= instance.OnShift;
            }

            public void RemoveCallbacks(IKeyboardActions instance)
            {
                if (m_Wrapper.m_KeyboardActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IKeyboardActions instance)
            {
                foreach (var item in m_Wrapper.m_KeyboardActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_KeyboardActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public KeyboardActions @Keyboard => new KeyboardActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        private int m_TouchSchemeIndex = -1;
        public InputControlScheme TouchScheme
        {
            get
            {
                if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
                return asset.controlSchemes[m_TouchSchemeIndex];
            }
        }
        private int m_JoystickSchemeIndex = -1;
        public InputControlScheme JoystickScheme
        {
            get
            {
                if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
                return asset.controlSchemes[m_JoystickSchemeIndex];
            }
        }
        private int m_XRSchemeIndex = -1;
        public InputControlScheme XRScheme
        {
            get
            {
                if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
                return asset.controlSchemes[m_XRSchemeIndex];
            }
        }
        public interface ICursorActions
        {
            void OnSubButtonClick(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnClick(InputAction.CallbackContext context);
            void OnWheel(InputAction.CallbackContext context);
        }
        public interface IKeyboardActions
        {
            void OnShift(InputAction.CallbackContext context);
        }
    }
}
