using System;
using Download.NodeSystem;
using UnityEngine;


namespace Download {
    public static class NodeGameObjectsPrefab {

        public static GameObject ForestPrefab => GetPrefab("Node.Forest");
        public static GameObject FolderPrefab => GetPrefab("Node.Folder");
        public static GameObject WoodPrefab => GetPrefab("Node.Wood");
        public static GameObject WoodPlatterPrefab => GetPrefab("Node.WoodPlatter");
        public static GameObject AutoRunnerPrefab => GetPrefab("Node.AutoRunner");
        public static GameObject PersonPrefab => GetPrefab("Node.Person");
        public static GameObject ZipPrefab => GetPrefab("Node.Zip");
        public static GameObject StonePrefab => GetPrefab("Node.Stone");
        public static GameObject CavePrefab => GetPrefab("Node.Cave");
        // public static GameObject IronOrePrefab => GetPrefab("Node.IronOre");
        // public static GameObject CooperOrePrefab => GetPrefab("Node.CooperOre");
        // public static GameObject StoneFurnacePrefab => GetPrefab("Node.StoneFurnace");
        // public static GameObject IronPrefab => GetPrefab("Node.Iron");
        // public static GameObject CooperPrefab => GetPrefab("Node.Cooper");

        public static GameObject GetPrefabByNode(Node node) {
            return node switch {
                Cave => CavePrefab,
                Stone => StonePrefab,
                AutoRunner => NodeGameObjectsPrefab.AutoRunnerPrefab,
                Zip => ZipPrefab,
                Person => PersonPrefab,
                Forest => NodeGameObjectsPrefab.ForestPrefab,
                Wood => NodeGameObjectsPrefab.WoodPrefab,
                WoodPlatter => NodeGameObjectsPrefab.WoodPlatterPrefab,
                Folder => NodeGameObjectsPrefab.FolderPrefab,
                _ => throw new Exception("invalid node type")
            };
        }
        private static GameObject GetPrefab(string key) {
            return PrefabManager.Instance.GetPrefab(key);
        }
    }
}