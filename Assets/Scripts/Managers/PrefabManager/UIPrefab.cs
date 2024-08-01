using System;
using Download.NodeSystem;
using UnityEngine;


namespace Download {
    public static class UIPrefab {
        public static GameObject DroppableAreaPrefab { get; } = GetPrefab("UI.DroppableArea");

        private static GameObject GetPrefab(string key) {
            return PrefabManager.Instance.GetPrefab(key);
        }
    }
}