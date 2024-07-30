using UnityEngine;


/// <summary>
/// 주어진 Bounds를 Gizmo로 그립니다.
/// </summary>
/// <param name="bounds">그릴 Bounds</param>
/// <param name="color">선택적 Gizmo 색상 (기본값은 빨간색)</param>
namespace Mizuvt.Common {
    public partial class Utils {
        public static void DrawBoundsGizmo(Bounds bounds, Color? color = null) {
            Color gizmoColor = color ?? Color.red; // 색상이 주어지지 않으면 빨간색을 기본값으로 사용
            Gizmos.color = gizmoColor;

            // 중심 위치와 크기를 사용하여 Gizmo로 Bounds 그리기
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}