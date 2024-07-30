using UnityEngine;

public static class TransformExtensions {
    public static void AlignTransformToBounds(this Transform transform, Bounds currentBounds, Bounds targetBounds) {
        // 현재 Bounds의 중심 위치와 크기
        Vector3 currentCenter = currentBounds.center;
        Vector3 currentSize = currentBounds.size;

        // 목표 Bounds의 중심 위치와 크기
        Vector3 targetCenter = targetBounds.center;
        Vector3 targetSize = targetBounds.size;

        // 중심 위치 맞추기
        Vector3 newPosition = transform.position + (targetCenter - currentCenter);

        // 크기 맞추기
        Vector3 scaleRatio = new Vector3(
            currentSize.x != 0 ? targetSize.x / currentSize.x : 1,
            currentSize.y != 0 ? targetSize.y / currentSize.y : 1,
            currentSize.z != 0 ? targetSize.z / currentSize.z : 1
        );

        // Transform의 position과 localScale 설정
        transform.position = newPosition;
        transform.localScale = new Vector3(
            transform.localScale.x * scaleRatio.x,
            transform.localScale.y * scaleRatio.y,
            transform.localScale.z * scaleRatio.z
        );
    }
}
