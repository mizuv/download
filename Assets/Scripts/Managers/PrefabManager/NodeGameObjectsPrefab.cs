using System;
using Download.NodeSystem;
using UnityEngine;


namespace Download {
    public static class NodeGameObjectsPrefab {

        public static GameObject ApplePrefab { get; } = GetPrefab("Node.Apple");
        public static GameObject AutoRunnerPrefab { get; } = GetPrefab("Node.AutoRunner");
        public static GameObject AxeStonePrefab { get; } = GetPrefab("Node.AxeStone");
        public static GameObject BranchPrefab { get; } = GetPrefab("Node.Branch");
        public static GameObject CavePrefab { get; } = GetPrefab("Node.Cave");
        public static GameObject CoalPrefab { get; } = GetPrefab("Node.Coal");
        public static GameObject CoalOrePrefab { get; } = GetPrefab("Node.CoalOre");
        public static GameObject CopperIngotPrefab { get; } = GetPrefab("Node.CopperIngot");
        public static GameObject CopperOrePrefab { get; } = GetPrefab("Node.CopperOre");
        public static GameObject CopperRawPrefab { get; } = GetPrefab("Node.CopperRaw");
        public static GameObject FolderPrefab { get; } = GetPrefab("Node.Folder");
        public static GameObject ForestPrefab { get; } = GetPrefab("Node.Forest");
        public static GameObject FurnaceStonePrefab { get; } = GetPrefab("Node.FurnaceStone");
        public static GameObject IronIngotPrefab { get; } = GetPrefab("Node.IronIngot");
        public static GameObject IronOrePrefab { get; } = GetPrefab("Node.IronOre");
        public static GameObject IronRawPrefab { get; } = GetPrefab("Node.IronRaw");
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
                Branch => BranchPrefab,
                Cave => CavePrefab,
                Coal => CoalPrefab,
                CoalOre => CoalOrePrefab,
                CopperIngot => CopperIngotPrefab,
                CopperOre => CopperOrePrefab,
                CopperRaw => CopperRawPrefab,
                Forest => NodeGameObjectsPrefab.ForestPrefab,
                FurnaceStone => FurnaceStonePrefab,
                IronIngot => IronIngotPrefab,
                IronOre => IronOrePrefab,
                IronRaw => IronRawPrefab,
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