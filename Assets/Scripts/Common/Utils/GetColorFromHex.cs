
using UnityEngine;

namespace Mizuvt.Common {
    public partial class Utils {
        public static Color GetColorFromHex(string hex) {
            if (ColorUtility.TryParseHtmlString(hex, out Color color)) {
                return color;
            } else {
                Debug.LogError("Invalid color code");
                return Color.black; // 기본값으로 검정색 반환
            }
        }
    }
}