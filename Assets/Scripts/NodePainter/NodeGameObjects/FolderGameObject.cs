
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

        readonly GameObject gameObject;

        public FolderGameObject() {
            gameObject = Object.Instantiate(Prefab);
        }

    }
}