using System;
using Download.NodeSystem;
using UnityEngine;


namespace Download {
    public static class NodeGameObjectsPrefab {

        public static GameObject ApplePrefab { get; } = GetPrefab("Node.Apple");
        public static GameObject AutoRunnerPrefab { get; } = GetPrefab("Node.AutoRunner");
        public static GameObject AxeStonePrefab { get; } = GetPrefab("Node.AxeStone");
        public static GameObject CavePrefab { get; } = GetPrefab("Node.Cave");
        public static GameObject FolderPrefab { get; } = GetPrefab("Node.Folder");
        public static GameObject ForestPrefab { get; } = GetPrefab("Node.Forest");
        public static GameObject PersonPrefab { get; } = GetPrefab("Node.Person");
        public static GameObject StickPrefab { get; } = GetPrefab("Node.Stick");
        public static GameObject StonePrefab { get; } = GetPrefab("Node.Stone");
        public static GameObject TreePrefab { get; } = GetPrefab("Node.Tree");
        public static GameObject WoodPrefab { get; } = GetPrefab("Node.Wood");
        public static GameObject WoodPlatterPrefab { get; } = GetPrefab("Node.WoodPlatter");
        public static GameObject ZipPrefab { get; } = GetPrefab("Node.Zip");
        // public static GameObject IronOrePrefab => GetPrefab("Node.IronOre");
        // public static GameObject CooperOrePrefab => GetPrefab("Node.CooperOre");
        // public static GameObject StoneFurnacePrefab => GetPrefab("Node.StoneFurnace");
        // public static GameObject IronPrefab => GetPrefab("Node.Iron");
        // public static GameObject CooperPrefab => GetPrefab("Node.Cooper");

        public static GameObject GetPrefabByNode(Node node) {
            return node switch {
                Apple => ApplePrefab,
                AutoRunner => NodeGameObjectsPrefab.AutoRunnerPrefab,
                AxeStone => AxeStonePrefab,
                Cave => CavePrefab,
                Forest => NodeGameObjectsPrefab.ForestPrefab,
                Person => PersonPrefab,
                Stick => StickPrefab,
                Stone => StonePrefab,
                NodeSystem.Tree => TreePrefab,
                Wood => NodeGameObjectsPrefab.WoodPrefab,
                WoodPlatter => NodeGameObjectsPrefab.WoodPlatterPrefab,
                Zip => ZipPrefab,

                Folder => NodeGameObjectsPrefab.FolderPrefab,
                _ => throw new Exception("invalid node type")
            };
        }
        private static GameObject GetPrefab(string key) {
            return PrefabManager.Instance.GetPrefab(key);
        }
    }
}