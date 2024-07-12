
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;


namespace Download {
    public class ForestGameObject {
        public GameObject Prefab {
            get {
                var prefab = PrefabManager.Instance.GetPrefab("Node.Forest");
                if (!prefab) throw new System.Exception("Prefab not found");
                return prefab!;
            }
        }

        readonly GameObject gameObject;

        public ForestGameObject(Transform? parent) {
            gameObject = Object.Instantiate(Prefab, parent);
        }

    }
}