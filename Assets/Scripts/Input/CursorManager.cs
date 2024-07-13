using Mizuvt.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Download {
    public class CursorManager : PersistentSingleton<CursorManager> {
        private InputSystem_Actions inputActions;
        private Vector2 currentCursorPosition;

        private CursorEventListener? hoveredObject = null;
        private CursorEventListener? subbuttonClickedObject = null;

        protected override void Awake() {
            inputActions = new InputSystem_Actions();
        }

        private void OnEnable() {
            inputActions.Cursor.Enable();
            inputActions.Cursor.Move.performed += OnCursorMove;
            inputActions.Cursor.SubButtonClick.started += OnSubbuttonClickStarted;
            inputActions.Cursor.SubButtonClick.canceled += OnSubbuttonClickCanceled;
        }

        private void OnDisable() {
            inputActions.Cursor.Move.performed -= OnCursorMove;
            inputActions.Cursor.SubButtonClick.started -= OnSubbuttonClickStarted;
            inputActions.Cursor.SubButtonClick.canceled -= OnSubbuttonClickCanceled;
            inputActions.Cursor.Disable();
        }

        private void OnCursorMove(InputAction.CallbackContext context) {
            currentCursorPosition = context.ReadValue<Vector2>();
            CheckHoverEvent(currentCursorPosition);
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

        static CursorEventListener? GetCursorEventListenerHelper(RaycastHit2D hit) {
            if (hit.collider == null) return null;
            hit.collider.gameObject.TryGetComponent<CursorEventListener>(out var cursorEventListener);
            return cursorEventListener;
        }
    }
}
