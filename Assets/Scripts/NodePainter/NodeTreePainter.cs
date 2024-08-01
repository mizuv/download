
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Mizuvt.Common;
using Download.NodeSystem;
using UnityEngine;
using System;
using System.Collections;
using UniRx;


namespace Download {
    public class NodeTreePainter : MonoBehaviour, INodePainter {
        public const float VERTICAL_INTERVAL = 1.3f;
        public const float HORIZONTAL_INTERVAL = 0.4f;
        public const float DRAG_THRESHOLD_SCREEN_DISTANCE_SQUARE = 10;
        public const float CHILDREN_GROUP_PADDING = 0.2f;
        public const float CHILDREN_GROUP_WIDTH = 1;
        public const float NODE_SIZE = 1;

        private int DRAG_LAYER_MASK;

        public NodeSystem.NodeSystem NodeSystem;
        private readonly Dictionary<Node, NodeGameObject> NodeObjectMap = new();

        private IReadOnlyReactiveProperty<ClickContext?> dragContextReactive;

        public void Awake() {
            DRAG_LAYER_MASK = LayerMask.GetMask("Drag", "Drag+Cursor");
        }

        public void Start() {
            #region Drag
            var click = CursorManager.Instance.Click;
            var selectedNode = GameManager.Instance.SelectedNode;
            Vector2? latestClickEnterScreenPosition = null;
            click.Where(context => context is ClickEnterContext)
                .Subscribe(context => {
                    latestClickEnterScreenPosition = context.ScreenPosition;
                })
                .AddTo(this);

            IObservable<ClickContext?> dragContext = click
                .Select(context => {
                    if (latestClickEnterScreenPosition == null) return null;
                    // if (!selectedNode.Value.Contains(context.CursorEventListener)) return null;
                    if (Vector2.SqrMagnitude(latestClickEnterScreenPosition.Value - context.ScreenPosition) < DRAG_THRESHOLD_SCREEN_DISTANCE_SQUARE)
                        return null;
                    return context;
                })
                .DistinctUntilChanged();

            dragContextReactive = dragContext.ToReactiveProperty();

            GameObject? copiedSelectedSpriteParent = null;
            var emitOnExit = dragContext.Select(context => context is ClickExitContext).DistinctUntilChanged();
            emitOnExit
                .WithLatestFrom(selectedNode, (_, lastValue) => lastValue)
                .Merge(selectedNode)
                .Subscribe((selectedNode) => {
                    // ROOM FOR OPTIMALIZATION
                    Destroy(copiedSelectedSpriteParent);
                    copiedSelectedSpriteParent = new GameObject("DragParent");
                    copiedSelectedSpriteParent.transform.SetParent(transform);
                    copiedSelectedSpriteParent.SetActive(false);

                    selectedNode.ForEach((nodeGameObject) => {
                        var clonedSprite = Instantiate(nodeGameObject.SpriteGameObject, copiedSelectedSpriteParent.transform);
                        clonedSprite.transform.position = nodeGameObject.SpriteGameObject.transform.position;
                    });
                }).AddTo(this);


            dragContext.Subscribe(context => {
                switch (context) {
                    case ClickExitContext: {
                            if (copiedSelectedSpriteParent != null) copiedSelectedSpriteParent.SetActive(false);

                            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(context.ScreenPosition);
                            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity, DRAG_LAYER_MASK);
                            if (hit.collider == null) return;
                            hit.collider.gameObject.TryGetComponent<IDragEventListener>(out var cursorEventListener);
                            if (cursorEventListener == null) return;
                            cursorEventListener.OnDrop(new DragContext(selectedNode.Value.Select(nodeObject => nodeObject.Node)));
                            return;
                        }
                    case null: {
                            if (copiedSelectedSpriteParent != null) copiedSelectedSpriteParent.SetActive(false);
                            return;
                        }
                    default: {
                            if (latestClickEnterScreenPosition == null) return;
                            // 아래 1줄은 맨 처음 드래그 시작시에만 호출하는게 맞지 않나. (귀찮아서 패스함)
                            if (!selectedNode.Value.Contains(context.CursorEventListener)) {
                                if (context.CursorEventListener is NodeGameObject nodeGameObject) Select(nodeGameObject);
                            }
                            if (copiedSelectedSpriteParent != null) {
                                copiedSelectedSpriteParent.transform.position =
                                    context.GetWorldPosition() - (Vector2)Camera.main.ScreenToWorldPoint(latestClickEnterScreenPosition.Value);
                                copiedSelectedSpriteParent.SetActive(true);
                            }
                        }
                        break;
                }
            }).AddTo(this);

            #endregion Drag 
        }

