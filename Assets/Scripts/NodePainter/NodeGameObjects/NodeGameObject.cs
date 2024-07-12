
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;
using UnityEngine.UIElements;


namespace Download {
    public abstract class NodeGameObject {
        protected abstract string PrefabKey { get; }
        public GameObject Prefab {
            get {
                var prefab = PrefabManager.Instance.GetPrefab(PrefabKey);
                if (!prefab) throw new System.Exception("Prefab not found");
                return prefab!;
            }
        }

        public readonly GameObject gameObject;

        public NodeGameObject(Vector3 localPosition, Transform? parent) {
            gameObject = Object.Instantiate(Prefab, new(), Quaternion.identity, parent);
            gameObject.transform.localPosition = localPosition;
        }

    }
}