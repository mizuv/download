
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mizuvt.Common {
    public partial class Utils {
        public static T? GetComponentByRaycast<T>(Vector2 screenPosition, int? layerMask = null) where T : class {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            RaycastHit2D hit = layerMask == null ? Physics2D.Raycast(worldPosition, Vector2.zero) : Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, layerMask.Value);
            if (hit.collider == null) return null;
            hit.collider.gameObject.TryGetComponent<T>(out var cursorEventListener);
            return cursorEventListener;
        }
    }
}