        public void Initialize(NodeSystem.NodeSystem nodeSystem) {

            nodeSystem.NodeExistenceEvent.Subscribe(nodeEvent => {
                switch (nodeEvent) {
                    case NodeCreate nodeCreate: {
                            var node = nodeCreate.Node;
                            if (node.Parent == null) {
                                if (node is not Folder folder) throw new Exception("only root folder can have null parent");
                            }
                            var prefab = NodeGameObjectsPrefab.GetPrefabByNode(node);
                            var gameObject = Instantiate(prefab, transform);
                            var nodeGameObject = gameObject.GetComponent<NodeGameObject>();

                            nodeGameObject.Initialize(node, this);
                            nodeGameObject.Click.Subscribe(context => {
                                if (context is ClickExitContext) {
                                    if (dragContextReactive.Value != null) return;
                                    this.Select(nodeGameObject);
                                }
                            }).AddTo(nodeGameObject);
                            NodeObjectMap.Add(node, nodeGameObject);

                            SetTransform(nodeGameObject);
                            break;
                        }
                    case NodeDelete nodeDelete: {
                            var node = nodeDelete.Node;
                            var nodeObject = NodeObjectMap.GetValueOrDefault(node);
                            if (nodeObject == null) break;
                            Destroy(nodeObject.gameObject);

                            var parentGameObject = GetNodeGameObject(nodeDelete.ParentRightBeforeDelete);
                            if (parentGameObject is not FolderGameObject parentFolderGameObject) throw new Exception("only folder can be a parent");
                            parentFolderGameObject.DrawChildren();
                            break;
                        }
                    case NodeParentChange nodeParentChange: {
                            var node = nodeParentChange.Node;
                            var nodeObject = NodeObjectMap[node];
                            SetTransform(nodeObject);

                            var parentGameObject = GetNodeGameObject(nodeParentChange.ParentPrevious);
                            if (parentGameObject is not FolderGameObject parentFolderGameObject) throw new Exception("only folder can be a parent");
                            parentFolderGameObject.DrawChildren();
                            break;
                        }
                    case NodeIndexChange nodeIndexChange: {
                            var folder = nodeIndexChange.IndexChangedFolder;
                            var nodeGameObject = GetNodeGameObject(folder);
                            if (nodeGameObject is not FolderGameObject folderGameObject) throw new Exception("only folder can be a parent");
                            folderGameObject.DrawChildren();
                            break;
                        }
                    case NodeExistenceEventMergeToItemCreatedBeforeMergeFromItemDeleted mergeToItemCreated: {
                            var mergedToItem = mergeToItemCreated.MergedToItem;
                            var mergeables = mergeToItemCreated.Mergeables;
                            var currentSelectedNode = GameManager.Instance.SelectedNode.Value.Select(nodeObject => (IMergeable)nodeObject.Node!);
                            var isAllSelectedItemIsMergeFrom = currentSelectedNode.ToHashSet().IsSubsetOf(mergeables);
                            if (!isAllSelectedItemIsMergeFrom) return;

                            GameManager.Instance.SelectedNode.Value = mergedToItem
                                                                        .Select(node => NodeObjectMap
                                                                        .TryGetValue(node, out var nodeObject) ? nodeObject : null)
                                                                        .Compact()
                                                                        .ToImmutableOrderedSet();
                            break;
                        }
                }
            }).AddTo(this);

            NodeSystem = nodeSystem;
            NodeSystem.Initialize();

            void SetTransform(NodeGameObject nodeGameObject) {
                if (nodeGameObject is FolderGameObject folderGameObject) {
                    folderGameObject.ChildrenContainer.localPosition = Vector3.down * VERTICAL_INTERVAL;
                }
                var node = nodeGameObject.Node!;
                if (node.Parent != null) {
                    // setParent
                    var parent = NodeObjectMap[node.Parent];
                    if (parent is not FolderGameObject parentFolderGameObject) throw new Exception("only folder can be a parent");
                    nodeGameObject.transform.SetParent(parentFolderGameObject.ChildrenContainer);
                    // reorder on sibilings
                    parentFolderGameObject.DrawChildren();
                }
            }
        }

        void Select(NodeGameObject nodeGameObject) {
            if (ButtonManager.Instance.ShiftPressed.Value) {
                GameManager.Instance.SelectedNode.Value = GameManager.Instance.SelectedNode.Value.Add(nodeGameObject);
                return;
            }
            GameManager.Instance.SelectedNode.Value = ImmutableOrderedSet<NodeGameObject>.Create(nodeGameObject);
        }

        public NodeGameObject GetNodeGameObject(Node node) {
            return NodeObjectMap[node];
        }
    }
}