using UnityEngine;


namespace Download {
    public static class NodeGameObjectsPrefab {

        public static GameObject ForestPrefab => GetPrefab("Node.Forest");
        public static GameObject FolderPrefab => GetPrefab("Node.Folder");

        private static GameObject GetPrefab(string key) {
            var prefab = PrefabManager.Instance.GetPrefab(key);
            if (!prefab) throw new System.Exception("Prefab not found");
            return prefab!;

        }
    }
}