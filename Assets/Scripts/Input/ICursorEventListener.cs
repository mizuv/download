using UnityEngine;

namespace Download {
    // monobehavior와 이것을 달고있으면 감지됩니다. 
    public interface ICursorEventListener {
        public void OnHoverEnter();
        public void OnHoverExit();

        public void OnClickEnter(Vector2 screenPosition);
        public void OnClickHold(ClickHoldContext context);
        public void OnClickExit(Vector2 screenPosition);

        public void OnSubbuttonClickEnter();
        public void OnSubbuttonClickExit();

        public bool IsDestoryed { get; }
    }

    public class ClickHoldContext {
        public readonly Vector2 screenPosition;

        public ClickHoldContext(Vector2 screenPosition) {
            this.screenPosition = screenPosition;
        }

        public Vector2 GetWorldPosition() { return Camera.main.ScreenToWorldPoint(this.screenPosition); }

    }
}
