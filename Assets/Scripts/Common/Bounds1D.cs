using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mizuvt.Common {
    // GPT가 만들어줌
    public readonly struct Bounds1D : IEquatable<Bounds1D> {
        public float Min { get; }
        public float Max { get; }

        public Bounds1D(float min, float max) {
            Min = min;
            Max = max;
        }

        // 범위 내에 특정 값이 있는지 확인
        public bool Contains(float value) {
            return value >= Min && value <= Max;
        }

        // 범위의 크기 반환
        public float Size() {
            return Max - Min;
        }

        // 범위 중심값 반환
        public float Center() {
            return (Min + Max) / 2.0f;
        }

        // 다른 범위와의 교차 여부 확인
        public bool Intersects(Bounds1D other) {
            return Min <= other.Max && Max >= other.Min;
        }

        // 특정 값을 포함하도록 범위를 확장
        public Bounds1D Encapsulate(float value) {
            float newMin = Mathf.Min(Min, value);
            float newMax = Mathf.Max(Max, value);
            return new Bounds1D(newMin, newMax);
        }

        // 다른 범위를 포함하도록 범위를 확장
        public Bounds1D Encapsulate(Bounds1D other) {
            float newMin = Mathf.Min(Min, other.Min);
            float newMax = Mathf.Max(Max, other.Max);
            return new Bounds1D(newMin, newMax);
        }

        // 범위를 문자열로 변환 (디버깅 용도)
        public override string ToString() {
            return $"Bounds1D(Min: {Min}, Max: {Max})";
        }

        public static Bounds1D Encapsulate(IEnumerable<Bounds1D> boundsList) {
            float min = float.MaxValue;
            float max = float.MinValue;

            foreach (var bounds in boundsList) {
                if (bounds.Min < min) {
                    min = bounds.Min;
                }
                if (bounds.Max > max) {
                    max = bounds.Max;
                }
            }

            return new Bounds1D(min, max);
        }

        // == 연산자 오버로드
        public static bool operator ==(Bounds1D lhs, Bounds1D rhs) {
            return lhs.Min == rhs.Min && lhs.Max == rhs.Max;
        }

        // != 연산자 오버로드
        public static bool operator !=(Bounds1D lhs, Bounds1D rhs) {
            return !(lhs == rhs);
        }

        // Equals 메서드 오버라이드
        public override bool Equals(object obj) {
            if (obj is Bounds1D) {
                return this == (Bounds1D)obj;
            }
            return false;
        }

        // IEquatable<Bounds1D> 구현
        public bool Equals(Bounds1D other) {
            return this == other;
        }

        // GetHashCode 오버라이드
        public override int GetHashCode() {
            return Min.GetHashCode() ^ Max.GetHashCode();
        }
    }
    public static class BoundsExtensions {
        public static Bounds1D ToBounds1D(this Bounds bounds, Axis axis) {
            return axis switch {
                Axis.X => new Bounds1D(bounds.min.x, bounds.max.x),
                Axis.Y => new Bounds1D(bounds.min.y, bounds.max.y),
                Axis.Z => new Bounds1D(bounds.min.z, bounds.max.z),
                _ => throw new System.ArgumentException("Invalid axis."),
            };
        }
    }
}