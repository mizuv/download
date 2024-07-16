using System;
using UniRx;
using UnityEngine.InputSystem;

public static class InputActionExtensions {
    public static IObservable<InputAction.CallbackContext> AsObservable(this InputAction inputAction) {
        return Observable.Create<InputAction.CallbackContext>(observer => {
            void OnPerformed(InputAction.CallbackContext context) {
                observer.OnNext(context);
            }

            inputAction.started += OnPerformed;
            inputAction.performed += OnPerformed;
            inputAction.canceled += OnPerformed;

            return Disposable.Create(() => {
                inputAction.started -= OnPerformed;
                inputAction.performed -= OnPerformed;
                inputAction.canceled -= OnPerformed;
            });
        });
    }
}
