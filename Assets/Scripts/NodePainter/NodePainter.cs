
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
    public class NodePainter : MonoBehaviour {
        const float VERTICAL_INTERVAL = 1.3f;
        const float HORIZONTAL_INTERVAL = 1.4f;
        const float DRAG_THRESHOLD_SCREEN_DISTANCE_SQUARE = 10;

        public NodeSystem.NodeSystem NodeSystem;
        public Dictionary<Node, NodeGameObject> nodeObjectMap = new();

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

            GameObject? copiedSelectedSpriteParent = null;
            selectedNode.Subscribe((selectedNode) => {
                // ROOM FOR OPTIMALIZATION
                Destroy(copiedSelectedSpriteParent);
                copiedSelectedSpriteParent = new GameObject("DragParent");
                copiedSelectedSpriteParent.SetActive(false);

                selectedNode.ForEach((nodeGameObject) => {
                    var clonedSprite = Instantiate(nodeGameObject.SpriteGameObject, copiedSelectedSpriteParent.transform);
                    clonedSprite.transform.position = nodeGameObject.SpriteGameObject.transform.position;

                });

            }).AddTo(this);

            var dragContext = click
                .Select(context => {
                    if (latestClickEnterScreenPosition == null) return null;
                    if (!selectedNode.Value.Contains(context.CursorEventListener)) return null;
                    if (Vector2.SqrMagnitude(latestClickEnterScreenPosition.Value - context.ScreenPosition) < DRAG_THRESHOLD_SCREEN_DISTANCE_SQUARE)
                        return null;
                    return context;
                })
                .DistinctUntilChanged();

            dragContext.Subscribe(context => {
                if (context == null) {
                    if (copiedSelectedSpriteParent != null) copiedSelectedSpriteParent.SetActive(false);
                    return;

                }
                if (context is ClickExitContext) {
                    if (copiedSelectedSpriteParent != null) copiedSelectedSpriteParent.SetActive(false);

                    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(context.ScreenPosition);
                    RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
                    if (hit.collider == null) return;
                    hit.collider.gameObject.TryGetComponent<ICursorEventListener>(out var cursorEventListener);
                    if (cursorEventListener is not NodeGameObject nodeGameObject) return;
                    if (nodeGameObject.Node is not Folder folder) return;
                    selectedNode.Value.ForEach((node) => { node.Node?.SetParent(folder); });
                    return;
                }
                if (latestClickEnterScreenPosition == null) return;
                if (copiedSelectedSpriteParent != null) {
                    copiedSelectedSpriteParent.transform.position =
                        context.GetWorldPosition() - (Vector2)Camera.main.ScreenToWorldPoint(latestClickEnterScreenPosition.Value);
                    copiedSelectedSpriteParent.SetActive(true);
                }
            }).AddTo(this);

            #endregion Drag 
        }

        public void Initialize(NodeSystem.NodeSystem nodeSystem) {

            nodeSystem.NodeExistenceEvent.Subscribe(nodeEvent => {
                switch (nodeEvent) {
                    case NodeExistenceEventCreate nodeEventCreate: {
                            var node = nodeEventCreate.Node;
                            if (node.Parent == null) {
                                if (node is not Folder folder) throw new Exception("only root folder can have null parent");
                            }
                            var prefab = NodeGameObjectsPrefab.GetPrefabByNode(node);
                            var gameObject = Instantiate(prefab);
                            var nodeGameObject = gameObject.GetComponent<NodeGameObject>();

                            var onClickEnter = new Action<NodeGameObject>((nodeGameObject) => {
                                if (ButtonManager.Instance.ShiftPressed.Value) {
                                    GameManager.Instance.SelectedNode.Value = GameManager.Instance.SelectedNode.Value.Add(nodeGameObject);
                                    return;
                                }
                                GameManager.Instance.SelectedNode.Value = ImmutableOrderedSet<NodeGameObject>.Create(nodeGameObject);
                            });
                            nodeGameObject.Initialize(node, onClickEnter);
                            nodeObjectMap.Add(node, nodeGameObject);

                            SetTransform(nodeGameObject);
                            break;
                        }
                    case NodeExistenceEventDelete nodeEventDelete: {
                            var node = nodeEventDelete.Node;
                            var nodeObject = nodeObjectMap.GetValueOrDefault(node);
                            if (nodeObject == null) break;
                            Destroy(nodeObject.gameObject);
                            Reorder(nodeEventDelete.ParentRightBeforeDelete);
                            break;
                        }
                    case NodeExistenceEventParentChange nodeExistenceEventParentChange: {
                            var node = nodeExistenceEventParentChange.Node;
                            var nodeObject = nodeObjectMap[node];
                            SetTransform(nodeObject);
                            var previousParent = nodeExistenceEventParentChange.ParentPrevious;
                            Reorder(previousParent);
                            break;
                        }
                    case NodeExistenceEventMergeToItemCreatedBeforeMergeFromItemDeleted mergeToItemCreated: {
                            var mergedToItem = mergeToItemCreated.MergedToItem;
                            var mergeables = mergeToItemCreated.Mergeables;
                            var currentSelectedNode = GameManager.Instance.SelectedNode.Value.Select(nodeObject => (IMergeable)nodeObject.Node!);
                            var isAllSelectedItemIsMergeFrom = currentSelectedNode.ToHashSet().IsSubsetOf(mergeables);
                            if (!isAllSelectedItemIsMergeFrom) return;

                            GameManager.Instance.SelectedNode.Value = mergedToItem
                                                                        .Select(node => nodeObjectMap
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
                    folderGameObject.ChildrenContainer.position += Vector3.down * VERTICAL_INTERVAL;
                }
                var node = nodeGameObject.Node!;
                if (node.Parent != null) {
                    // setParent
                    var parent = nodeObjectMap[node.Parent];
                    if (parent is not FolderGameObject parentFolderGameObject) throw new Exception("only folder can be a parent");
                    nodeGameObject.transform.SetParent(parentFolderGameObject.ChildrenContainer);
                    // reorder on sibilings
                    var parentNode = parent.Node;
                    if (parentNode is not Folder folder) throw new Exception("only folder can be a parent");
                    Reorder(folder);
                }
            }
            void Reorder(Folder folder) {
                var xPositions = Utils.GenerateZeroMeanArray(folder.children.Count, HORIZONTAL_INTERVAL);
                folder.children.ForEach((child, index) => {

                    var childGameObject = nodeObjectMap[child].gameObject;
                    childGameObject.transform.localPosition = new Vector3(xPositions[index], 0, 0);
                });
            };
        }
    }
}