using Mizuvt.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

namespace Download {
    public class CursorManager : PersistentSingleton<CursorManager> {
        private InputSystem_Actions inputActions;
        private Vector2 currentCursorPosition;

        private ICursorEventListener? hoveredObject = null;
        private ICursorEventListener? subbuttonClickedObject = null;
        private ICursorEventListener? clickedObject = null;

        protected override void Awake() {
            inputActions = new InputSystem_Actions();
        }

        private void OnEnable() {
            inputActions.Cursor.Enable();

            inputActions.Cursor.Move
                .AsObservable()
                .Select(context => context.ReadValue<Vector2>())
                .DistinctUntilChanged()
                .Subscribe((position) => {
                    currentCursorPosition = position;
                    CheckHoverEvent(currentCursorPosition);

                })
                .AddTo(this);

            inputActions.Cursor.Click
                .AsObservable()
                .Where(context => context.phase == InputActionPhase.Started)
                .Subscribe(OnClickStarted)
                .AddTo(this);

            inputActions.Cursor.Click
                .AsObservable()
                .Where(context => context.phase == InputActionPhase.Canceled)
                .Subscribe(OnClickCanceled)
                .AddTo(this);

            inputActions.Cursor.SubButtonClick
                .AsObservable()
                .Where(context => context.phase == InputActionPhase.Started)
                .Subscribe(OnSubbuttonClickStarted)
                .AddTo(this);

            inputActions.Cursor.SubButtonClick
                .AsObservable()
                .Where(context => context.phase == InputActionPhase.Canceled)
                .Subscribe(OnSubbuttonClickCanceled)
                .AddTo(this);
        }

        private void OnDisable() {
            inputActions.Cursor.Disable();
        }

        private void OnClickStarted(InputAction.CallbackContext context) {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(currentCursorPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            clickedObject = GetCursorEventListenerHelper(hit);
            if (clickedObject != null) {
                clickedObject.OnClickEnter();
            }
        }

        private void OnClickCanceled(InputAction.CallbackContext context) {
            var prevClickedObject = clickedObject;
            clickedObject = null;
            if (prevClickedObject != null) {
                prevClickedObject.OnClickExit();
            }
        }

        private void OnSubbuttonClickStarted(InputAction.CallbackContext context) {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(currentCursorPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            subbuttonClickedObject = GetCursorEventListenerHelper(hit);
            if (subbuttonClickedObject != null) {
                subbuttonClickedObject.OnSubbuttonClickEnter();
            }
        }

        private void OnSubbuttonClickCanceled(InputAction.CallbackContext context) {
            var prevSubbuttonClickedObject = subbuttonClickedObject;
            subbuttonClickedObject = null;
            if (prevSubbuttonClickedObject != null) {
                prevSubbuttonClickedObject.OnSubbuttonClickExit();
            }
        }

        void CheckHoverEvent(Vector2 currentCursorPosition) {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(currentCursorPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            var prevHoveredObject = hoveredObject;
            hoveredObject = GetCursorEventListenerHelper(hit);

            if (hoveredObject == null) {
                if (prevHoveredObject == null) return;
                prevHoveredObject.OnHoverExit();
                return;
            }

            // if (prevHoveredObject == hoveredObject) {
            //     prevHoveredObject.OnHover();
            //     return;
            // }

            if (prevHoveredObject != null) {
                // 이전에 호버했던 오브젝트에서 나감
                prevHoveredObject.OnHoverExit();
            }
            // 새 오브젝트로 호버
            hoveredObject.OnHoverEnter();
        }

        static ICursorEventListener? GetCursorEventListenerHelper(RaycastHit2D hit) {
            if (hit.collider == null) return null;
            hit.collider.gameObject.TryGetComponent<ICursorEventListener>(out var cursorEventListener);
            return cursorEventListener;
        }
    }
}
