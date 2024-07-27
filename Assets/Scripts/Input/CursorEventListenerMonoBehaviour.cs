using System;
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
        private const int DOUBLECLICK_INTERVAL = 300;

        public bool IsDestoryed => this == null;

        public Subject<ClickContext> Click { get; } = new Subject<ClickContext>();
        public Subject<Unit> DoubleClick { get; } = new Subject<Unit>();
        public Subject<HoverContext> Hover { get; } = new Subject<HoverContext>();
        public Subject<SubbuttonClickContext> SubbuttonClick { get; } = new Subject<SubbuttonClickContext>();

        virtual protected void Awake() {
            Click
                .Timestamp() // 타임스탬프를 추가하여 클릭 시간 기록
                .Pairwise()  // 연속된 두 개의 클릭을 쌍으로 묶음
                .Where(pair => pair.Current.Timestamp - pair.Previous.Timestamp < System.TimeSpan.FromMilliseconds(DOUBLECLICK_INTERVAL))
                .Subscribe(_ => DoubleClick.OnNext(Unit.Default))
                .AddTo(this);
        }
    }

    public abstract class ClickContext {
        public readonly Vector2 ScreenPosition;
        public readonly ICursorEventListener CursorEventListener;

        public ClickContext(Vector2 screenPosition, ICursorEventListener cursorEventListener) {
            this.ScreenPosition = screenPosition;
            this.CursorEventListener = cursorEventListener;
        }
        public Vector2 GetWorldPosition() { return Camera.main.ScreenToWorldPoint(this.ScreenPosition); }
    }

    public class ClickEnterContext : ClickContext {
        public ClickEnterContext(Vector2 screenPosition, ICursorEventListener cursorEventListener) : base(screenPosition, cursorEventListener) { }
    }
    public class ClickHoldContext : ClickContext {
        public ClickHoldContext(Vector2 screenPosition, ICursorEventListener cursorEventListener) : base(screenPosition, cursorEventListener) { }
    }
    public class ClickExitContext : ClickContext {
        public ClickExitContext(Vector2 screenPosition, ICursorEventListener cursorEventListener) : base(screenPosition, cursorEventListener) { }
    }

    public abstract class HoverContext { }
    public class HoverEnterContext : HoverContext { }
    public class HoverExitContext : HoverContext { }

    public abstract class SubbuttonClickContext { }
    public class SubbuttonClickEnterContext : SubbuttonClickContext { }
    public class SubbuttonClickExitContext : SubbuttonClickContext { }
}
