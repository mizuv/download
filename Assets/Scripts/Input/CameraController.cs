using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Download {
    public class CameraController : MonoBehaviour {
        public const float ZOOM_SPEED = 0.0078125f;
        public const float MIN_ZOOM = 2f;
        public const float MAX_ZOOM = 20f;

        private Vector2 currentCursorPosition;
        private Camera mainCamera;
        private bool isDragging = false;
        protected CompositeDisposable _disposables = new();

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

            CursorManager.Instance.Wheel.Subscribe(context => {
                float scrollValue = context.ScrollValue.y;
                float currentZoom = mainCamera.orthographicSize;
                float newZoom = Mathf.Clamp(currentZoom - scrollValue * ZOOM_SPEED, MIN_ZOOM, MAX_ZOOM);

                // 줌 비율 계산
                float zoomRatio = newZoom / currentZoom;

                // 카메라의 새로운 위치 계산
                Vector3 cameraPosition = mainCamera.transform.position;
                Vector3 zoomCenterWorld = mainCamera.ScreenToWorldPoint(context.ScreenPosition);
                Vector3 offset = zoomCenterWorld - cameraPosition;
                mainCamera.transform.position = cameraPosition + offset * (1 - zoomRatio);

                // 새로운 줌 값 설정
                mainCamera.orthographicSize = newZoom;
            }).AddTo(_disposables);
        }

        private void OnDisable() {
            inputActions.Cursor.SubButtonClick.started -= OnSubbuttonClickStarted;
            inputActions.Cursor.SubButtonClick.canceled -= OnSubbuttonClickCanceled;
            inputActions.Cursor.Move.performed -= OnCursorMove;
            inputActions.Cursor.Disable();

            _disposables.Clear();
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
