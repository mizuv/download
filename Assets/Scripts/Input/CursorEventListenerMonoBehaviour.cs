using UniRx;
using UnityEngine;

namespace Download {
    public interface ICursorEventListener {
        public Subject<ClickContext> Click { get; }
        public Subject<HoverContext> Hover { get; }
        public Subject<SubbuttonClickContext> SubbuttonClick { get; }

        public bool IsDestoryed { get; }
    }

    public abstract class CursorEventListenerMonoBehaviour : MonoBehaviour, ICursorEventListener {
        public bool IsDestoryed => this == null;

        public Subject<ClickContext> Click { get; } = new Subject<ClickContext>();
        public Subject<HoverContext> Hover { get; } = new Subject<HoverContext>();
        public Subject<SubbuttonClickContext> SubbuttonClick { get; } = new Subject<SubbuttonClickContext>();
    }

    public abstract class ClickContext {
        public readonly Vector2 ScreenPosition;

        public ClickContext(Vector2 screenPosition) {
            this.ScreenPosition = screenPosition;
        }
        public Vector2 GetWorldPosition() { return Camera.main.ScreenToWorldPoint(this.ScreenPosition); }
    }
    public class ClickEnterContext : ClickContext {
        public ClickEnterContext(Vector2 screenPosition) : base(screenPosition) { }
    }
    public class ClickHoldContext : ClickContext {
        public ClickHoldContext(Vector2 screenPosition) : base(screenPosition) {
        }
    }
    public class ClickExitContext : ClickContext {
        public ClickExitContext(Vector2 screenPosition) : base(screenPosition) { }
    }

    public abstract class HoverContext { }
    public class HoverEnterContext : HoverContext { }
    public class HoverExitContext : HoverContext { }

    public abstract class SubbuttonClickContext { }
    public class SubbuttonClickEnterContext : SubbuttonClickContext { }
    public class SubbuttonClickExitContext : SubbuttonClickContext { }
}
