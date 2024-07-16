using Mizuvt.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

namespace Download {
    public class ButtonManager : PersistentSingleton<ButtonManager> {
        private InputSystem_Actions inputActions;

        private readonly ReactiveProperty<bool> _shiftPressed = new(false);
        public IReadOnlyReactiveProperty<bool> ShiftPressed => _shiftPressed;


        protected override void Awake() {
            inputActions = new InputSystem_Actions();
        }

        private void OnEnable() {
            inputActions.Keyboard.Enable();

            inputActions.Keyboard.Shift
                .AsObservable()
                .Where(context => context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Canceled)
                .Subscribe(context => {
                    if (context.phase == InputActionPhase.Started) {
                        _shiftPressed.Value = true;
                    }
                    if (context.phase == InputActionPhase.Canceled) {
                        _shiftPressed.Value = false;
                    }
                })
                .AddTo(this);

        }

        private void OnDisable() {
            inputActions.Keyboard.Disable();
        }
    }
}
