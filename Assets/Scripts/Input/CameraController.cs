using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Download {
    public class CameraController : MonoBehaviour {
        private Vector2 currentCursorPosition;
        private Camera mainCamera;
        private bool isDragging = false;

        private InputSystem_Actions inputActions;

        private void Awake() {
            inputActions = new InputSystem_Actions();
            mainCamera = GetComponent<Camera>();
        }

        private void OnEnable() {
            inputActions.Cursor.Enable();
            inputActions.Cursor.SubButtonClick.started += OnSubbuttonClickStarted;
            inputActions.Cursor.SubButtonClick.canceled += OnSubbuttonClickCanceled;
            inputActions.Cursor.Move.performed += OnCursorMove;
        }

        private void OnDisable() {
            inputActions.Cursor.SubButtonClick.started -= OnSubbuttonClickStarted;
            inputActions.Cursor.SubButtonClick.canceled -= OnSubbuttonClickCanceled;
            inputActions.Cursor.Move.performed -= OnCursorMove;
            inputActions.Cursor.Disable();
        }

        private void OnCursorMove(InputAction.CallbackContext context) {
            currentCursorPosition = context.ReadValue<Vector2>();
        }


        private void OnSubbuttonClickStarted(InputAction.CallbackContext context) {
            isDragging = true;

            var dragOrigin = mainCamera.ScreenToWorldPoint(new Vector3(currentCursorPosition.x, currentCursorPosition.y, mainCamera.nearClipPlane));
            StartCoroutine(Drag(dragOrigin));
        }

        private void OnSubbuttonClickCanceled(InputAction.CallbackContext context) {
            isDragging = false;
        }

        private IEnumerator Drag(Vector3 dragOrigin) {
            while (isDragging) {
                Vector3 currentScreenPoint = new Vector3(currentCursorPosition.x, currentCursorPosition.y, mainCamera.nearClipPlane);
                Vector3 currentPosition = mainCamera.ScreenToWorldPoint(currentScreenPoint);

                Vector3 offset = dragOrigin - currentPosition;
                mainCamera.transform.position += offset;

                // 카메라의 z 축 위치를 고정합니다.
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -10);

                yield return null;
            }
        }
    }
}
