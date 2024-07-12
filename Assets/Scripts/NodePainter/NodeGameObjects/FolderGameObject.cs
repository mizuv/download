
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;
using UnityEngine.UIElements;


namespace Download {
    public class FolderGameObject : NodeGameObject {
        protected override string PrefabKey => "Node.Folder";
        public FolderGameObject(Vector3 position, Transform? parent) : base(position, parent) { }
    }
}