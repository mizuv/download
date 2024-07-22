using Mizuvt.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

namespace Download {
    public class CursorManager : PersistentSingleton<CursorManager> {
        private InputSystem_Actions inputActions;
        private IReadOnlyReactiveProperty<Vector2> currentCursorPosition;

        private ICursorEventListener? hoveredObject = null;
        private ICursorEventListener? subbuttonClickedObject = null;

        private readonly Subject<Unit> nullClickSubject = new Subject<Unit>();
        public System.IObservable<Unit> NullClick => nullClickSubject.AsObservable();


        protected override void Awake() {
            inputActions = new InputSystem_Actions();
        }

        private void OnEnable() {
            inputActions.Cursor.Enable();

            currentCursorPosition = inputActions.Cursor.Move
                .AsObservable()
                .Select(context => context.ReadValue<Vector2>())
                .DistinctUntilChanged()
                .ToReactiveProperty();

            currentCursorPosition
                .Subscribe((position) => {
                    CheckHoverEvent(position);
                })
                .AddTo(this);

            var nullCilckEventListener = new NullCursorEventListener(() => { nullClickSubject.OnNext(Unit.Default); });
            var clickedObject = inputActions.Cursor.Click
                .AsObservable()
                .Where(context => context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Canceled)
                .Select(context => {
                    if (context.phase != InputActionPhase.Started) return null;

                    if (IsPointerOverUIElement()) return null;

                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(currentCursorPosition.Value);
                    RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

                    var clickedObject = GetCursorEventListenerHelper(hit);
                    if (clickedObject == null) {
                        return nullCilckEventListener;
                    }
                    return clickedObject;
                })
                .DistinctUntilChanged();

            clickedObject
                .StartWith((ICursorEventListener?)null)
                .Pairwise()
                .CombineLatest(currentCursorPosition, (listenerPair, position) => (listenerPair, position))
                .Subscribe(pair => {
                    var previousClickedObject = pair.listenerPair.Previous;
                    var currentClickedObject = pair.listenerPair.Current;
                    var position = pair.position;

                    if (previousClickedObject == currentClickedObject) {
                        if (currentClickedObject != null && !currentClickedObject.IsDestoryed) {
                            currentClickedObject.OnClickHold(new ClickHoldContext(position));
                        }
                        return;
                    }

                    if (previousClickedObject != null && !previousClickedObject.IsDestoryed) {
                        previousClickedObject.OnClickExit(position);
                    }
                    if (currentClickedObject != null && !currentClickedObject.IsDestoryed) {
                        currentClickedObject.OnClickEnter(position);
                    }
                })
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

        private void OnSubbuttonClickStarted(InputAction.CallbackContext context) {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(currentCursorPosition.Value);
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

            if (hoveredObject == null || hoveredObject.IsDestoryed) {
                if (prevHoveredObject == null || prevHoveredObject.IsDestoryed) return;
                prevHoveredObject.OnHoverExit();
                return;
            }

            if (prevHoveredObject == hoveredObject) {
                //     prevHoveredObject.OnHover();
                return;
            }

            // 이유는 모르겠으나 ICursorEventListener타입인 prevHoveredObject를 여기서 nullcheck하면 통과가 되는데(ICursorEventListenerInstance == null 의 결과가 false)
            // 실제구현체인 monoBehavior에서 nullcheck하면 통과가 안되는 경우가 있었음.
            if (prevHoveredObject != null && !prevHoveredObject.IsDestoryed) {
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

        private List<RaycastResult> raycastResults = new List<RaycastResult>();
        private bool IsPointerOverUIElement() {
            // 현재 커서 위치에서 레이캐스트하여 UI 요소를 감지
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = currentCursorPosition.Value;
            raycastResults.Clear();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults.Count > 0;
        }

        private class NullCursorEventListener : ICursorEventListener {
            public bool IsDestoryed => false;

            public Action OnClickEnterAction;
            public NullCursorEventListener(Action onClickEnter) {
                this.OnClickEnterAction = onClickEnter;
            }

            public void OnClickEnter(Vector2 _) {
                OnClickEnterAction();
            }

            public void OnHoverEnter() { }
            public void OnHoverExit() { }
            public void OnClickHold(ClickHoldContext context) { }
            public void OnClickExit(Vector2 screenPosition) { }
            public void OnSubbuttonClickEnter() { }
            public void OnSubbuttonClickExit() { }
        }
    }
}
