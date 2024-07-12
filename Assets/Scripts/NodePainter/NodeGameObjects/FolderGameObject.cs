
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;


namespace Download {
    public class FolderGameObject {
        public GameObject Prefab {
            get {
                var prefab = PrefabManager.Instance.GetPrefab("Node.Folder");
                if (!prefab) throw new System.Exception("Prefab not found");
                return prefab!;
            }
        }

        public readonly GameObject gameObject;

        public FolderGameObject(Transform? parent) {
            gameObject = Object.Instantiate(Prefab, parent);
        }

    }
}